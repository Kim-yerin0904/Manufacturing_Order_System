using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.ViewModels
{
    public partial class ProductCalViewModel : ObservableObject
    {
        [ObservableProperty]
        private int requiredTime;

        [ObservableProperty]
        private int actualQuantity;
    }
}
