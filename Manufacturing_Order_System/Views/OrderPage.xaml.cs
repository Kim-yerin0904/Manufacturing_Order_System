using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Manufacturing_Order_System.ViewModels;
using MySql.Data.MySqlClient;
using System.Windows.Navigation;

namespace Manufacturing_Order_System.Views
{
    public partial class OrderPage : Page
    {
        private readonly MySQLManager _manager;
        public OrderPageViewModel ViewModel { get; } = new();

        public OrderPage()
        {
            InitializeComponent();

            DataContext = ViewModel;
            _manager = new MySQLManager();
            _manager.Initialize();

            LoadOrders();
            LoadTaskteams();
        }

        private void TaskTeamButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int orderId)
            {
                // 작업 정보 페이지로 이동
                WorkingInfo workinginfo = new(orderId);
                NavigationService.GetNavigationService(this)?.Navigate(workinginfo);
            }
        }

        private void OnRejectButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel.Orders.Count == 0)
                {
                    MessageBox.Show("처리할 주문이 없습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int orderId = ViewModel.OrderDetail[0].selectedOrderId;

                if (_manager.OpenMySqlConnection())
                {
                    var updateOrderStatusQuery = @"
                                                UPDATE wpf.order
                                                SET order_status = 5
                                                WHERE order_id = @OrderId";

                    var command = new MySqlCommand(updateOrderStatusQuery, App.connection);
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    command.ExecuteNonQuery();

                    MessageBox.Show("주문을 거절했습니다", "성공", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"주문 상태 업데이트 중 오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _manager.CloseMySqlConnection();
                LoadOrders();
            }
        }

        private void OnAcceptButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedTaskteam = ViewModel.Taskteams.FirstOrDefault(t => t.IsSelect);
                if (selectedTaskteam == null)
                {
                    MessageBox.Show("작업팀을 선택하세요.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int taskteamId = selectedTaskteam.TaskteamId;
                int actualQuantity = ViewModel.ProductCal[0].ActualQuantity;

                int orderId = ViewModel.OrderDetail[0].selectedOrderId;

                if (_manager.OpenMySqlConnection())
                {
                    var insertTaskQuery = @"
                                        INSERT INTO wpf.task (order_id, task_production_quantity, taskteam_id)
                                        VALUES (@OrderId, @ProductionQuantity, @TaskteamId)";

                    var insertTaskCommand = new MySqlCommand(insertTaskQuery, App.connection);
                    insertTaskCommand.Parameters.AddWithValue("@OrderId", orderId);
                    insertTaskCommand.Parameters.AddWithValue("@ProductionQuantity", actualQuantity);
                    insertTaskCommand.Parameters.AddWithValue("@TaskteamId", taskteamId);
                    insertTaskCommand.ExecuteNonQuery();

                    var updateOrderStatusAndReceiptDateQuery = @"
                        UPDATE wpf.order
                        SET order_status = 1, 
                        order_receipt_date = NOW()
                        WHERE order_id = @OrderId";

                    var updateOrderStatusCommand = new MySqlCommand(updateOrderStatusAndReceiptDateQuery, App.connection);
                    updateOrderStatusCommand.Parameters.AddWithValue("@OrderId", orderId);
                    updateOrderStatusCommand.ExecuteNonQuery();

                    var updateTaskteamStatusQuery = @"
                        UPDATE wpf.taskteam
                        SET taskteam_status = 1
                        WHERE taskteam_id = @TaskteamId";

                    var updateTaskteamStatusCommand = new MySqlCommand(updateTaskteamStatusQuery, App.connection);
                    updateTaskteamStatusCommand.Parameters.AddWithValue("@TaskteamId", taskteamId);
                    updateTaskteamStatusCommand.ExecuteNonQuery();

                    MessageBox.Show("작업이 성공적으로 저장되고 상태가 업데이트되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"작업 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _manager.CloseMySqlConnection();
                LoadOrders();
                LoadTaskteams();
            }
        }

        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int orderId)
            {
                LoadProductInfo(orderId);
                LoadCustomerData(orderId);

                ViewModel.OrderDetail.Clear();

                ViewModel.OrderDetail.Add(new OrderDetailViewModel
                {
                    selectedOrderId = orderId
                });

            }
        }

        private void OnConfirmQuantityButtonClick(object sender, RoutedEventArgs e)
        {
            string inputText = InputQuantityTextBox.Text;

            if (int.TryParse(inputText, out int inputQuantity))
            {
                ViewModel.ProductCal.Clear();

                int rt = (int)Math.Ceiling((double)inputQuantity / ViewModel.ProductInfo[0].ProductionTime);
                int aq = rt * ViewModel.ProductInfo[0].ProductionTime;

                ViewModel.ProductCal.Add(new ProductCalViewModel
                {
                    ActualQuantity = aq,
                    RequiredTime = rt
                });
            }
        }

        private static string GetOrderStatusText(int status) => status switch
        {
            0 => "접수대기",
            1 => "접수완료",
            2 => "생산중",
            3 => "생산완료",
            4 => "출고",
            5 => "거절",
            _ => "Unknown"
        };

        private void LoadOrders()
        {
            if (_manager.OpenMySqlConnection())
            {
                try
                {
                    var query = @"
                    SELECT 
                        o.order_id AS OrderId, 
                        o.customer_id AS CustomerId, 
                        o.product_type_id AS ProductTypeId, 
                        o.order_quantity AS OrderQuantity, 
                        o.order_duedate AS OrderDueDate, 
                        o.order_receipt_date AS OrderReceiptDate, 
                        o.order_status AS OrderStatus, 
                        o.order_date AS OrderDate, 
                        p.product_type_name AS ProductTypeName
                    FROM 
                        wpf.order o
                    JOIN 
                        wpf.product_type p 
                    ON 
                        o.product_type_id = p.product_type_id
                    ORDER BY
                        o.order_status, o.order_id;";

                    var command = new MySqlCommand(query, App.connection);
                    using var reader = command.ExecuteReader();

                    // ViewModel 데이터 초기화
                    ViewModel.Orders.Clear();

                    while (reader.Read())
                    {
                        ViewModel.Orders.Add(new OrderViewModel
                        {
                            OrderId = reader.GetInt32("OrderId"),
                            CustomerId = reader.GetInt32("CustomerId"),
                            ProductTypeName = reader.GetString("ProductTypeName"),
                            OrderQuantity = reader.GetInt32("OrderQuantity"),
                            OrderDueDate = reader.GetDateTime("OrderDueDate"),
                            OrderReceiptDate = reader.IsDBNull(reader.GetOrdinal("OrderReceiptDate"))
                            ? "-"
                            : reader.GetDateTime("OrderReceiptDate").ToString(),
                            OrderStatus = GetOrderStatusText(reader.GetInt32("OrderStatus")),
                            OrderDate = reader.GetDateTime("OrderDate")
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading orders: {ex.Message}");
                }
                finally
                {
                    _manager.CloseMySqlConnection();
                }
            }
        }

        private void LoadProductInfo(int orderId)
        {
            if (_manager.OpenMySqlConnection())
            {
                try
                {
                    var query = @"
                                SELECT 
                                    pt.product_type_name AS ProductTypeName,
                                    o.order_quantity AS OrderQuantity,
                                    pt.product_type_quantity AS StockQuantity,
                                    pt.product_type_eta AS ProductionTime,
                                    o.order_duedate AS DueDate
                                FROM 
                                    wpf.order o
                                JOIN 
                                    wpf.product_type pt ON o.product_type_id = pt.product_type_id
                                WHERE 
                                    o.order_id = @OrderId
                                GROUP BY 
                                    o.order_id, pt.product_type_name, o.order_quantity, pt.product_type_quantity, pt.product_type_eta, o.order_duedate";

                    var command = new MySqlCommand(query, App.connection);
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    using var reader = command.ExecuteReader();

                    ViewModel.ProductInfo.Clear();

                    while (reader.Read())
                    {
                        var productName = reader.GetString("ProductTypeName");
                        var orderQuantity = reader.GetInt32("OrderQuantity");
                        var stockQuantity = reader.GetInt32("StockQuantity");
                        var productionTime = reader.GetInt32("ProductionTime");
                        var dueDate = reader.GetDateTime("DueDate");

                        var requiredQuantity = Math.Max(0, orderQuantity - stockQuantity);
                        var remainingDays = (dueDate - DateTime.Today).Days;

                        // ViewModel에 추가
                        ViewModel.ProductInfo.Add(new ProductInfoViewModel
                        {
                            ProductName = productName,
                            OrderQuantity = orderQuantity,
                            StockQuantity = stockQuantity,
                            ProductionTime = productionTime,
                            RequiredQuantity = requiredQuantity,
                            RemainingDays = remainingDays
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading product info: {ex.Message}");
                }
                finally
                {
                    _manager.CloseMySqlConnection();
                }
            }
        }
        private void LoadCustomerData(int orderId)
        {
            if (_manager.OpenMySqlConnection())
            {
                try
                {
                    var query = @"
                SELECT 
                    c.customer_name AS CustomerName,
                    c.customer_contact AS CustomerContact,
                    c.customer_email AS CustomerEmail,
                    c.customer_address AS CustomerAddress
                FROM 
                    wpf.customer c
                JOIN 
                    wpf.order o ON c.customer_id = o.customer_id
                WHERE 
                    o.order_id = @OrderId";

                    var command = new MySqlCommand(query, App.connection);
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    using var reader = command.ExecuteReader();

                    ViewModel.CustomerInfo.Clear();

                    if (reader.Read())
                    {
                        ViewModel.CustomerInfo.Add(new CustomerViewModel
                        {
                            CustomerName = reader.GetString("CustomerName"),
                            CustomerContact = reader.GetString("CustomerContact"),
                            CustomerEmail = reader.IsDBNull(reader.GetOrdinal("CustomerEmail"))
                                ? "N/A"
                                : reader.GetString("CustomerEmail"),
                            CustomerAddress = reader.GetString("CustomerAddress")
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading customer data: {ex.Message}");
                }
                finally
                {
                    _manager.CloseMySqlConnection();
                }
            }
        }

        public void LoadTaskteams()
        {
            if (_manager.OpenMySqlConnection())
            {
                try
                {
                    var query = "SELECT taskteam_id, taskteam_name, taskteam_status FROM wpf.taskteam";
                    var command = new MySqlCommand(query, App.connection);

                    using var reader = command.ExecuteReader();
                    ViewModel.Taskteams.Clear();

                    while (reader.Read())
                    {
                        ViewModel.Taskteams.Add(new TaskteamViewModel
                        {
                            TaskteamId = reader.GetInt32("taskteam_id"),
                            TaskteamName = reader.GetString("taskteam_name"),
                            TaskStatus = reader.GetBoolean("taskteam_status")
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading task teams: {ex.Message}");
                }
                finally
                {
                    _manager.CloseMySqlConnection();
                }
            }
        }
    }
}
