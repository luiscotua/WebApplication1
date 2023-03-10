using Microsoft.Extensions.Hosting;

namespace WebApplication1.Models
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }

        public string[] Roles { get; set; }
    }

    public class UsuarioDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class TokensDto
    {
        public string AccesTokenActual { get; set; }
        public string AccessTokenLast { get; set; }
        public string Mensaje { get; set; }
        public bool IsSuccess { get; set; }

    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class Libro
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
