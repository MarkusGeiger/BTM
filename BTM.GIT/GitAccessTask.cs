using BTM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM.GIT
{
  [Export(typeof(ITask))]
  [ExportMetadata("Name", "GitAccessTask")]
  [ExportMetadata("Version", "1.0.0.1")]
  public class GitAccessTask : ITask
  {
    public ITask Previous { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ITask Next { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IEnumerable<KeyValuePair<string, string>> Parameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IEnumerable<string> GetParameterList()
    {
      throw new NotImplementedException();
    }

    public void SetParameter(string name, string value)
    {
      throw new NotImplementedException();
    }
  }
}
