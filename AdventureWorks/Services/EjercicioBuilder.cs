using AdventureWorks.Models;

namespace AdventureWorks.Services
{
    public class EjercicioBuilder : IProductoEspecificacion, IConsultaProducto
    {
        private IProductoEspecificacion especificacion;
        public EjercicioBuilder()
        {
            IProductoEspecificacion PorInicio = new EspecificacionComienzo()
            {
                letras = ["A", "B", "C"]
            };
            IProductoEspecificacion PorColor = new EspecificacionColor()
            {
                colores = ["RED", "BLACK","BLUE"]
            };
            IProductoEspecificacion PorDinero = new EspecificacionDinero()
            {
                Price = 2
            };
            IProductoEspecificacion And01 = new EspecificacionY()
            {
                Spec1 = PorColor,
                Spec2 = PorDinero,
            };
            especificacion = new EspecificacionY()
            {
                Spec1 = And01,
                Spec2 = PorInicio
            };
        }

        public IEnumerable<Product> dameProductos(IEnumerable<Product> products)
        {
            return products.Where(x => isValid(x)).OrderBy(x => x.Name);
        }

        public bool isValid(Product _producto)
        {
            return especificacion.isValid(_producto);
        }
    }
}
