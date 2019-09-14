

using UnityEngine;
using System;
using System.Text;

/// 
/// zExtensionPrimitives-  Extensions to string, float etc
/// 

public static class zExtensionPrimitives
{
    public static bool IsNullOrEmpty(this Array source)
    {
        return (source == null || source.Length == 0);
    }
    public static bool IsNullOrSmallerThan(this Array source, int len)
    {
        return (source == null || source.Length < len); // <=?
    }
    public static bool IsNullOrEmpty(this string source)
    {
        return string.IsNullOrEmpty(source);
    }

    public static string ToShortString(this float f)
    {
        return (f.ToString("F"));
        //return (Mathf.Round(f * 100) / 100).ToString();
    }

    [Obsolete("Use ToShortString instead")]
    public static string ToStringShort(this float f)
    {
        return (Mathf.Round(f * 100) / 100).ToString();

    }

    public static string MakeGreen(this string s)
    {
        return "<color=green>" + s + "</color>";
    }
    
    public static string MakeColor(this string s, float r, float g, float b, float a =1 )
    {
        Color c = new Color (r,g,b,a);
        return s.MakeColor(c);
    }

    public static string MakeBlue(this string s)
    {
        return "<color=#1010ff>" + s + "</color>";
    }
    public static string MakeRed(this string s)
    {
        return "<color=red>" + s + "</color>";
    }

    public static string Larger(this string s)
    {
        return "<size=16>" + s + "</size>";
    }

    public static string Large(this string s)
    {
        return "<size=14>" + s + "</size>";
    }
    public static string Small(this string s)
    {
        return "<size=8>" + s + "</size>";
    }
    public static string MakeWhite(this string s, float brightness = 0.9f)
    {
        if (brightness < 0) brightness = 0;
        if (brightness > 1) brightness = 1;
        string c = ((int)(brightness * 255)).ToString("x");
        return "<color=#" + c + c + c + ">" + s + "</color>";
    }

    public static string MakeColor(this string s, Color c)
    {
        return "<color=" + ColorUtility.ToHtmlStringRGB(c) + ">" + s + "</color>";
    }



    public static float RandomizeClamped(this float f, float howMuch)
    {
        float n = f.Randomize(howMuch);
        if (n < 0) n = 0;
        if (n > 1) n = 1;
        return n;

    }
    public static float Randomize(this float f, float howMuch)
    {
        float n = f + UnityEngine.Random.value * howMuch - howMuch / 2;
       
        return n;

    }

       public static bool CheckFloat(this float f)
    {
        if (Single.IsNaN(f))
        {
            Debug.Log("invalid float (NAN), dividing by zero? !");
            return false;
        }
        return true;
    }





    public static string RandomString(int length)
    {
        const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
        var builder = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var c = pool[UnityEngine.Random.Range(0, pool.Length - 1)];
            builder.Append(c);
        }

        return builder.ToString();
    }

        public static bool ToBool(this int b)
        {
            return (b == 1);
        }
        public static int ToInt(this bool b)
        {
            return (b ? 1 : 0);
        }

    public static string PadString(this string s, int len)
    {
        if (string.IsNullOrEmpty(s)) s = "";

        for (int i = s.Length; i < len; i++) s += ' ';
        return s;
    }
}