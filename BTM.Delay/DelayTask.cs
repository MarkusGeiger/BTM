using BTM.Common;
using BTM.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading;

namespace BTM.Delay
{
  [Export(typeof(ITask))]
  [ExportMetadata("Name", "DelayTask")]
  [ExportMetadata("Version", "1.0.0.1")]
  [JsonConverter(typeof(TaskJsonConverter))]
  public class DelayTask : TaskBase, ITask
  {
    private const string TIMESPAN = "Timespan";

    public DelayTask()
    {
      InitParameters();
    }

    public override string Name { get => GetType().Name; }
    public override Version Version { get => GetType().Assembly.GetName().Version; }

    public override ITask GetInstance()
    {
      return new DelayTask();
    }

    protected override void DoWork()
    {
      Thread.Sleep(GetParameter<Int32>(TIMESPAN));
    }

    private void InitParameters()
    {
      Parameters = new ObservableCollection<ITaskProperty>
      {
        new TaskProperty<Int32>(TIMESPAN, 0)
      };
    }
  }
}
