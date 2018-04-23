using System;
using System.Collections.Generic;
using System.Linq;
using BTM.Common;
using System.ComponentModel.Composition;

namespace BTM.CommandLine
{
  [Export(typeof(ITask))]
  [ExportMetadata("Name", "CommandLineTask")]
  [ExportMetadata("Version", "1.0.0.1")]
  public class CommandLineTask : ITask
  {
    public ITask Previous { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ITask Next { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IEnumerable<KeyValuePair<string, string>> Parameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IEnumerable<string> GetParameterList()
    {
      return Parameters.Select(kvp => kvp.Key);
    }

    public void SetParameter(string name, string value)
    {
      throw new NotImplementedException();
    }
  }
}
