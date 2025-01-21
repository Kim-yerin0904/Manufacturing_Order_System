using System.Configuration;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Manufacturing_Order_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MySqlConnection connection;

        //db 결과 여부
        public static bool DataSaveResult;
        public static bool DataSearchResult;
        
    }

}
