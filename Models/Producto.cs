using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using tl2_tp6_2024_MauroOrlando2000.Validation;

namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class Producto
    {
        private int idProducto;
        private string descripcion;
        private int precio;

        public int IdProducto { get => idProducto; }
        [DataType(DataType.Text)]
        [StringLength(250, ErrorMessage = "La descripcion no puede ser mayor a 250 caracteres")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Required(ErrorMessage = "Debe ingresar un precio")]
        [PrecioProducto(ErrorMessage = "El precio debe ser positivo")]
        public int Precio { get => precio; set => precio = value; }

        public Producto(){}

        public Producto(int id, string des, int price)
        {
            idProducto = id;
            descripcion = des;
            precio = price;
        }

        public void CambiarID(int id)
        {
            idProducto = id;
        }
    }
}