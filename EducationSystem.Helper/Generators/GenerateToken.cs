namespace EducationSystem.Helper.Generators
{
    public class GenerateToken
    {
        private string symbols = "qwertyuiopasdfghjklzxcvbnm1234567890";
        public async Task<string> GenerateAsync()
        {
            return await Task.Run(() =>
            {
                var result = "";
                var rnd = new Random();

                for (int i = 0; i < 48; i++)
                    result += rnd.Next(2) == 0 ? symbols[rnd.Next(0, symbols.Length)].ToString().ToUpper() :
                                                 symbols[rnd.Next(0, symbols.Length)].ToString().ToLower();
                return result;
            });
        }
    }
}
