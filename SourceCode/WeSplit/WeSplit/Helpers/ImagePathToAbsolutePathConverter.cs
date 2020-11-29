using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.Helpers
{
    class ImagePathToAbsolutePathConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pathRelative = value as string;

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var pathAbsolute = $"{folder}\\Assets\\Images\\Systems\\{pathRelative}";
            return pathAbsolute;

            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
