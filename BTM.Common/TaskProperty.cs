﻿using System;

namespace BTM.Common
{
  public class TaskProperty<T> : ITaskProperty
  {
    private Type _dataType;
    
    public TaskProperty(string name, T data)
    {
      _dataType = typeof(T);
      Value = data;
    }

    public T Value { get; set; }

    public string Name { get; }

    object ITaskProperty.Value { get; set; }

    public override string ToString()
    {
      return $"{Name} : {_dataType} = {Value}";
    }
  }
}
