namespace MainSolutionTemplate.Api.WebApi.ODataSupport
{
  public interface IQueryToODataWrapper
  {
    long? Count { get;  }
    bool RequiresPagedValue { get; }
    object GetPagedResult();
  }
}