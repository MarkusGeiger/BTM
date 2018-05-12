using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace BTM.Converter
{
  public class VisibilityInvertConverter : MarkupExtension, IValueConverter
  {

    public Visibility DefaultVisibility { get; set; }
    public Visibility InvertedVisibility { get; set; }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if(value is Visibility visibilityValue)
      {
        if(visibilityValue == DefaultVisibility)
        {
          return InvertedVisibility;
        }
      }
      return DefaultVisibility;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Convert(value, targetType, parameter, culture);
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return this;
    }
  }
}
