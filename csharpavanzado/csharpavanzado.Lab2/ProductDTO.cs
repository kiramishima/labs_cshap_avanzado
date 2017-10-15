using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpavanzado.Lab02
{
    class ProductDTO
    {
        public ProductDTO()
        {
            // Simular un proceso de larga duracion
            System.Threading.Thread.Sleep(1);
        }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? UnitsInStock { get; set; }
    }
}
