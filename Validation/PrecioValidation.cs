using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_MauroOrlando2000.Validation
{
    public class PrecioProductoAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value == null) return false;

            int valor = (int)value;
            return valor > 0;
        }
    }
}