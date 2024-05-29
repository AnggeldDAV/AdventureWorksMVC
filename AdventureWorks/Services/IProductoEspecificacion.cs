using AdventureWorks.Models;

namespace AdventureWorks.Services
{
    public interface IProductoEspecificacion
    {
        bool isValid(Product _producto);
    }
}
