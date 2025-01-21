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
    public partial class WorkingInfo : Window
    {
        MySQLManager manager = new MySQLManager();
        private WorkingInfoViewModel viewModel;

        public WorkingInfo()
        {
            InitializeComponent();
            manager.Initialize();
            viewModel = new WorkingInfoViewModel();
            DataContext = viewModel;
            Loaded += Workinginfo_Loaded;
            
        }
  
        private void Workinginfo_Loaded(object sender, RoutedEventArgs e)
        {
            if (manager.OpenMySqlConnection() == true)
            {
                // 주문 정보 가져오기
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
                mySqlDataAdapter.SelectCommand = new MySqlCommand("select * from wpf.order where order_id=3;", App.connection);
                viewModel.orders.Clear();
                DataTable dataTable = new DataTable();
                mySqlDataAdapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    viewModel.orders.Add(new Models.Order
                    {
                        OrderId = Convert.ToInt32(row["order_id"]),
                        CustomerId = Convert.ToInt32(row["customer_id"]),
                        ProductTypeId = Convert.ToInt32(row["product_type_id"]),
                        OrderQuantity = Convert.ToInt32(row["order_quantity"]),
                        OrderDate = row["order_date"] != DBNull.Value
                            ? DateTime.Parse(row["order_date"].ToString())
                            : default,
                        OrderDueDate = row["order_duedate"] != DBNull.Value
                               ? DateOnly.FromDateTime(DateTime.Parse(row["order_duedate"].ToString()))
                               : default,
                        OrderStatus = Convert.ToInt32(row["order_status"]),
                        OrderReceiptDate = row["order_receipt_date"] != DBNull.Value
                                   ? DateTime.Parse(row["order_receipt_date"].ToString())
                                   : default
                    });
                }

                // 작업 정보 가져오기
                mySqlDataAdapter.SelectCommand = new MySqlCommand("select * from wpf.task where order_id=3;", App.connection); 
                viewModel.tasks.Clear();
                dataTable.Clear();
                mySqlDataAdapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    viewModel.tasks.Add(new Models.Task
                    {
                        TaskId = Convert.ToInt32(row["task_id"]),
                        OrderId = Convert.ToInt32(row["order_id"]),
                        TaskProductionQuantity = Convert.ToInt32(row["task_production_quantity"]),
                        TaskCompletedQuantity = Convert.ToInt32(row["task_completed_quantity"]),
                        TaskteamId = Convert.ToInt32(row["taskteam_id"])
                    });
                }

                //작업자 정보 가져오기
                mySqlDataAdapter.SelectCommand = new MySqlCommand("select * from wpf.worker where taskteam_id=1;", App.connection);
                viewModel.workers.Clear();
                dataTable.Clear();
                mySqlDataAdapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    viewModel.workers.Add(new Worker
                    {
                        Id = Convert.ToInt32(row["worker_id"]),
                        Name = row["worker_name"].ToString(),
                        Contact = row["worker_contact"].ToString(),
                        Email = row["worker_email"] != DBNull.Value
                        ? row["worker_email"].ToString() : default,
                        WorkerPosition = row["worker_position"].ToString(),

                    });
                }
                App.connection.Close();
            }
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle clickedRectangle)
            {
                switch (clickedRectangle.Name)
                {
                    //case "orderlist_menu":
                    //    // 주문 목록 페이지로 이동
                    //    var orderListPage = new OrderListWindow(); // OrderListWindow는 새 페이지
                    //    orderListPage.Show();
                    //    this.Close(); // 현재 창 닫기
                    //    break;

                    case "workinginfo_menu":
                        // 작업 정보 페이지로 이동
                        var workingInfoPage = new WorkingInfo(); // WorkingInfoWindow는 새 페이지
                        workingInfoPage.Show();
                        this.Close(); // 현재 창 닫기
                        break;

                    //case "stocklist_menu":
                    //    // 재고 목록 페이지로 이동
                    //    var stockListPage = new StockListWindow(); // StockListWindow는 새 페이지
                    //    stockListPage.Show();
                    //    this.Close(); // 현재 창 닫기
                    //    break;

                    case "dailyreport_menu":
                        // 일일 실적 페이지로 이동
                        var dailyReportPage = new Report(); // DailyReportWindow는 새 페이지
                        dailyReportPage.Show();
                        this.Close(); // 현재 창 닫기
                        break;

                    default:
                        MessageBox.Show("알 수 없는 메뉴입니다.");
                        break;
                }
            }
        }
    }
}