using CSharpFunctionalExtensions;
using GestionFacturas.Dominio.Infra;

namespace GestionFacturas.Dominio
{
    
    public class Usuario : Entity<string>
    {

        protected Usuario()
        {

        }

        private Usuario(string id, string email, string password) : base(id)
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