using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Manufacturing_Order_System.ViewModels;
using Manufacturing_Order_System.Models;
using System.Threading.Tasks;
using Mysqlx.Crud;


namespace Manufacturing_Order_System.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WorkingInfo : Page
    {
        MySQLManager manager = new MySQLManager();
        private WorkingInfoViewModel viewModel;

        public WorkingInfo(int selected_orderid)
        {
            InitializeComponent();
            manager.Initialize();
            viewModel = new WorkingInfoViewModel(selected_orderid);
            DataContext = viewModel;
        }
    }
}