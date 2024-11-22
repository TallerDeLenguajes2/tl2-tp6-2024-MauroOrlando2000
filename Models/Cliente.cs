using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class Cliente
    {
        private int idCliente;
        private string nombre;
        private string email;
        private string telefono;

        public int IdCliente { get => idCliente; set => idCliente = value; }
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [EmailAddress(ErrorMessage = "Debe ingresar una dirección de email válida")]
        public string Email { get => email; set => email = value; }
        [StringLength(10, MinimumLength=10, ErrorMessage = "Debe ingresar un numero de telefono válido")]
        public string Telefono { get => telefono; set => telefono = value; }

        public Cliente(){}

        public Cliente(int id, string name, string mail, uint phone)
        {
            idCliente = id;
            nombre = name;
            email = mail;
            telefono = phone.ToString();
        }
    }
}