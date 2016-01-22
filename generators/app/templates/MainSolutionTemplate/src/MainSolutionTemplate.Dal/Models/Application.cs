namespace MainSolutionTemplate.Dal.Models
{
	public class Application : BaseDalModelWithId 
	{
		public string ClientId { set; get; }
        
		#region Implementation of IOAuthClient

		public string Secret { get; set; }
		public bool Active { get; set; }
		public string AllowedOrigin { get; set; }
		public double RefreshTokenLifeTime { get; set; }

		#endregion
	}
}