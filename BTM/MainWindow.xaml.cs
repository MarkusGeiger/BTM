using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Input;

namespace BTM
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : MetroWindow
  {
    public MainWindow()
    {
      InitializeComponent();
      DataContext = new MainViewModel(DialogCoordinator.Instance);
    }

    private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
    {
      ((MainViewModel)DataContext).Initialize();

    }

    protected override void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e)
    {
      e.Handled = true;
    }

  }
}
