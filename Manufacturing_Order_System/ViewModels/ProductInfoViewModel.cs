using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.ViewModels
{
    public partial class ProductInfoViewModel : ObservableObject
    {
        [ObservableProperty]
        private string productName;

        [ObservableProperty]
        private int orderQuantity;

        [ObservableProperty]
        private int stockQuantity;

        [ObservableProperty]
        private int productionTime;

        [ObservableProperty]
        private int requiredQuantity; // 필요 최소 제품 수량

        [ObservableProperty]
        private int remainingDays;    // 남은일수
    }
}
