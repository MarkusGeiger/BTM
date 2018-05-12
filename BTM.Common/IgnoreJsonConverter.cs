using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTM.Common
{
  public class IgnoreJsonConverter : JsonConverter
  {
    public override bool CanRead => true;
    public override bool CanWrite => false;
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(ITask);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new InvalidOperationException("Use default serialization.");
    }
  }
}
