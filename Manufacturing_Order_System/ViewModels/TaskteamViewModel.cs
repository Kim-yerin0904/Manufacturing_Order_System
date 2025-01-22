using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufacturing_Order_System.ViewModels
{
    public partial class TaskteamViewModel : ObservableObject
    {
        [ObservableProperty]
        private int taskteamId;

        [ObservableProperty]
        private string taskteamName = string.Empty;

        [ObservableProperty]
        private bool taskStatus;

        [ObservableProperty]
        private bool isSelect = false;

        public bool IsEnabled => !TaskStatus;
    }
}
