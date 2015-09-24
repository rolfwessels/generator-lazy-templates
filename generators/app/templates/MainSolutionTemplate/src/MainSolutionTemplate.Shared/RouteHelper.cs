namespace MainSolutionTemplate.Shared
{
	public static class RouteHelper
	{
		public const string ApiPrefix = "api/";

        public const string WithId = "{id}";
        public const string WithDetail = "detail";

        public const string UserController = ApiPrefix+"user";
		public const string UserControllerRegister = "register";
		public const string UserControllerLogin = "login";
		public const string UserControllerForgotPassword = "forgotPassword/{email}";

        public const string ProjectController = ApiPrefix + "project";


        
	}
}