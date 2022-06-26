
namespace EducationSystem.Helper.Options
{
    public class OptionsValidatePassword
    {
        public int RequiredLength { get; set; }
        public bool RequireNonAlphabet { get; set; }
        public bool RequireNonNumber { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
    }
}
