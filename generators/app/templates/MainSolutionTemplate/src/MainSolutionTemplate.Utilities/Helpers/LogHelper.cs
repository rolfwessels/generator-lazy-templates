using Newtonsoft.Json;

namespace MainSolutionTemplate.Utilities.Helpers
{
  public static class LogHelper
  {
    public static string Dump(this object preProcessorAssemblies)
    {
      return Dump(preProcessorAssemblies, true);
    }

    public static string Dump(this object preProcessorAssemblies, bool indented)
    {
      return JsonConvert.SerializeObject(preProcessorAssemblies, indented ? Formatting.Indented : Formatting.None);
    }
  }
}