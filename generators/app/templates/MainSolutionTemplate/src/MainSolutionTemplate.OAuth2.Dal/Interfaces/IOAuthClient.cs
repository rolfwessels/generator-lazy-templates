namespace MainSolutionTemplate.OAuth2.Dal.Interfaces
{
	public interface IOAuthClient
	{
		string Secret { get; set; }
		bool Active { get; set; }
		string AllowedOrigin { get; set; }

		/// <summary>
		///     Gets or sets the refresh token life time in minutes.
		/// </summary>
		/// <value>
		///     The refresh token life time in minutes.
		/// </value>
		double RefreshTokenLifeTime { get; set; }
	}
}