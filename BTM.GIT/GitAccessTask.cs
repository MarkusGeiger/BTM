using BTM.Common;
using BTM.Configuration;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace BTM.GIT
{
  [Export(typeof(ITask))]
  [ExportMetadata("Name", "GitAccessTask")]
  [ExportMetadata("Version", "1.0.1.1")]
  [JsonConverter(typeof(TaskJsonConverter))]
  public class GitAccessTask : TaskBase, ITask
  {
    public override string Name => GetType().Name;

    public override Version Version => GetType().Assembly.GetName().Version;

    public override ITask GetInstance()
    {
      return new GitAccessTask();
    }

    protected override void DoWork()
    {
      throw new NotImplementedException();
    }
  }
}
