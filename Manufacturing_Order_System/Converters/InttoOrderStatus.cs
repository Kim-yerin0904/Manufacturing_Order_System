using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Manufacturing_Order_System.Converters
{
    public class InttoOrderStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch(value){
                case 0:
                    return "접수 대기";
                case 1:
                    return "접수 완료";
                case 2:
                    return "생산 중";
                case 3:
                    return "생산 완료";
                case 4:
                    return "출고";
                case 5:
                    return "거절";
                default:
                    return "Unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException(); 
        }
    }
}
