using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_tp6_2024_MauroOrlando2000.Repositories;

namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class AgregarProductoViewModel
    {
        public int IdPresupuesto { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public List<SelectListItem> ListadoProductos { get; set; }

        public AgregarProductoViewModel()
        {
            List<Producto> listado = new ProductoRepository().ObtenerProductos();
            ListadoProductos = listado.Select(x => new SelectListItem
            {
                Value = x.IdProducto.ToString(),
                Text = x.Descripcion + " - " + x.Precio.ToString("c")
            }).ToList();
        }
    }
}