using BTM.Common;
using BTM.Configuration;
using BTM.Extensibility;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

      TaskItems = new ObservableCollection<ITask>();
    }

    public ICommand ComposeCommand { get => _composeCommand ?? (_composeCommand = new DelegateCommand(OnCompose)); }

    public ICommand RunCommand { get => _runCommand ?? (_runCommand = new DelegateCommand(OnRun)); }

    private void OnRun()
    {
      throw new NotImplementedException();
    }

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
            ExtensionList = ExtensionManager.Instance.AvailableParts.Select(lazy => lazy.Metadata);
            TaskItems = TaskConfig.Instance.TaskCollection;
            // Close...
            controller.CloseAsync();
          });
        });

    }

    public ICommand SaveCommand
    {
      get => _saveCommand ?? (_saveCommand = new DelegateCommand(OnSave));
    }

    private void OnSave()
    {
      TaskConfig.Instance.TaskCollection = TaskItems;
    }

    internal void Initialize()
    {
      OnCompose();
    }

    private IEnumerable<ITaskMetaData> _extensionList;
    private ObservableCollection<ITask> _taskItems;
    private DelegateCommand _composeCommand;
    private IDialogCoordinator dialogCoordinator;
    private DelegateCommand _runCommand;
    private DelegateCommand _saveCommand;

    public IEnumerable<ITaskMetaData> ExtensionList
    {
      get => _extensionList;
      set => SetProperty(ref _extensionList, value);
    }
    public ObservableCollection<ITask> TaskItems
    {
      get => _taskItems;
      set => SetProperty(ref _taskItems, value);
    }
  }
}
