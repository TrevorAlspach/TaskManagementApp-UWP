using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4930_TaskManagementApp_UWP.Services
{
    //Allows dates to be displayed in a more readable format
    public class DateStringFormatter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType,
   object parameter, string language)
        {
            string formatString = parameter as string;
            if (!string.IsNullOrEmpty(formatString))
            {
                return string.Format(formatString, value);
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType,
    object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleantoPriorityConverter : Windows.UI.Xaml.Data.IValueConverter 
    {
        public object Convert(object value, Type targetType,
   object parameter, string language)
        {
            if ((bool)value)
            {
                return "Yes";
            }
            return "No";
        }

        public object ConvertBack(object value, Type targetType,
    object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    //Displays true as Complete, false as Incomplete
    public class BooleantoStatusConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType,
   object parameter, string language)
        {
            if ((bool)value)
            {
                return "Complete";
            }
            return "Incomplete";
        }

        public object ConvertBack(object value, Type targetType,
    object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowPriorityConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType,
   object parameter, string language)
        {
            if ((bool)value)
            {
                return "PRIORITY";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType,
    object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
