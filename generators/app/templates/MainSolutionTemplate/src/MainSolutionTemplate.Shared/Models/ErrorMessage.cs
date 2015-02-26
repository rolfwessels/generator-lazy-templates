using System;

namespace MainSolutionTemplate.Shared.Models
{
  public class ErrorMessage
  {
    private readonly string _message;

    public ErrorMessage()
    {
      _message = String.Empty;
    }

    public ErrorMessage(string message)
    {
      _message = message;
    }

    public string Message
    {
      get { return _message; }
    }

    public dynamic AdditionalDetail { get; set; }
  }
}