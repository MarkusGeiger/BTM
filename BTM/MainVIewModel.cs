using BTM.Common;
using BTM.Extensibility;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BTM
{
  internal sealed class MainViewModel : BindableBase
  {
    public MainViewModel(MahApps.Metro.Controls.Dialogs.IDialogCoordinator instance)
    {
      dialogCoordinator = instance;
    }

    public ICommand ComposeCommand { get => _composeCommand ?? (_composeCommand = new DelegateCommand(OnCompose)); }

    private async void OnCompose()
    {
      // Show...
      ProgressDialogController controller = await dialogCoordinator.ShowProgressAsync(this, "Loading Plugins", "Please wait until application is created.");
      controller.SetIndeterminate();

      await Task.Factory.StartNew(() =>
      {
        ExtensionManager.Instance.ComposeParts();
      }).ContinueWith(
        task =>
        {
          Application.Current.Dispatcher.Invoke(() =>
          {
            ExtensionList = ExtensionManager.Instance.AvailableParts;
            // Close...
            controller.CloseAsync();
          });
        });

    }

    internal void Initialize()
    {
      OnCompose();
    }

    private IEnumerable<ITaskData> _extensionList;
    private DelegateCommand _composeCommand;
    private IDialogCoordinator dialogCoordinator;

    public IEnumerable<ITaskData> ExtensionList
    {
      get => _extensionList;
      set => SetProperty(ref _extensionList, value);
    }
  }
}
