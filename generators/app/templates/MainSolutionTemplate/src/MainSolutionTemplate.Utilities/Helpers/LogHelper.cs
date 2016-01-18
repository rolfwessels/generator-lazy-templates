using System;
using Newtonsoft.Json;

namespace MainSolutionTemplate.Utilities.Helpers
{
  public static class LogHelper
  {
    public static string Dump(this object val)
    {
      return Dump(val, true);
    }

    public static string Dump(this object val, bool indented)
    {
      return JsonConvert.SerializeObject(val, indented ? Formatting.Indented : Formatting.None);
    }

    public static T Dump<T>(this T val, string description)
    {
        Console.Out.WriteLine(description+":"+val.Dump());
        return val;
    }
  }
}