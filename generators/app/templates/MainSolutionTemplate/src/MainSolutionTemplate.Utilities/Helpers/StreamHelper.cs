using System.IO;
using System.Text;

namespace MainSolutionTemplate.Utilities.Helpers
{
  public static class StreamHelper
  {
	  public static Stream ToStream(this string stringValue)
    {
      return new MemoryStream(Encoding.UTF8.GetBytes(stringValue));  
    }

    public static byte[] ToBytes(this Stream input)
    {
      var memoryStream = input as MemoryStream;
      if (memoryStream != null) return memoryStream.ToArray();
      using (var ms = new MemoryStream())
      {
        input.Position = 0;
        input.CopyTo(ms);
        return ms.ToArray();
      }
    }

    public static MemoryStream ToMemoryStream(this Stream stringValue)
    {
      var stream = stringValue as MemoryStream;
      if (stream != null)
      {
        stream.Position = 0;
        return stream;
      }

      var memoryStream = new MemoryStream();
      stringValue.CopyTo(memoryStream);
      memoryStream.Flush();
      memoryStream.Position = 0;
      return memoryStream;
    }

    public static string ReadToString(this Stream stream)
    {
      var streamReader = new StreamReader(stream);
      return streamReader.ReadToEnd();
    }

    public static FileInfo SaveTo(this Stream stream, string stringValue)
    {
      using (FileStream fileStream = File.OpenWrite(stringValue))
      {
        stream.CopyTo(fileStream);
      }
      return new FileInfo(stringValue);
    }

  }
}