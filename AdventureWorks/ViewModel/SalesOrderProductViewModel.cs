﻿namespace AdventureWorks.ViewModel
{
    public class SalesOrderProductViewModel
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public string ColorProducto { get; set; }
        public decimal PrecioUnitarioProducto { get; set; }
        public short Cantidad { get; set; }
        public List<SalesOrderProductViewModel> ListaAgrupada { get; set; }
    }
}

