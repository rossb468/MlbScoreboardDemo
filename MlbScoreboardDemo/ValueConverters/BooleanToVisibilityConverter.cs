using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MlbScoreboardDemo.ValueConverters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
		/*
		 * Pass "true" to as the converter parameter to invert visibility
		 * EX: Binding to Game.IsCompleted would display the element if game is complete.
		 *     Passing a parameter "true" would hide the element if game is complete
		 * */
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolValue = value as bool? ?? false;
            var boolParam = System.Convert.ToBoolean(parameter);

            return boolValue ^ boolParam ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
