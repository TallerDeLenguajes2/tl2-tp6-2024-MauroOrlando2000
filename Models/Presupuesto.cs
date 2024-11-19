namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class Presupuesto
    {
        private int idPresupuesto;
        private Cliente cliente;
        private string fechaCreacion;
        private List<PresupuestoDetalle> detalle;

        public int IdPresupuesto { get => idPresupuesto; }
        public Cliente Cliente { get => cliente; }
        public string FechaCreacion { get => fechaCreacion; }
        public List<PresupuestoDetalle> Detalle { get => detalle; }
        public int IdCliente {get; set;}

        public Presupuesto()
        {
            fechaCreacion = DateOnly.FromDateTime(DateTime.Today).ToString("o");
            detalle = new List<PresupuestoDetalle>();
        }

        public Presupuesto(int idPres, Cliente client)
        {
            idPresupuesto = idPres;
            cliente = client;
            fechaCreacion = DateOnly.FromDateTime(DateTime.Today).ToString("o");
            detalle = new List<PresupuestoDetalle>();
        }

        public double MontoPresupuesto()
        {
            double monto = 0;
            foreach(PresupuestoDetalle detail in detalle)
            {
                monto += detail.Cantidad * detail.Producto.Precio;
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
                cantidad += detail.Cantidad;
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
            fechaCreacion = fecha;;
        }
    }
}