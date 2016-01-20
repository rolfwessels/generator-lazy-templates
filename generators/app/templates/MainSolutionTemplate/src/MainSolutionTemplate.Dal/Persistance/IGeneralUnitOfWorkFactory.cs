namespace MainSolutionTemplate.Dal.Persistance
{
    public interface IGeneralUnitOfWorkFactory
    {
        IGeneralUnitOfWork GetConnection();
    }
}