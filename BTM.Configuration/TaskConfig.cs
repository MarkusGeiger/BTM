using BTM.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;

namespace BTM.Configuration
{
  public class TaskConfig : BindableBase
  {
    private const string FILEPATH = "TaskConfig.json";
    private ObservableCollection<ITask> _taskCollection;

    public ObservableCollection<ITask> TaskCollection
    {
      get => _taskCollection;
      set => SetProperty(ref _taskCollection, value);
    }

    private TaskConfig()
    {
      if (File.Exists(FILEPATH))
      {
        try
        {
          TaskCollection = JsonConvert.DeserializeObject<ObservableCollection<ITask>>(File.ReadAllText(FILEPATH));
        }
        catch (JsonSerializationException jsonSerializationException)
        {
          throw new ConfigurationException($"Error reading AddOn config file {FILEPATH}", jsonSerializationException);
        }
        finally
        {
          if (TaskCollection == null)
          {
            File.Delete(FILEPATH);
            TaskCollection = new ObservableCollection<ITask>();
            TaskCollection.CollectionChanged += OnTaskCollectionChanged;
          }
        }
      }
      else
      {
        TaskCollection = new ObservableCollection<ITask>();
      }

      TaskCollection.CollectionChanged += OnTaskCollectionChanged;
    }

    private void OnTaskCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if(sender is ObservableCollection<ITask> taskCollection)
      {
        File.WriteAllText(FILEPATH, JsonConvert.SerializeObject(taskCollection));
      }
    }

    private static TaskConfig _instance;

    public static TaskConfig Instance
    {
      get => _instance ?? (_instance = new TaskConfig());
    }
  }
  public class TaskJsonConverter : JsonConverter
  {
    public override bool CanWrite => false;
    public override bool CanRead => true;
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(ITask);
    }
    public override void WriteJson(JsonWriter writer,
        object value, JsonSerializer serializer)
    {
      throw new InvalidOperationException("Use default serialization.");
    }

    public override object ReadJson(JsonReader reader,
        Type objectType, object existingValue,
        JsonSerializer serializer)
    {
      var jsonObject = JObject.Load(reader);
      var profession = default(ITask);
      //switch (jsonObject["JobTitle"].Values())
      //{
      //  case "Software Developer":
      //    profession = new Programming();
      //    break;
      //  case "Copywriter":
      //    profession = new Writing();
      //    break;
      //}
      serializer.Populate(jsonObject.CreateReader(), profession);
      return profession;
    }
  }
}
