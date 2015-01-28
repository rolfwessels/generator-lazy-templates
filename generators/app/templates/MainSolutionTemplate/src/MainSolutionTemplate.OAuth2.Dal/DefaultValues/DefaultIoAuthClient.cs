using MainSolutionTemplate.OAuth2.Dal.Interfaces;

namespace MainSolutionTemplate.OAuth2.Dal.DefaultValues
{
	public class DefaultIoAuthClient : IOAuthClient
	{
		#region Implementation of IOAuthClient

		public string Secret { get; set; }
		public bool Active { get; set; }
		public string AllowedOrigin { get; set; }
		public double RefreshTokenLifeTime { get; set; }

		#endregion
	}
}