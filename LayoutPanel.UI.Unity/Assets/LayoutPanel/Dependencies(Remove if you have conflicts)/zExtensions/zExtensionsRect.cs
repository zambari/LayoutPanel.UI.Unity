

using UnityEngine;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
#endif

//v0.2 rect row split
//v0.3 setpivot now accepts vector2

public static class zExtensionsRect
{

    public static Rect SplitRow(this Rect row, float start, float end, float margin = 2, float lineHeight = 14)
    {
        float x = row.x + row.width * start;
        float w = row.width * (end - start);
        if (start > 0) x += margin;
        if (end < 1) w -= margin;
        return new Rect(x, row.y, w, lineHeight);
    }
    public static Rect MarginLeft(this Rect rect, float margin)
    {
        return new Rect(rect.x + margin, rect.y, rect.width - margin, rect.height);
    }
    public static Rect AdvanceLine(this Rect rect, float advance = 16)
    {
        return new Rect(rect.x, rect.y - advance, rect.width, rect.height);
    }
    public static Rect MarginRight(this Rect rect, float margin)
    {
        return new Rect(rect.x, rect.y, rect.width - margin, rect.height);
    }
    public static void RemoveChildren(this Transform transform, int childIndex = 0)
    {
        int k = 0;
        for (int i = transform.childCount - 1; i >= childIndex; i--)
        {

            GameObject go = transform.GetChild(i).gameObject;
#if UNITY_EDITOR
            MonoBehaviour.DestroyImmediate(go);
#else
            MonoBehaviour.Destroy(go);
#endif
            k++;
        }
        //        Debug.Log("destroyed " + k + " children of " + transform.name, transform.gameObject);
    }
    public static void RemoveChildren(this GameObject g, int childIndex = 0)
    {
        RemoveChildren(g.transform, 0);
    }
    public static void SetParentAndFill(this RectTransform rect, RectTransform parentRect)
    {
        rect.SetParent(parentRect);
        FillParent(rect);

    }
    public static void FillParent(this RectTransform rect)
    {
        RectTransform parentRect = rect.transform.parent.GetComponent<RectTransform>();
        if (parentRect == null)
        {
            Debug.Log("no parent here", rect);

        }
        else
        {
            rect.localScale = Vector3.one;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.localPosition = Vector3.zero;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

    }
    public static RectTransform AddChild(this RectTransform parentRect)
    {
        GameObject go = new GameObject();
        RectTransform rect = go.AddComponent<RectTransform>();


        go.transform.SetParent(parentRect);

        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.sizeDelta = new Vector2(10, 10);
        rect.offsetMin = new Vector2(5, 5);
        rect.offsetMax = new Vector2(-5, -5);
        rect.localPosition = Vector2.zero;
        rect.localScale = Vector3.one;
        //Debug.Log(" added child to ",parentRect.gameObject);
        //	Debug.Log("new object is",rect.gameObject);

        return rect;
    }
    public static void AddLayoutElementFlexible(this GameObject g, bool flexible = true)
    {
        LayoutElement le = g.GetComponent<LayoutElement>();
        if (le == null) le = g.AddComponent<LayoutElement>();
        le.flexibleHeight = 1; le.flexibleWidth = 1;
#if UNITY_EDITOR
        UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(le, false);
#endif
    }

    public static RectTransform AddChildRectTransform(this GameObject parent)
    {
        RectTransform parentRect = parent.GetComponent<RectTransform>();
        return parentRect.AddChild();
    }
    public static void LayoutParameters(this VerticalLayoutGroup vl)
    {
        vl.spacing = 2;
        vl.padding = new RectOffset(3, 3, 3, 3);
        vl.childControlWidth = true;
        vl.childControlHeight = true;
        vl.childForceExpandHeight = false;
        vl.childForceExpandWidth = false;
    }



    public static void SetRelativeSizeX(this RectTransform rect, RectTransform parentRect, float v)
    {
        float sizeX = parentRect.rect.width;
        if (v.CheckFloat())
            rect.sizeDelta = new Vector2(sizeX * v, rect.sizeDelta.y);
        else Debug.Log("source is" + rect.name + " parent " + parentRect.name, rect.gameObject);
    }
    public static void SetRelativeSizeY(this RectTransform rect, RectTransform parentRect, float v)
    {
        float sizeY = parentRect.rect.height;
        if (v.CheckFloat())
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, sizeY * v);
        else Debug.Log("source is" + rect.name + " parent " + parentRect.name, rect.gameObject);
    }
    public static void SetRelativeLocalY(this RectTransform rect, RectTransform parentRect, float v)
    {
        float sizeY = parentRect.rect.height;
        if (v.CheckFloat())
            rect.localPosition = new Vector2(rect.localPosition.x, sizeY * v);
        else Debug.Log("source is" + rect.name + " parent " + parentRect.name, rect.gameObject);
    }
    public static void SetSizeXY(this RectTransform rect, float x, float y)
    {

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);

    }
    public static void SetSizeX(this RectTransform rect, float v)
    {

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, v);

    }
    public static void SetSizeY(this RectTransform rect, float v)
    {

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v);

    }
    public static float GetWidth(this RectTransform rect)
    {

        return rect.rect.width;

    }

    public static float GetHeight(this RectTransform rect)
    {

        return rect.rect.height;

    }
    public static void SetLocalX(this RectTransform rect, float v)
    {
        if (float.IsNaN(v)) return;
        if (rect == null) return;
        rect.localPosition = new Vector2(v, rect.localPosition.y);

    }
    public static void SetLocalY(this RectTransform rect, float v)
    {
        rect.localPosition = new Vector2(rect.localPosition.y, v);

    }
    public static void SetRelativeLocalX(this RectTransform rect, RectTransform parentRect, float v)
    {
        float sizeX = parentRect.rect.width;
        if (v.CheckFloat())
            rect.localPosition = new Vector2(sizeX * v, rect.localPosition.y);
        else Debug.Log("source is" + rect.name + " parent " + parentRect.name, rect.gameObject);

    }

    public static void SetRelativeStartX(this RectTransform rect, RectTransform parentRect, float v)
    {

        float sizeX = parentRect.rect.width;
        if (v.CheckFloat())
            rect.offsetMin = new Vector2(sizeX * v, rect.offsetMin.y);
        else Debug.Log("source is" + rect.name + " parent " + parentRect.name, rect.gameObject);
    }
    public static void SetRelativeEndX(this RectTransform rect, RectTransform parentRect, float v)
    {

        float sizeX = parentRect.rect.width;
        if (v.CheckFloat())
            rect.offsetMax = new Vector2(-sizeX * v, rect.offsetMax.y);
        else Debug.Log("source is" + rect.name + " parent " + parentRect.name, rect.gameObject);
    }
    public static void SetRelativeStartX(this RectTransform rect, float v)
    {
        if (rect.transform.parent == null)
        {
            Debug.Log("no parent", rect);
            return;
        }
        RectTransform parentRect = rect.transform.parent.GetComponent<RectTransform>();
        if (parentRect == null)
        {
            Debug.Log("no parent RectTransform Component", rect);
            return;
        }
        rect.SetRelativeStartX(parentRect, v);

    }
    public static void SetRelativeEndX(this RectTransform rect, float v)
    {
        if (rect.transform.parent == null)
        {
            Debug.Log("no parent", rect);
            return;
        }
        RectTransform parentRect = rect.transform.parent.GetComponent<RectTransform>();
        if (parentRect == null)
        {
            Debug.Log("no parent RectTransform Component", rect);
            return;
        }
        rect.SetRelativeEndX(parentRect, v);
    }
    public static void SetRelativeEndY(this RectTransform rect, float v)
    {
        if (rect.transform.parent == null)
        {
            Debug.Log("no parent", rect);
            return;
        }
        RectTransform parentRect = rect.transform.parent.GetComponent<RectTransform>();
        if (parentRect == null)
        {
            Debug.Log("no parent RectTransform Component", rect);
            return;
        }
        float sizeY = parentRect.rect.height;
        rect.offsetMin = new Vector2(rect.offsetMin.x, sizeY * v);
    }
    public static void SetRelativeStartY(this RectTransform rect, float v)
    {
        if (rect.transform.parent == null)
        {
            Debug.Log("no parent", rect);
            return;
        }
        RectTransform parentRect = rect.transform.parent.GetComponent<RectTransform>();
        if (parentRect == null)
        {
            Debug.Log("no parent RectTransform Component", rect);
            return;
        }
        float sizeY = parentRect.rect.height;
        rect.offsetMax = new Vector2(rect.offsetMax.x, -sizeY * v);

    }
    public static void SetAnchorLeft(this RectTransform rect, float v)
    {
        rect.anchorMin = new Vector2(v, rect.anchorMin.y);
    }
    public static void SetAnchorRight(this RectTransform rect, float v)
    {
        rect.anchorMax = new Vector2(1 - v, rect.anchorMax.y);
    }
    public static void SetAnchorTop(this RectTransform rect, float v)
    {
        rect.anchorMax = new Vector2(rect.anchorMax.x, v);
    }
    public static void SetAnchorBottom(this RectTransform rect, float v)
    {
        rect.anchorMin = new Vector2(rect.anchorMin.x, v);
    }
    public static void SetAnchorsY(this RectTransform rect, float min, float max)
    {
        rect.anchorMin = new Vector2(min, rect.anchorMin.y);
        rect.anchorMax = new Vector2(max, rect.anchorMax.y);
    }
    public static void SetAnchorsX(this RectTransform rect, float min, float max)
    {
        rect.anchorMin = new Vector2(rect.anchorMin.x, min);
        rect.anchorMax = new Vector2(rect.anchorMax.y, max);
    }
    public static void SetAnchorX(this RectTransform rect, float v)
    {
        rect.anchorMin = new Vector2(v, rect.anchorMin.y);
        rect.anchorMax = new Vector2(v, rect.anchorMax.y);
    }
    public static void SetPivotX(this RectTransform rect, float v)
    {
        float deltaPivot = rect.pivot.x - v;
        Vector2 temp = rect.localPosition;
        rect.pivot = new Vector2(v, rect.pivot.y);
        rect.localPosition = temp - new Vector2(deltaPivot * rect.rect.width * rect.localScale.x, 0);
    }
    public static void SetPivotY(this RectTransform rect, float v)
    {
        float deltaPivot = rect.pivot.y - v;
        Vector2 temp = rect.localPosition;
        rect.pivot = new Vector2(rect.pivot.x, v);
        rect.localPosition = temp - new Vector2(0, deltaPivot * rect.rect.height * rect.localScale.y);
    }
    public static void SetPivot(this RectTransform rect, float x, float y)
    {
        float deltaPivotx = rect.pivot.x - x;
        float deltaPivoty = rect.pivot.y - y;
        Vector2 temp = rect.localPosition;
        rect.pivot = new Vector2(x, y);
        rect.localPosition = temp - new Vector2(deltaPivotx * rect.rect.width * rect.localScale.x, deltaPivoty * rect.rect.height * rect.localScale.y);
    }
    public static void SetPivot(this RectTransform rect, Vector2 pivot)
    {
        float deltaPivotx = rect.pivot.x - pivot.x;
        float deltaPivoty = rect.pivot.y - pivot.y;
        Vector2 temp = rect.localPosition;
        rect.pivot = pivot;
        rect.localPosition = temp - new Vector2(deltaPivotx * rect.rect.width * rect.localScale.x, deltaPivoty * rect.rect.height * rect.localScale.y);
    }

    public static void SetTopLeftAnchor(this RectTransform rect, Vector2 newAnchor)
    {
        Vector2 temp = rect.sizeDelta;
        rect.anchorMin = new Vector2(newAnchor.x, rect.anchorMin.y);
        rect.anchorMax = new Vector2(rect.anchorMax.x, newAnchor.y);
        rect.sizeDelta = temp;

    }
    public static void SetBottomRightAnchor(this RectTransform rect, Vector2 newAnchor)
    {
        rect.anchorMin = new Vector2(rect.anchorMin.x, newAnchor.y);
        rect.anchorMax = new Vector2(newAnchor.x, rect.anchorMax.y);

    }


    /// <summary>
    /// Creates a child recttransorm
    /// </summary>

    public static RectTransform GetChild(this RectTransform thisRect)
    {

        RectTransform rect = thisRect.GetComponent<RectTransform>();
        if (rect == null) rect = thisRect.gameObject.AddComponent<RectTransform>();
        return rect;

    }

}