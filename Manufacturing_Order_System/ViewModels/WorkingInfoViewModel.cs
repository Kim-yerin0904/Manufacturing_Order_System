using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Manufacturing_Order_System.Models;
using Manufacturing_Order_System.ViewModels.Bases;

namespace Manufacturing_Order_System.ViewModels
{
    public class WorkingInfoViewModel : ViewModelBase
    {
        public ObservableCollection<Order> orders { get; set; }
        public ObservableCollection<Models.Task> tasks { get; set; }
        public ObservableCollection<Taskteam> taskteams { get; set; }
        public ObservableCollection<Worker> workers { get; set; }

        private Models.Task _selectedTask;
        public Models.Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public WorkingInfoViewModel()
        {
            orders = new ObservableCollection<Order>();
            tasks = new ObservableCollection<Models.Task>();
            taskteams = new ObservableCollection<Taskteam> { };
            workers = new ObservableCollection<Worker>();
        }

        


    }
}
