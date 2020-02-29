//z2k17

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;
// v.02 pan cursor
public class zResourceLoader : MonoBehaviour
{

    public static Texture2D _horizontalCursor;
    [SerializeField]
    public static Texture2D _vertialCursor;
    [SerializeField]
    public static Texture2D _moveCursor;
    [SerializeField]
    public static Texture2D _upLeftResizeCursor;
    [SerializeField]
    public static Texture2D _upRightResizeCursor;
    [SerializeField]
    public static Texture2D _panCursor;
    static bool loaded;
    static Sprite _lineH;

    static Sprite _lineV;
    public static Sprite lineH
    {
        get
        {
            if (!loaded) loadResources();
            return _lineH;
        }
    }

    public static Sprite lineV
    {
        get
        {
            if (!loaded) loadResources();
            return _lineV;
        }
    }
    public static Texture2D horizontalCursor
    {
        get
        {
            if (!loaded) loadResources();
            return _horizontalCursor;
        }
    }

    public static Texture2D vertialCursor
    {
        get
        {
            if (!loaded) loadResources();
            return _vertialCursor;
        }
    }

    public static Texture2D moveCursor
    {
        get
        {
            if (!loaded) loadResources();
            return _moveCursor;
        }
    }

    public static Texture2D panCursor
    {
        get
        {
            if (!loaded) loadResources();
            return _panCursor;
        }
    }
    [SerializeField]
    public static Texture2D upLeftResizeCursor
    {
        get
        {
            if (!loaded) loadResources();
            return _upLeftResizeCursor;
        }
    }
    [SerializeField]
    public static Texture2D upRightResizeCursor
    {
        get
        {
            if (!loaded) loadResources();
            return _upRightResizeCursor;
        }
    }

    public static void loadResources()
    {
        loaded = true;
        if (_horizontalCursor == null) _horizontalCursor = Resources.Load("ResizeHorizontal") as Texture2D;
        if (_vertialCursor == null) _vertialCursor = Resources.Load("ResizeVertical") as Texture2D;
        if (_panCursor == null) _panCursor = Resources.Load("PanView") as Texture2D;
        if (_horizontalCursor == null || _vertialCursor == null)
        { }
        else
        {
            if (_moveCursor == null) _moveCursor = Resources.Load("PanView") as Texture2D;
            if (_upLeftResizeCursor == null) _upLeftResizeCursor = Resources.Load("ResizeUpLeft") as Texture2D; ;
            if (_upRightResizeCursor == null) _upRightResizeCursor = Resources.Load("ResizeUpRight") as Texture2D;
        }
        if (_lineH == null) _lineH = Resources.Load<Sprite>("lineH") as Sprite;
        if (_lineV == null) _lineV = Resources.Load<Sprite>("lineV") as Sprite;

    }
}
