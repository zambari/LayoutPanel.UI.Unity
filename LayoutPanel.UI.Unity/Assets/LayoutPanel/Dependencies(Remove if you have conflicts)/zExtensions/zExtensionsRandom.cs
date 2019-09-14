

using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


public static class zExtensionsRandom
{
    public static Color Randomize(this Color c, float howMuchHue = 0.15f, float howMuchSat = 0.3f, float howMuchL = 0.2f)
    {
        float H, S, L;
        Color.RGBToHSV(c, out H, out S, out L);
        H = H.RandomizeClamped(howMuchHue);
        S = S.RandomizeClamped(howMuchSat);
        L = L.RandomizeClamped(howMuchL);
        Color newCol = Color.HSVToRGB(H, S, L);
        newCol.a = c.a;
        return newCol;
    }

    public static Vector3 RandomizeXY(this Vector3 r, float range = 300, bool allowNegatives = true)

    {

        float x = r.x + UnityEngine.Random.value * range;
        float y = r.y + UnityEngine.Random.value * range;
        if (!allowNegatives)
        {
            if (x < 0) x = x * -1;
            if (y < 0) y = y * -1;
        }
        return new Vector3(x, y, r.z);

    }
    public static Vector2 Randomize(this Vector2 r, float range = 300, bool allowNegatives = true)
    {
        float x = r.x + UnityEngine.Random.value * range;
        float y = r.y + UnityEngine.Random.value * range;
        if (!allowNegatives)
        {
            if (x < 0) x = x * -1;
            if (y < 0) y = y * -1;
        }
        return new Vector2(x, y);
    }

    public static Color Random(this Color c)
    {
        /*  if (c==null) { c =baseColor; }
          float r=c.r;
          float g=c.g;
          float b=c.b;
          r=r/2+r*UnityEngine.Random.value;
          g=g/2+g*UnityEngine.Random.value;
          g=g/2+b*UnityEngine.Random.value;

          c=new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0.3f);
          return c;*/
        //        Color n=UnityEngine.Random.ColorHSV(0.4f,0.8f,0.3f,0.6f);
        c.a = UnityEngine.Random.value * 0.4f + 0.2f;
        return UnityEngine.Random.ColorHSV(0.4f, 0.8f, 0.3f, 0.6f);
    }

}