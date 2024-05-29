using AdventureWorks.Models;

namespace AdventureWorks.Services
{
    public class EspecificacionY :IProductoEspecificacion
    {
        public IProductoEspecificacion Spec1 { get; set; }
        public IProductoEspecificacion Spec2 { get; set; }

        public bool isValid(Product _producto)
        {
            return Spec1.isValid(_producto) && Spec2.isValid(_producto);
        }
    }
}
