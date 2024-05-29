using AdventureWorks.Models;

namespace AdventureWorks.Services
{
    public interface IConsultaProducto
    {
        IEnumerable<Product> dameProductos(IEnumerable<Product> products);
    }
}
