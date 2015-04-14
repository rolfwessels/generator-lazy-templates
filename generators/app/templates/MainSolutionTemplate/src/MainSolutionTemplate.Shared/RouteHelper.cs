namespace MainSolutionTemplate.Shared
{
	public static class RouteHelper
	{
		public const string ApiPrefix = "api/";

        public const string WithId = "{id}";
        public const string WithDetail = "Detail";

        public const string UserController = ApiPrefix+"User";
		public const string UserControllerRegister = "Register";
		public const string UserControllerLogin = "Login";
		public const string UserControllerForgotPassword = "ForgotPassword/{email}";

        public const string ProjectController = ApiPrefix + "Project";


        
	}
}