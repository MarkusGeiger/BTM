using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM.Common
{
  [JsonConverter(typeof(IgnoreJsonConverter))]
  public abstract class TaskBase : BindableBase, ITask
  {
    private ITask _previous;
    private ITask _next;
    private IEnumerable<ITaskProperty> _parameters;
    private bool _isFirst;

    [JsonIgnore]
    public ITask Previous
    {
      get => _previous;
      set => SetProperty(ref _previous, value);
    }

    [JsonIgnore]
    public ITask Next
    {
      get => _next;
      set => SetProperty(ref _next, value);
    }

    public IEnumerable<ITaskProperty> Parameters
    {
      get => _parameters;
      set => SetProperty(ref _parameters, value);
    }

    public IEnumerable<object> InputValues { get; set; }
    public IEnumerable<object> OutputValues { get; set; }

    public abstract string Name { get; }

    public abstract Version Version { get; }
    public bool IsFirst { get => _isFirst; set => SetProperty(ref _isFirst,value); }

    public abstract ITask GetInstance();

    public IEnumerable<string> GetParameterList()
    {
      return Parameters != null ? Parameters.Select(taskProperty => taskProperty.Name) : Enumerable.Empty<string>();
    }

    protected virtual T GetParameter<T>(string name)
    {
      if (Parameters == null || !Parameters.Any(param => param.Name == name)) return default(T);
      
      return Parameters.Cast<TaskProperty<T>>().First(param => param.Name == name).Value;
    }

    public void Run()
    {
      if (Previous != null) InputValues = Previous.OutputValues;
      DoWork();
      if (Next != null) Next.Run();
    }

    protected abstract void DoWork();

    public void SetParameter(string name, string value)
    {
      if (Parameters != null && Parameters.Any(param => param.Name == name))
      {
        Parameters.First(param => param.Name == name).Value = value;
      }
    }
  }
}
