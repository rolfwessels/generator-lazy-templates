namespace MainSolutionTemplate.Dal.Persistance
{
  public interface IUnitOfWork
  {
    void MarkDirty(object entity);
    void MarkNew(object entity);
    void MarkDeleted(object entity);
    void Commit();
    void Rollback();
  }
}
