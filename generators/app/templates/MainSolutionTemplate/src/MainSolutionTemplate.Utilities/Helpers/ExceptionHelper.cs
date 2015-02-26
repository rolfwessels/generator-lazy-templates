using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MainSolutionTemplate.Utilities.Helpers
{
    public static class ExceptionHelper
    {
        public static Exception ToSimpleException(this AggregateException exception)
        {
            var errorBuilder = new StringBuilder();
            if (exception.InnerExceptions != null && exception.InnerExceptions.Count > 0)
            {
                foreach (var innerEx in exception.InnerExceptions)
                {
                    errorBuilder.AppendLine(innerEx.Message);
                }
            }
            else
            {
                errorBuilder.AppendLine(exception.Message);
            }
            return new Exception(errorBuilder.ToString(), exception);
        }


        public static Exception ToSimpleException(this ReflectionTypeLoadException ex)
        {
          var sb = new StringBuilder();
          foreach (Exception exSub in ex.LoaderExceptions)
          {
            sb.AppendLine(exSub.Message);
            if (exSub is FileNotFoundException)
            {
              var exFileNotFound = exSub as FileNotFoundException;
              if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
              {
                sb.AppendLine("Fusion Log:");
                sb.AppendLine(exFileNotFound.FusionLog);
              }
            }
            sb.AppendLine();
          }
          return new Exception(sb.ToString(), ex);
        }


      public static string ToSingleExceptionString(this Exception exception)
      {
        var aggregateException = exception as AggregateException;
        Exception simpleException = aggregateException != null ? aggregateException.ToSimpleException() : exception;
        return simpleException.Message + Environment.NewLine + simpleException.StackTrace;
      }

      public static Exception ToFirstExceptionOfException(this Exception exception)
      {
        var aggregateException = exception as AggregateException;
        if (aggregateException != null) return aggregateException.InnerExceptions.First();
        return exception;
      }
    }
}