
namespace EducationSystem.Helper.Options
{
    public class OptionsJwtValidate
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public string ExpiresAccess { get; set; }
        public string ExpiresRefresh { get; set; }
    }
}