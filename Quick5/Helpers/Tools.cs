using System.Text.RegularExpressions;

namespace Quick5.Helpers
{
    public static class Tools
    {
        private static Regex only_digits = new Regex(@"[^0-9]");

        public static string DigitOnly(string text)
        {
            try
            {
                return only_digits.Replace(text, "");
            }
            catch
            {
                return "";
            }
        }
    }
}