namespace tl2_tp6_2024_MauroOrlando2000.Models
{
    public class PresupuestoDetalle
    {
        private Producto producto;
        private int cantidad;
        public Producto Producto { get => producto; }
        public int Cantidad { get => cantidad; set => cantidad = value; }

        public PresupuestoDetalle(){}
        
        public PresupuestoDetalle(Producto product, int cant)
        {
            producto = product;
            cantidad = cant;
        }
    }
}