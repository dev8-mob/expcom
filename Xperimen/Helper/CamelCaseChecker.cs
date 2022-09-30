
namespace Xperimen.Helper
{
    public class CamelCaseChecker
    {
        public CamelCaseChecker() { }

        public string CapitalizeWord(string word)
        {
            var finalword = string.Empty;
            if (!string.IsNullOrEmpty(word))
            {
                var split = word.Trim().Split(' ');
                if (split.Length > 1)
                {
                    foreach (var data in split)
                        finalword += char.ToUpper(data[0]) + data.Substring(1) + " ";
                }
                else finalword += char.ToUpper(word[0]) + word.Substring(1);
            }
            return finalword.Trim();
        }
    }
}
