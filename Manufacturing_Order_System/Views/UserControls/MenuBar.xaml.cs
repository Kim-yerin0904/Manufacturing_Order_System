using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Manufacturing_Order_System.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
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
                        WorkingInfo workinginfo = new(1);
                        NavigationService.GetNavigationService(this)?.Navigate(workinginfo);
                        break;

                    case "stocklist_menu":
                        // 재고 목록 페이지로 이동
                        StockManager stockmanager = new StockManager();
                        NavigationService.GetNavigationService(this)?.Navigate(stockmanager);
                        break;

                    case "dailyreport_menu":
                        // 일일 실적 페이지로 이동
                        Report report = new Report();
                        NavigationService.GetNavigationService(this)?.Navigate(report);
                        break;

                    default:
                        MessageBox.Show("알 수 없는 메뉴입니다.");
                        break;
                }
            }
        }
    }
}
