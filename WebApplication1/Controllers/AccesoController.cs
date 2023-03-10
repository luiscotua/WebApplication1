using Microsoft.AspNetCore.Mvc;

using WebApplication1.Models;
using WebApplication1.Data;

//1.- REFERENCES AUTHENTICATION COOKIE
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication1.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //USAR REFERENCIAS Models y Data
        [HttpPost]
        public async Task<IActionResult> Index(Usuario _usuario)
        {
            //DA_Usuario _da_usuario = new DA_Usuario();

            //var usuario = _da_usuario.ValidarUsuario(_usuario.Correo,_usuario.Clave);

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri("http://200.7.103.154:99");
            cliente.DefaultRequestHeaders.Add("Apikey", "TKHYOADJQDUHSSPDTODUGVFWXTWPJOZF");
            cliente.DefaultRequestHeaders.Add("CodigoAplicacion", "f55dae5a-b9b9-45c6-aa9c-f417f5fecd8b");
            var content = new StringContent(JsonConvert.SerializeObject(new Models.UsuarioDto { UserName = _usuario.Correo, Password = _usuario.Clave }), Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync("/api/loginazure", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var resultado = JsonConvert.DeserializeObject<TokensDto>(jsonResponse);
                var claimsTokenAzure = await GetTokenInfo(resultado.AccesTokenActual);

                string permisos = claimsTokenAzure["role"];
                string[] permiso = permisos.Split(' ');

                //2.- CONFIGURACION DE LA AUTENTICACION
                #region AUTENTICACTION
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, claimsTokenAzure["unique_name"]),
                    new Claim("Correo",claimsTokenAzure["email"]),
                };
                foreach (string rol in permiso) {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                #endregion


                return RedirectToAction("Index", "Home");
            }
            else {
                return View();
            }
            
        }

        public async Task<IActionResult> Salir()
        {
            //3.- CONFIGURACION DE LA AUTENTICACION
            #region AUTENTICACTION
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            #endregion

            return RedirectToAction("Index");

        }

        private async Task<Dictionary<string, string>> GetTokenInfo(string token)
        {
            var TokenInfo = new Dictionary<string, string>();

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims.ToList();

            foreach (var claim in claims)
            {
                TokenInfo.Add(claim.Type, claim.Value);
            }

            return TokenInfo;
        }

    }
}
