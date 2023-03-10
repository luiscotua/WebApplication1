using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = "Admin,Supervisor,Operador,Ninguno")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Supervisor,Operador")]
        public async Task<IActionResult> Ventas()
        {
            List<Libro> libros = new List<Libro>();
            string baseUrl = "http://200.7.103.154:99/api/posts";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                    {
                        using (HttpContent content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();
                            if (content != null)
                            {
                                libros = JsonConvert.DeserializeObject<List<Libro>>(data);
                            }
                            else
                            {
                                Console.WriteLine("NO Data----------");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception Hit------------");
                Console.WriteLine(exception);
            }
            return View(libros);
        }

        [Authorize(Roles = "Admin,Supervisor")]
        public IActionResult Compras()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Clientes()
        {
            List<User> users = new List<User>();
            string baseUrl = "http://200.7.103.154:99/api/users";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                    {
                        using (HttpContent content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();
                            if (content != null)
                            {
                                users = JsonConvert.DeserializeObject<List<User>>(data);
                            }
                            else
                            {
                                Console.WriteLine("NO Data----------");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception Hit------------");
                Console.WriteLine(exception);
            }
            return View(users);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}