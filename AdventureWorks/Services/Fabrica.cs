namespace AdventureWorks.Services
{
    public class Fabrica : IFactoriaEspecificaciones
    {
        public IProductoEspecificacion dameInstancia(EnumeracionEjercicios ejercicio)
        {

            IProductoEspecificacion especificacion = null;
            switch (ejercicio)
            {
                case EnumeracionEjercicios.Ejercicio:
                    {
                        especificacion = new EjercicioBuilder();
                        break;
                    }
            }

            return especificacion;
        }
    }
}

