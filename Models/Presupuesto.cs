using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class Presupuesto
    {
        private int idPresupuesto;
        private string nombreDestinatario;
        private DateOnly fechaCreacion;
        private List<PresupuestoDetalle> detalle;

        public int IdPresupuesto { get => idPresupuesto; }
        public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
        public string FechaCreacion { get => fechaCreacion.ToString("o"); }
        public List<PresupuestoDetalle> Detalle { get => detalle; }

        public Presupuesto()
        {
            fechaCreacion = DateOnly.FromDateTime(DateTime.Today);
            detalle = new List<PresupuestoDetalle>();
        }

        public Presupuesto(int id, string nom)
        {
            idPresupuesto = id;
            nombreDestinatario = nom;
            fechaCreacion = DateOnly.FromDateTime(DateTime.Today);
            detalle = new List<PresupuestoDetalle>();
        }

        public double MontoPresupuesto()
        {
            double monto = 0;
            foreach(PresupuestoDetalle detail in detalle)
            {
                monto += detail.Cantidad * detail.Producto.IdProducto;
            }
            return monto;
        }

        public double MontoPresupuestoConIVA()
        {
            double monto = MontoPresupuesto() * 1.21;
            return monto;
        }

        public int CantidadProductos()
        {
            int cantidad = 0;
            foreach(PresupuestoDetalle detail in detalle)
            {
                cantidad += cantidad;
            }
            return cantidad;
        }

        public bool AgregarProducto(Producto product, int cant)
        {
            PresupuestoDetalle nuevo = new PresupuestoDetalle(product, cant);
            detalle.Add(nuevo);
            return true;
        }

        public void CambiarFecha(string fecha)
        {
            fechaCreacion = DateOnly.Parse(fecha);
        }
    }
}