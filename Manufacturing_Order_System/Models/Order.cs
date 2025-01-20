using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Manufacturing_Order_System.Models
{
    public class Order : ObservableObject
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int ProductTypeId { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime OrderDate { get; set; }
        public DateOnly OrderDueDate { get; set; }
        public int OrderStatus { get; set; }
        public DateTime OrderReceiptDate { get; set; }
    }

}
