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

    }
}
