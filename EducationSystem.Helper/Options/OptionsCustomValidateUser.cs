﻿
namespace EducationSystem.Helper.Options
{
    public class OptionsCustomValidateUser
    {
        public bool NickNameDontUseSymbol { get; set; }
        public bool NickNameUseFormat { get; set; }
        public List<string> BlockEmailRegions { get; set; }
    }

}