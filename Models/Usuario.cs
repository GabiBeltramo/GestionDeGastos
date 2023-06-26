using System.ComponentModel.DataAnnotations;
using BCrypt.Net;

namespace GestionDeGastos.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Password { get; set; }
        public decimal Fondo { get; set; }


        public void EncriptarContraseña()
        {
            Password = BCrypt.Net.BCrypt.HashPassword(Password);
        }

        public bool VerificarContraseña(string contraseña)
        {
            return BCrypt.Net.BCrypt.Verify(contraseña, Password);
        }
    }

}