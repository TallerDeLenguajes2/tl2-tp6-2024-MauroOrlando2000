namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class Cliente
    {
        private int idCliente;
        private string nombre;
        private string email;
        private uint? telefono;

        public int IdCliente { get => idCliente; set => idCliente = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Email { get => email; set => email = value; }
        public uint? Telefono { get => telefono; set => telefono = value; }

        public Cliente(){}

        public Cliente(int id, string name, string mail, uint? phone)
        {
            idCliente = id;
            nombre = name;
            email = mail;
            telefono = phone;
        }
    }
}