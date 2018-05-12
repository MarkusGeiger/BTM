using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BTM.Common
{
  [JsonConverter(typeof(IgnoreJsonConverter))]
  public interface ITask : INotifyPropertyChanged
  {
    ITask Previous { get; set; }
    ITask Next { get; set; }
    IEnumerable<ITaskProperty> Parameters { get; set; }

    ITask GetInstance();

    IEnumerable<object> InputValues { get; set; }
    IEnumerable<object> OutputValues { get; set; }

    string Name { get; }

    bool IsFirst { get; set; }

    Version Version { get; }

    void Run();

    void SetParameter(string name, string value);
    IEnumerable<string> GetParameterList();
  }
}
