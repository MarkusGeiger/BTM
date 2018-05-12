using System;
using System.Collections.Generic;
using System.Linq;
using BTM.Common;
using System.ComponentModel.Composition;
using Prism.Mvvm;
using Newtonsoft.Json;
using BTM.Configuration;

namespace BTM.CommandLine
{
  [Export(typeof(ITask))]
  [ExportMetadata("Name", "CommandLineTask")]
  [ExportMetadata("Version", "1.0.0.1")]
  [JsonConverter(typeof(TaskJsonConverter))]
  public class CommandLineTask : TaskBase, ITask
  {
   
    public override string Name => GetType().Name;

    public override Version Version => GetType().Assembly.GetName().Version;

    public override ITask GetInstance()
    {
      return new CommandLineTask();
    }

    protected override void DoWork()
    {
      throw new NotImplementedException();
    }
  }
}
