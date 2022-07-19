namespace EducationSystem.Web.Api.Contracts.v1
{
    internal class Routes
    {
        private const string _version = "v1";
        private const string _api = "education-system/api";

        private const string _base = $"{_api}/{_version}";


        public const string ControllerAuth = _base + "/auth";

        public const string Register = "register";
        public const string Login = "login";
        public const string SendConfirmMessage = "send-confirm-message";
        public const string ConfirmEmail = "confirm-email";
        public const string Refresh = "refresh";
        public const string Logout = "logout";

        public const string ControllerAccount = _base + "/account";

        public const string ControllerUser = _base + "/user";

        public const string AddToRole = "add-to-role";
        public const string RemoveToRole = "remove-to-role";

        public const string ControllerSchool = _base + "/school";

        public const string ControllerOrderSchool = _base + "/order-school";

        // base crud contracts
        public const string Create = "create";
        public const string Get = "get";
        public const string GetPage = "get-page";
        public const string Update = "update";
        public const string Delete = "delete";

    }
}
