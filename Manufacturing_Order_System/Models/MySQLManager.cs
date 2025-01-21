using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Data;

namespace Manufacturing_Order_System
{
    public class MySQLManager
    {
        public void Initialize()
        {
            Debug.WriteLine("DataBase Initialize");

            string connectionPath = $"SERVER=localhost;DATABASE=wpf;UID=root;PASSWORD=dkssud5%A";
            App.connection = new MySqlConnection(connectionPath);
        }

        // DataBase Connection
        public bool OpenMySqlConnection()
        {
            try
            {
                App.connection.Open();
                Debug.WriteLine("Success to connect to Server");
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 0:
                        Debug.WriteLine("Unable to Connect to Server");
                        break;
                    case 1045:
                        Debug.WriteLine("Please check your ID or PassWord");
                        break;
                }
                return false;
            }
        }

        // DataBase Close
        public bool CloseMySqlConnection()
        {
            try
            {
                App.connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public DataTable SqlExecute(string query)
        {
            DataTable dataTable = new DataTable();
            if (OpenMySqlConnection() == true)
            {
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter();
                mySqlDataAdapter.SelectCommand = new MySqlCommand(query);
                mySqlDataAdapter.Fill(dataTable);
            }
            return dataTable;
        }
    }
}
