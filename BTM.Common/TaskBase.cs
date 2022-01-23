using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTM.Common
{
  [JsonConverter(typeof(IgnoreJsonConverter))]
  public abstract class TaskBase : BindableBase, ITask
  {
    protected TaskBase()
    {
      Guid = Guid.NewGuid();
    }

    [JsonIgnore]
    public readonly Guid Guid;

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

    public override bool Equals(object obj)
    {
      var taskBase = obj as TaskBase;
      return taskBase != null &&
             Guid.Equals(taskBase.Guid) &&
             Name == taskBase.Name &&
             EqualityComparer<Version>.Default.Equals(Version, taskBase.Version) &&
             IsFirst == taskBase.IsFirst;
    }

    public override int GetHashCode()
    {
      var hashCode = -523652398;
      hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(Guid);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
      hashCode = hashCode * -1521134295 + EqualityComparer<Version>.Default.GetHashCode(Version);
      hashCode = hashCode * -1521134295 + IsFirst.GetHashCode();
      return hashCode;
    }
  }
}
