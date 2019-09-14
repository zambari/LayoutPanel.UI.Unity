

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

/// oeverrides zRectExtensions

public static class zExtensionsUI
{
public static void SetText(this Text text,float s)
    {
        if (text!=null) text.SetText(s.ToShortString());

    }
    public static void SetText(this Text text,int s)
    {
        if (text!=null) text.SetText(s.ToString());

    }

    public static void SetText(this Text text,string s)
    {
        if (text!=null) text.text=s;
    }
   
    public static LayoutElement[] GetActiveElements(this HorizontalLayoutGroup layout)
    {
        List<LayoutElement> elements = new List<LayoutElement>();
        if (layout == null) return elements.ToArray();
        for (int i = 0; i < layout.transform.childCount; i++)
        {
            GameObject thisChild = layout.transform.GetChild(i).gameObject;
            LayoutElement le = thisChild.GetComponent<LayoutElement>();
            if (le != null)
            {
                if (!le.ignoreLayout) elements.Add(le);
            }
        }
        return elements.ToArray();
    }
    
    // public static Image Image(this RectTransform rect, float transparency = 1)
    // {
    //     Image thisImage = rect.GetComponent<Image>();
    //     if (thisImage == null)
    //     {
    //         thisImage = rect.gameObject.AddComponent<Image>();
    //         thisImage.color = thisImage.color.Random();
    //     }
    //     return thisImage;
    // }
    // public static RectTransform rect(this GameObject go)
    // {
        
    //     RectTransform r = go.GetComponent<RectTransform>();
    //     if (r == null) r = go.AddComponent<RectTransform>();
    //     return r;
    // }

    
    public static Texture2D Create(this Texture2D t, Color fillColor, int sixeX = 1, int sizeY = 1) //, bool apply=true
    {
        Texture2D texture = new Texture2D(sixeX, sizeY);
        Color32[] black = new Color32[texture.width * texture.height];
        for (int i = 0; i < black.Length; i++)
            black[i] = fillColor;

        texture.SetPixels32(black);
        texture.Apply();
        return texture;

    }
    public static void Multiply(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Color32[] colors = texture.GetPixels32();
        for (int i = 0; i < colors.Length; i++)
            colors[i] = colors[i] * fillColor;

        texture.SetPixels32(colors);
        texture.Apply();

    }

    public static void Add(this Texture2D texture, Color fillColor) //, bool apply=true
    {
        Color32[] colors = texture.GetPixels32();
        for (int i = 0; i < colors.Length; i++)
            colors[i] = colors[i] + fillColor;
        texture.SetPixels32(colors);
        texture.Apply();


    }

}