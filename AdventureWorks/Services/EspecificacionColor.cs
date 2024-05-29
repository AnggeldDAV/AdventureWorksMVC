using AdventureWorks.Models;
using System.Drawing;

namespace AdventureWorks.Services
{
    public class EspecificacionColor : IProductoEspecificacion
    {
        public string[] colores { get; set; }
        public bool isValid(Product producto)
        {
            if (producto.Color is not null)
                return colores.Any(producto.Color.ToUpper().Contains);
            else
                return false;
        }
    }
}
