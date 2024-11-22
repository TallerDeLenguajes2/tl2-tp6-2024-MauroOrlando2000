using Microsoft.AspNetCore.Mvc.Rendering;

namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class AltaPresupuestoViewModel
    {
        public int IdCliente { get; set; }
        public List<SelectListItem> ListadoClientes { get; }

        public AltaPresupuestoViewModel()
        {
            List<Cliente> listado = new ClienteRepository().ObtenerClientes();
            ListadoClientes = listado.Select(x => new SelectListItem
            {
                Value = x.IdCliente.ToString(),
                Text = x.Nombre
            }).ToList();
        }
    }
}