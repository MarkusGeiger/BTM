using System.Windows;
using System.Windows.Controls;

namespace BTM.Extensions
{
  public static class ItemsControlExtensions
  {
    public static void ScrollIntoView(this ItemsControl control, object item)
    {
      if (!(control.ItemContainerGenerator.ContainerFromItem(item) is FrameworkElement frameworkElement)) return;
      frameworkElement.BringIntoView();
    }

    public static void ScrollIntoView(this ItemsControl itemsControl)
    {
      int count = itemsControl.Items.Count;
      if (count == 0) return;
      object item = itemsControl.Items[count - 1];
      itemsControl.ScrollIntoView(item);
    }
  }
}
