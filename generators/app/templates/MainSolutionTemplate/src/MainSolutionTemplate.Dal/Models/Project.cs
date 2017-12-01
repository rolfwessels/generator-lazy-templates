namespace MainSolutionTemplate.Dal.Models
{
	public class Project : BaseDalModelWithId
	{
		public string Name { get; set; }

	    public override string ToString()
	    {
	        return $"Project: {Name}";
	    }
	}
}