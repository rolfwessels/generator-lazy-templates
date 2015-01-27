namespace MainSolutionTemplate.Dal.Persistance
{
  public interface IUnitOfWork
  {
    void Rollback();
  }
}
