using System;

namespace BTM.Extensibility
{
  public class ExtensibilityException : Exception
  {
    public ExtensibilityException(string message) : base(message)
    {
    }

    public ExtensibilityException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}
