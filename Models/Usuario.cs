using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class User
    {
        private int idUsusario;
        private string nombre;
        private string usuario;
        private string password;
        private int rol;

        public int IdUsusario { get => idUsusario; }
        [Required(ErrorMessage = "Debe ingresar su nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
        [StringLength(30, ErrorMessage = "El usuario no puede superar los 30 caracteres")]
        public string Usuario { get => usuario; set => usuario = value; }
        [Required(ErrorMessage = "Debe ingresar una contraseña")]
        [Length(5, 30, ErrorMessage = "La contraseña debe tener entre 5 y 30 caracteres")]
        public string Password { get => password; }
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas deben ser iguales")]
        public string PasswordConfirm { get; set; }
        public int Rol { get => rol; }

        public User(int id, string name, string user, string pass, int role)
        {
            idUsusario = id;
            nombre = name;
            usuario = user;
            password = pass;
            rol = role;
        }
    }
}