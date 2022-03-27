using CSharpFunctionalExtensions;
using GestionFacturas.Dominio.Infra;

namespace GestionFacturas.Dominio
{
    
    public class Usuario : Entity<int>
    {
        public static readonly Usuario Nulo = new (0, "-", "-");
        public static readonly Usuario Admin = new (1, "info@bahiacode.com", "vamos a facturar");
        

        public static IReadOnlyList<Usuario> Todos = new List<Usuario> {
            Admin
        };

        public static Usuario DeId(long id)
        {
            return Todos.First(m => m.Id == id);
        }

        protected Usuario()
        {

        }

        private Usuario(int id, string email, string password) : base(id)
        {
            Email = email;
            Password = PasswordHasher.HashPassword(password);
        }

        public string Email { get; private set; } = string.Empty;

        public string Password { get; private set; } = string.Empty;

        public bool EsPasswordCorrecto(string password)
        {
            return PasswordHasher.VerifyHashedPassword(Password, password);
        }
    }
    
}