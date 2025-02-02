using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public DateTime ProductManufactureDate { get; set; }
        public DateTime ProductShippingDate { get; set; }
        public bool ProductShippingStatus   { get; set; }
        public int OrderId { get; set; }
        public bool ProductDefectiveStatus  { get; set; }
    }
}
