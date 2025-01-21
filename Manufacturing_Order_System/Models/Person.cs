using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string? Email { get; set; }
        
    }

    public class Customer : Person
    {
        public string CustomerAddress { get; set; }
    }

    public class Worker : Person
    {
        public string WorkerPosition { get; set; }
    }
}
