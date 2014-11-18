using System;
using System.Text.RegularExpressions;

namespace ZxcWorkLog.Util
{
    static class TextUtil
    {
        public static String GetFirstLine(string text)
        {
            var result = Regex.Split(text, "\r\n|\r|\n");
            return result[0];
        }
    }
}
