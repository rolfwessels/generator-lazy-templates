namespace MainSolutionTemplate.Dal.Models.Interfaces
{
	public interface IBaseDalModelWithId : IBaseDalModel
	{
        string Id { get; set; }
	}
}