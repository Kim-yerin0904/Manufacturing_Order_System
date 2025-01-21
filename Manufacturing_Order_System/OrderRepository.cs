using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Manufacturing_Order_System.Models;

namespace Manufacturing_Order_System
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Order> GetOrders()
        {
            var orders = new List<Order>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM wpf.order", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            OrderId = reader.GetInt32("order_id"),
                            CustomerId = reader.GetInt32("customer_id"),
                            ProductTypeId = reader.GetInt32("product_type_id"),
                            OrderQuantity = reader.GetInt32("order_quantity"),
                            OrderDate = reader.GetDateTime("order_date"),
                            OrderDueDate = DateOnly.FromDateTime(reader.GetDateTime("order_duedate")),
                            OrderStatus = reader.GetInt32("order_status"),
                            OrderReceiptDate = reader.GetDateTime("order_receipt_date")
                        });
                    }
                }
            }

            return orders;
        }

        public IEnumerable<Models.Task> GetTasksByOrderId(int orderId)
        {
            var tasks = new List<Models.Task>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM wpf.task WHERE order_id = @OrderId", connection);
                command.Parameters.AddWithValue("@OrderId", orderId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Models.Task
                        {
                            TaskId = reader.GetInt32("task_id"),
                            OrderId = reader.GetInt32("order_id"),
                            TaskProductionQuantity = reader.GetInt32("task_production_quantity"),
                            TaskCompletedQuantity = reader.GetInt32("task_completed_quantity"),
                            TaskteamId = reader.GetInt32("taskteam_id")
                        });
                    }
                }
            }

            return tasks;
        }
    }
}
