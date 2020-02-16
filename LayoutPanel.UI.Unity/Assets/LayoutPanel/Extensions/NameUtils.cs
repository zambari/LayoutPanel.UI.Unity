using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z
{
    public static class NameUtils
    {
        public static readonly char separator = ' ';// ' '; //set to something invisle

        public static bool HasTag(this string s)
        {
            if (s == null) return false;
            return s.IndexOf(separator) > 0;
        }
        public static string RemoveTag(this string s)
        {
            if (s == null) return s;
            int pos = s.IndexOf(separator);
            if (pos > 0) return s.Substring(0, pos);
            return s;
        }
        public static string GetTag(this string s)
        {
            int pos = s.IndexOf(separator);
            if (pos > 0) return s.Substring(pos + 1);
            return null;
        }

        public static string AddTag(this string s, string tagString)
        {
            if (s == null) return separator + tagString;
            int pos = s.IndexOf(separator);
            if (pos < 0)
                s += separator;
            s += tagString;

            return s;
        }
        public static string SetTag(this string s, string tagString)
        {
            if (s == null) return s.AddTag(tagString);
            int pos = s.IndexOf(separator);
            if (pos < 0)
                s += separator;
            else
                s = s.Substring(0, pos + 1);
            s += tagString;
            return s;
        }
    }
}