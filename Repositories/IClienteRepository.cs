namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public interface IClienteRepository
    {
        List<Cliente> ObtenerClientes();
        bool CrearCliente(Cliente client);
        Cliente? Buscar(int id);
        bool EliminarCliente(int id);
        bool ModificarCliente(int id, Cliente client);
    }
}