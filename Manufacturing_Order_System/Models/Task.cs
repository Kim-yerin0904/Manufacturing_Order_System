﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        public int OrderId { get; set; }
        public int TaskProductionQuantity { get; set; }
        public int TaskCompletedQuantity { get; set; }
        public int TaskteamId { get; set; }
   
    }
}
