using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.ViewModels
{
    public partial class OrderViewModel : ObservableObject
    {
        [ObservableProperty]
        private int orderId;

        [ObservableProperty]
        private int customerId;

        [ObservableProperty]
        private string productTypeName;

        [ObservableProperty]
        private int orderQuantity;

        [ObservableProperty]
        private DateTime orderDueDate;

        [ObservableProperty]
        private string orderReceiptDate;

        [ObservableProperty]
        private string orderStatus;

        [ObservableProperty]
        private DateTime orderDate;
    }
}
