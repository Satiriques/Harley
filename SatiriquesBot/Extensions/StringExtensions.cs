namespace System
{
    public static class StringExtensions
    {
        public static string ClearCode(this string text)
        {
            return text.Replace("`", "");
        }
    }
}
