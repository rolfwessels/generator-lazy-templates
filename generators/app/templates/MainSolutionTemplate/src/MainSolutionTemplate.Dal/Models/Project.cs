namespace MainSolutionTemplate.Dal.Models
{
	public class Project : BaseDalModelWithId
	{
		public string Name { get; set; }

	    public override string ToString()
	    {
	        return string.Format("Project: {0}", Name);
	    }
	}
}