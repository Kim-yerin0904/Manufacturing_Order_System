using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Manufacturing_Order_System.Models;
using Manufacturing_Order_System.ViewModels.Bases;
using MySql.Data.MySqlClient;
using ScottPlot.Plottables;

namespace Manufacturing_Order_System.ViewModels
{
    public class ProductData
    {
        public string Name { get; set; }
        public int ProductA { get; set; }
        public int ProductB { get; set; }
        public int ProductC { get; set; }
        public int ProductD { get; set; }
        public int ProductE { get; set; }
    }

    public class RevenueData
    {
        public string Name { get; set; }
        public int ProductA { get; set; }
        public int ProductB { get; set; }
        public int ProductC { get; set; }
        public int ProductD { get; set; }
        public int ProductE { get; set; }

    }
    public class ReportViewModel : ViewModelBase
    {
        public ObservableCollection<ProductType> producttypes { get; set; }
        public ObservableCollection<ProductData> ProductTable { get; set; }
        public ObservableCollection<RevenueData> revenueDatas { get; set; }

        private decimal _totalRevenue;

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set
            {
                _totalRevenue = value;
                OnPropertyChanged(nameof(TotalRevenue));
            }
        }
        public ReportViewModel()
        {
            producttypes = new ObservableCollection<ProductType>();
            ProductTable = new ObservableCollection<ProductData>();
            revenueDatas = new ObservableCollection<RevenueData>();
            decimal TotalRevenue;
        }
    }
}
