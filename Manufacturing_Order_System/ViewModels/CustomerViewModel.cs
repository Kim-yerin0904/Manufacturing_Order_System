using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        [ObservableProperty]
        private string customerName;

        [ObservableProperty]
        private string customerContact;

        [ObservableProperty]
        private string customerEmail;

        [ObservableProperty]
        private string customerAddress;
    }
}
