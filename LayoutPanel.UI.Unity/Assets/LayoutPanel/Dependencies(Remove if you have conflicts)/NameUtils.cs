using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    // v .02 
    // remove all extension
    public static class NameUtils
    {
        public static readonly char defaultSeparator = (char)8198;// ' '; //set to something invisle
        public static readonly char defaultPreSeparator = (char)2063;
        /// some other space chars =2063, 8198, 8201, 8202 , 8287
        /// 
        /// 
        /// 9312 = ①
        /// 9332 = ⑴
        /// 9352 = ⒈
        /// 9372 = ⒜
        ///  9461 = ⓵
        /// char 9424 = ⓐ


        public enum UnicodeNumberType { circled, smalCircled, tallBrackets, elegant, letters, lettersInCircles }
        public static int GetStartChar(UnicodeNumberType code = UnicodeNumberType.circled)
        {
            switch (code)
            {
                default:
                case UnicodeNumberType.smalCircled: return 9312;
                case UnicodeNumberType.tallBrackets: return 9332;
                case UnicodeNumberType.elegant: return 9352;
                case UnicodeNumberType.circled: return 9461;
                case UnicodeNumberType.letters: return 9372;
                case UnicodeNumberType.lettersInCircles: return 9424;
            }
        }
        public static bool HasTag(this string s, char separator = '$')
        {
            if (s == null) return false;
            return s.IndexOf(CheckSeparator(separator)) >= 0;
        }
        public static string RemoveTag(this string s, char separator = '$')
        {
            if (s == null) return s;
            separator = CheckSeparator(separator);
            int pos = s.IndexOf(separator);
            if (pos >= 0) return s.Substring(0, pos);
            return s;
        }
        public static string GetTag(this string s, char separator = '$')
        {
            int pos = s.IndexOf(CheckSeparator(separator));
            if (pos > 0) return s.Substring(pos + 1);
            return null;
        }

        // public static string AddTag(this string s, string tagString, char separator = '$')
        // {
        //     separator = CheckSeparator(separator);
        //     //   if (s == null) return separator + tagString;
        //     int pos = s.IndexOf(separator);
        //     if (pos < 0)
        //         s += separator;
        //     s += tagString;

        //     return s;
        // }
        public static string SetTag(this string s, string tagString, char separator = '$')
        {
            separator = CheckSeparator(separator);
            // if (s == null) return s.AddTag(tagString, separator);
            int pos = s.IndexOf(separator);
            if (pos < 0)
                s += separator;
            else
                s = s.Substring(0, pos + 1);
            s += tagString;
            return s;
        }

        static char CheckSeparator(char sep)
        {
            if (sep == '$')
                return defaultSeparator;
            else return sep;
        }

        public static bool HasPreTag(this string s, char separator = '$')
        {
            if (s == null) return false;
            separator = CheckPreSeparator(separator);

            return s.IndexOf(separator) >= 0;
        }
        public static string RemoveAllTags(this string s, char separator = '$')
        {
            return s.RemoveTag().RemovePreTag(); ;
        }
        public static string RemovePreTag(this string s, char separator = '$')
        {
            if (s == null) return s;
            int pos = s.IndexOf(CheckPreSeparator(separator));
            if (pos >= 0) return s.Substring(pos + 1);
            return s;
        }
        public static string GetPreTag(this string s, char separator = '$')
        {
            int pos = s.IndexOf(CheckPreSeparator(separator));
            if (pos >= 0) return s.Substring(0, pos);
            return null;
        }

        public static string AddPreTag(this string s, string tagString, char separator = '$')
        {
            separator = CheckPreSeparator(separator);
            int pos = s.IndexOf(separator);
            if (pos < 0) // no tag
                s = tagString + separator + s;
            else
                s = tagString + s;//Substring(pos);

            return s;
        }
        public static string SetPreTag(this string s, string tagString, char separator = '$')
        {
            separator = CheckPreSeparator(separator);
            s = s.RemovePreTag(separator);
            s = tagString + separator + s;
            return s;
        }

        static char CheckPreSeparator(char sep)
        {
            if (sep == '$')
                return defaultPreSeparator;
            else return sep;
        }
        public static void RemoveAllTags(this GameObject game)
        {
            string n = game.name;
            n = n.RemovePreTag();
            n = n.RemoveTag();
            game.name = n;
        }
        public static void SetTag(this GameObject game, string tag)
        {
            game.name = game.name.SetTag(tag);
        }
        public static void SetPreTag(this GameObject game, string tag)
        {
            game.name = game.name.SetPreTag(tag);
        }
        public static string ToFunnyNumber(this int val, UnicodeNumberType type = UnicodeNumberType.tallBrackets)
        {
            if (val < 1 || val > 20) return val.ToString();
            return ((char)(val + GetStartChar(type) - 1)).ToString();
        }
        // static string ToFunnyUnicodeLetters(this int num, int funnyStart)
        // {
        //     string funnyString = num < 0 ? "-" : "";
        //     if (num < 0) num = -num;
        //     string asString = num.ToString();

        //     for (int i = 0; i < asString.Length; i++)
        //     {
        //         char thisChar = asString[i];

        //         int thicChar = thisChar - 48;
        //         funnyString += (char)(thicChar + funnyStart);
        //     }
        //     return funnyString;
        // }
        // public static string ToFunnyUnicodeLettersDouble(this int num)
        // {
        //     return num.ToFunnyUnicodeLetters(9460);

        // }
        // public static string ToFunnyUnicodeLetters(this int num)
        // {
        //     return num.ToFunnyUnicodeLetters(9450);

        // }
    }
}