using System;
using System.Collections.Generic;
using System.Text;

namespace SatiriquesBot.Common
{
    public static class Style
    {
        public static string Bold(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            return "**" + text + "**";
        }

        public static string Italics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            return "*" + text + "*";
        }
    }
}
