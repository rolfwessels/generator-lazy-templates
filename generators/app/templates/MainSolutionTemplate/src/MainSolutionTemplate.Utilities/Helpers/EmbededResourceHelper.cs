using System.IO;
using System.Reflection;

namespace MainSolutionTemplate.Utilities.Helpers
{
  public static class EmbededResourceHelper
  {
    public static string ReadResource(string resourceName, Assembly getExecutingAssembly)
    {
      var assembly = getExecutingAssembly;
      using (Stream stream = assembly.GetManifestResourceStream(resourceName))
      {
        return stream.ReadToString();
      }
    }
  }
}