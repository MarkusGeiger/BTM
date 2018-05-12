using System;
using System.Collections.Generic;
using System.Linq;
using BTM.Common;
using BTM.Configuration;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace BTM.Extensibility
{
  [JsonConverter(typeof(TaskJsonConverter))]
  public class DefaultTask : BindableBase, ITask
  {
    public ITask Previous { get; set; }
    public ITask Next { get; set; }
    public IEnumerable<ITaskProperty> Parameters { get; set; }
    public IEnumerable<object> InputValues { get; set; }
    public IEnumerable<object> OutputValues { get; set; }

    public DefaultTask()
    {
      _TaskList = ExtensionManager.Instance.AvailableParts.Select(lazy => lazy.Value);
    }

    private IEnumerable<ITask> _TaskList;
    private ITask _selectedTask;
    private bool _isFirst;

    public string Name => GetType().Name;

    public Version Version => GetType().Assembly.GetName().Version;

    public IEnumerable<ITask> AvailableTaskList => _TaskList;

    public ITask SelectedTask
    {
      get => _selectedTask;
      set => SetProperty(ref _selectedTask, value);
    }
    public bool IsFirst { get => _isFirst; set => SetProperty(ref _isFirst, value); }

    public ITask GetInstance()
    {
      return new DefaultTask();
    }

    public IEnumerable<string> GetParameterList()
    {
      return Enumerable.Empty<string>();
    }

    public void SetParameter(string name, string value)
    {
      // Do nothing here...
    }

    public void Run()
    {
      // Do nothing here...
    }
  }
}
