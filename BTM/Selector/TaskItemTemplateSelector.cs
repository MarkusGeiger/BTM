using BTM.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BTM.Selector
{
  public class TaskItemTemplateSelector : DataTemplateSelector
  {
    public DataTemplate DefaultDataTemplate { get; set; }
    public DataTemplate SelectableDataTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      if(item is DefaultTask)
      {
        return DefaultDataTemplate;
      }
      return SelectableDataTemplate;
    }

  }
}
