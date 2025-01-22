using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.ViewModels
{
    public class OrderPageViewModel
    {
        public ObservableCollection<OrderViewModel> Orders { get; } = new();
        public ObservableCollection<ProductInfoViewModel> ProductInfo { get; } = new();
        public ObservableCollection<CustomerViewModel> CustomerInfo { get; } = new();
        public ObservableCollection<TaskteamViewModel> Taskteams { get; } = new();
        public ObservableCollection<ProductCalViewModel> ProductCal { get; } = new();
        public ObservableCollection<OrderDetailViewModel> OrderDetail { get; } = new();
    }
}
