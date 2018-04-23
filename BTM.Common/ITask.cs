using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM.Common
{
  public interface ITask
  {
    ITask Previous { get; set; }
    ITask Next { get; set; }
    IEnumerable<KeyValuePair<string, string>> Parameters { get; set; }

    void SetParameter(string name, string value);
    IEnumerable<string> GetParameterList();
  }
}
