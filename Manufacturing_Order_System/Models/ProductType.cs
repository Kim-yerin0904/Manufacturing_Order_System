using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.Models
{
    public class ProductType
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName   { get; set; }
        public int ProductTypeQuantity  { get; set; }
        public int ProductTypePrice { get; set; }
        public int ProductTypeEta { get; set; }
    }
}
