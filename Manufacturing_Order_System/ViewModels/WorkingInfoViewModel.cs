using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using Manufacturing_Order_System.Models;
using Manufacturing_Order_System.ViewModels.Bases;
using MySql.Data.MySqlClient;

namespace Manufacturing_Order_System.ViewModels
{
    public class WorkingInfoViewModel : ViewModelBase
    {
        public ObservableCollection<Order> orders { get; set; }
        public ObservableCollection<Models.Task> tasks { get; set; }
        public ObservableCollection<Taskteam> taskteams { get; set; }
        public ObservableCollection<Worker> workers { get; set; }
        MySQLManager manager = new MySQLManager();


        public WorkingInfoViewModel()
        {
            orders = new ObservableCollection<Order>();
            tasks = new ObservableCollection<Models.Task>();
            taskteams = new ObservableCollection<Taskteam> { };
            workers = new ObservableCollection<Worker>();
        }

        public WorkingInfoViewModel(int selected_orderid) : this() 
        {
            Workinginfo_Loaded(selected_orderid);

        }

        private void Workinginfo_Loaded(int selected_orderid)
        {
            if (manager.OpenMySqlConnection() == true)
            {
                // 주문 정보 가져오기
                string query = "select * from wpf.order where order_id=" + selected_orderid + ";";
                DataTable dataTable = manager.SqlExecute(query);
                orders.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    orders.Add(new Models.Order
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
                query = "select * from wpf.task where order_id=3;";
                tasks.Clear();
                dataTable.Clear();
                dataTable = manager.SqlExecute(query);
                foreach (DataRow row in dataTable.Rows)
                {
                    tasks.Add(new Models.Task
                    {
                        TaskId = Convert.ToInt32(row["task_id"]),
                        OrderId = Convert.ToInt32(row["order_id"]),
                        TaskProductionQuantity = Convert.ToInt32(row["task_production_quantity"]),
                        TaskCompletedQuantity = Convert.ToInt32(row["task_completed_quantity"]),
                        TaskteamId = Convert.ToInt32(row["taskteam_id"])
                    });
                }

                //작업자 정보 가져오기
                query = "select * from wpf.worker where taskteam_id=1;";
                workers.Clear();
                dataTable.Clear();
                dataTable = manager.SqlExecute(query);

                foreach (DataRow row in dataTable.Rows)
                {
                    workers.Add(new Worker
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


    }
}
