namespace MainSolutionTemplate.Dal.Persistance
{
  public interface IUnitOfWork
  {
    void Commit();
    void Rollback();
  }
}
