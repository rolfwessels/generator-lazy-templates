using System;

namespace MainSolutionTemplate.Shared.Models
{
  public class ErrorMessage
  {
    
    public ErrorMessage()
    {
        Message = String.Empty;
    }

    public ErrorMessage(string message)
    {
        Message = message;
    }

    public string Message { get; set; }

    public string AdditionalDetail { get; set; }
  }
}