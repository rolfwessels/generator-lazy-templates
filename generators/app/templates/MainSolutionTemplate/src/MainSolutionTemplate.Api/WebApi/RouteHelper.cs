namespace MainSolutionTemplate.Api.WebApi
{
	public static class RouteHelper
	{
		public const string ApiPrefix = "api/";
		public const string UserController = ApiPrefix+"User/";
		public const string UserControllerId = UserController + "{Id}/";

		public const string UserControllerRegister = UserController + "Register/";
		public const string UserControllerLogin = UserController + "Login/";
	}
}