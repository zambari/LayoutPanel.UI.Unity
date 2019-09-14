using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace zUI
{

    // v.0.4 merge with a more progressive branch
    // v.0.5 add child text

    public enum LayoutElementState { noLayoutElement, layoutPresentIgnoring, layoutPresentStretch, layoutStrechHorizontal, lyoutStretchVertical, layoutNotFlexible, layoutDenenerate }



    public static class MoreLayoutExt
    {

        public static LayoutElementState GetLayoutElementState(this LayoutElement layoutElement)
        {
            if (layoutElement == null) return LayoutElementState.noLayoutElement;
            if (layoutElement.ignoreLayout) return LayoutElementState.layoutPresentIgnoring;
            if (layoutElement.flexibleHeight > 0 && layoutElement.flexibleHeight > 0) return LayoutElementState.layoutPresentStretch;

            if ((layoutElement.preferredHeight > 0 || layoutElement.minHeight > 0) &&
            (layoutElement.preferredWidth > 0 || layoutElement.minWidth > 0)) return LayoutElementState.layoutNotFlexible;
            return LayoutElementState.layoutDenenerate;

        }


        public static void SetLayoutElementState(this LayoutElement layoutElement, LayoutElementState state)
        {
            if (layoutElement == null) return;
            switch (state)
            {
                case LayoutElementState.noLayoutElement:
                    //                    layoutElement.DestroySmart();
                    break;
                case LayoutElementState.layoutPresentIgnoring:
                    layoutElement.ignoreLayout = true;
                    break;
                case LayoutElementState.layoutPresentStretch:
                    layoutElement.ignoreLayout = false;
                    layoutElement.flexibleHeight = 1;
                    layoutElement.flexibleWidth = 1;

                    break;

                case LayoutElementState.layoutStrechHorizontal:
                    if (layoutElement.ignoreLayout)
                    {
                        layoutElement.ignoreLayout = false;

                    }
                    layoutElement.FillPreferredHeight();
                    layoutElement.flexibleHeight = -1;
                    layoutElement.flexibleWidth = 1;
                    break;
                case LayoutElementState.lyoutStretchVertical:
                    if (layoutElement.ignoreLayout)
                    {
                        layoutElement.ignoreLayout = false;

                    }
                    layoutElement.FillPreferredWidth();
                    layoutElement.flexibleHeight = 1;
                    layoutElement.flexibleWidth = -1;
                    break;
                case LayoutElementState.layoutNotFlexible:
                    layoutElement.ignoreLayout = false;
                    layoutElement.FillPreferred();
                    layoutElement.flexibleHeight = -1;
                    layoutElement.flexibleWidth = -1;

                    break;
            }


        }

    } // more layout class
    public static class zExtensionsLayout
    {

        const float defaultSize = 100;

        public static void FillPreferredHeight(this LayoutElement l, bool resetIfDegenerate = true)
        {
            if (l == null) return;
            float currentHeight = l.transform.GetHeight();
            if (currentHeight <= 0) currentHeight = defaultSize;
            l.preferredHeight = currentHeight;


        }
        public static void FillPreferredWidth(this LayoutElement l, bool resetIfDegenerate = true)
        {
            if (l == null) return;
            float currentWidth = l.transform.GetWidth();
            if (currentWidth <= 0) currentWidth = defaultSize;
            l.preferredWidth = currentWidth;


        }

        public static void FillPreferred(this LayoutElement l, bool resetIfDegenerate = true)
        {

            l.FillPreferredHeight(resetIfDegenerate);
            l.FillPreferredWidth(resetIfDegenerate);


        }
        public static int GetActiveElementCount(this VerticalLayoutGroup layout)
        {
            int count = 0;
            if (layout == null) return count;
            for (int i = 0; i < layout.transform.childCount; i++)
            {
                GameObject thisChild = layout.transform.GetChild(i).gameObject;
                if (thisChild != null)
                {
                    LayoutElement le = thisChild.GetComponent<LayoutElement>();
                    if (le != null)
                    {
                        if (!le.ignoreLayout) count++;
                    }
                }

            }

            return count;
        }
        public static ContentSizeFitter AddContentSizeFitter(this RectTransform rect)
        {
            ContentSizeFitter c = rect.gameObject.AddOrGetComponent<ContentSizeFitter>();
            c.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            c.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            return c;
        }

        public static LayoutElement GetLayout(RectTransform rect)
        {
            LayoutElement le = null;
            LayoutGroup lg = rect.GetComponent<LayoutGroup>();
            if (lg != null)
            {
                le = rect.gameObject.AddComponent<LayoutElement>();

                le.flexibleHeight = 1;
                le.flexibleWidth = 1;
            }
            return le;
        }
        /*         public static void removeAutoLayout(this Component c)
                {
                    ContentSizeFitter contentSizeFitter = c.GetComponent<ContentSizeFitter>();
                    if (contentSizeFitter != null) contentSizeFitter.DestroySmart();
                    LayoutElement le = c.GetComponent<LayoutElement>();
                    if (le != null) le.DestroySmart();
                    LayoutHelper lh = c.GetComponent<LayoutHelper>();
                    if (lh != null) lh.DestroySmart();
                    VerticalLayoutGroup v = c.GetComponent<VerticalLayoutGroup>();
                    if (v != null) v.DestroySmart();
                    HorizontalLayoutGroup h = c.GetComponent<HorizontalLayoutGroup>();
                    if (h != null) h.DestroySmart();


                }
        */
        public static VerticalLayoutGroup ToVeritical(this HorizontalLayoutGroup layout)
        {

            RectOffset padding = layout.padding;
            float spacing = layout.spacing;
            bool childForceExpandHeight = layout.childForceExpandHeight;
            bool childForceExpandWidth = layout.childForceExpandWidth;
            bool childControlHeight = layout.childControlHeight;
            bool childControlWidth = layout.childControlWidth;
            GameObject g = layout.gameObject;
            GameObject.DestroyImmediate(layout);
            VerticalLayoutGroup ng = g.AddComponent<VerticalLayoutGroup>();
            ng.childForceExpandHeight = false;
            ng.childForceExpandWidth = false;
            ng.childControlHeight = true;
            ng.childControlWidth = true;
            ng.spacing = spacing;
            ng.padding = padding;
            return ng;
        }

        public static HorizontalLayoutGroup ToHorizontal(this VerticalLayoutGroup layout)
        {

            RectOffset padding = layout.padding;
            float spacing = layout.spacing;
            bool childForceExpandHeight = layout.childForceExpandHeight;
            bool childForceExpandWidth = layout.childForceExpandWidth;
            bool childControlHeight = layout.childControlHeight;
            bool childControlWidth = layout.childControlWidth;
            GameObject g = layout.gameObject;
            GameObject.DestroyImmediate(layout);
            HorizontalLayoutGroup ng = g.AddComponent<HorizontalLayoutGroup>();
            ng.childForceExpandHeight = false;
            ng.childForceExpandWidth = false;
            ng.childControlHeight = true;
            ng.childControlWidth = true;
            ng.spacing = spacing;
            ng.padding = padding;
            return ng;
        }



        public static void SetMargin(this HorizontalLayoutGroup layout, int margin = 0)

        {
            if (layout == null) return;
            layout.padding = new RectOffset(margin, margin, margin, margin);
        }

        public static void SetMargin(this VerticalLayoutGroup layout, int margin = 0)

        {
            if (layout == null) return;
            layout.padding = new RectOffset(margin, margin, margin, margin);
        }

        public static Image AddImageChild(this GameObject g, float opacity = 0.3f)
        {
            Image image = g.AddChildRectTransform().gameObject.AddComponent<Image>();
            image.color = new Color(Random.value * 0.3f + 0.7f,
                                       Random.value * 0.3f + 0.7f,
                                       Random.value * 0.2f, opacity);
            image.sprite = Resources.Load("Background") as Sprite;
            image.name = "Image";
            return image;
        }
        public static Text AddTextChild(this GameObject g, string content = "no text")
        {
            Text text = g.AddChildRectTransform().gameObject.AddComponent<Text>();
            text.text = content;
            return text;
        }
        public static Image AddImageChild(this RectTransform rect, float opacity = 0.3f)
        {
            return rect.gameObject.AddImageChild(opacity);
        }


        public static float GetWidth(this RectTransform r)
        {
            return r.rect.width;
        }
        public static float GetHeight(this RectTransform r)
        {
            return r.rect.height;
        }
        public static float GetWidth(this Transform t)
        {
            RectTransform r = t.GetComponent<RectTransform>();
            if (r != null)
                return r.rect.width;
            else return -1;
        }
        public static float GetHeight(this Transform t)
        {
            RectTransform r = t.GetComponent<RectTransform>();
            if (r != null)
                return r.rect.height;
            else return -1;
        }
        public static float GetWidth(this GameObject t)
        {
            RectTransform r = t.GetComponent<RectTransform>();
            if (r != null)
                return r.rect.width;
            else return -1;
        }
        public static float GetHeight(this GameObject t)
        {
            RectTransform r = t.GetComponent<RectTransform>();
            if (r != null)
                return r.rect.height;
            else return -1;
        }
        public static void SetChildControl(this HorizontalLayoutGroup layout, float spacing = 0)

        {
            if (layout == null) return;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.spacing = spacing;
        }

        public static void SetChildControl(this VerticalLayoutGroup layout, float spacing = 0)

        {
            if (layout == null) return;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.spacing = spacing;
        }
        public static LayoutElement[] GetActiveLayoutElements(this VerticalLayoutGroup layout)
        {
            List<LayoutElement> elements = new List<LayoutElement>();
            if (layout == null) return elements.ToArray();
            for (int i = 0; i < layout.transform.childCount; i++)
            {
                GameObject thisChild = layout.transform.GetChild(i).gameObject;
                LayoutElement le = thisChild.GetComponent<LayoutElement>();
                if (le != null && !le.ignoreLayout) elements.Add(le);
            }
            return elements.ToArray();
        }

        public static LayoutElement[] GetActiveLayoutElements(this HorizontalLayoutGroup layout)
        {
            List<LayoutElement> elements = new List<LayoutElement>();
            if (layout == null) return elements.ToArray();
            for (int i = 0; i < layout.transform.childCount; i++)
            {
                GameObject thisChild = layout.transform.GetChild(i).gameObject;
                LayoutElement le = thisChild.GetComponent<LayoutElement>();
                if (le != null && !le.ignoreLayout) elements.Add(le);
            }
            return elements.ToArray();
        }

        public static ILayoutElement[] GetILayoutElements(this HorizontalLayoutGroup layout)
        {
            List<ILayoutElement> elements = new List<ILayoutElement>();
            if (layout == null) return elements.ToArray();
            for (int i = 0; i < layout.transform.childCount; i++)
            {
                GameObject thisChild = layout.transform.GetChild(i).gameObject;
                ILayoutElement le = thisChild.GetComponent<ILayoutElement>();
                if (le != null) elements.Add(le);
            }
            return elements.ToArray();
        }


        public static ILayoutElement[] GetILayoutElements(this VerticalLayoutGroup layout)
        {
            List<ILayoutElement> elements = new List<ILayoutElement>();
            if (layout == null) return elements.ToArray();
            for (int i = 0; i < layout.transform.childCount; i++)
            {
                GameObject thisChild = layout.transform.GetChild(i).gameObject;
                ILayoutElement le = thisChild.GetComponent<ILayoutElement>();
                if (le != null) elements.Add(le);
            }
            return elements.ToArray();
        }
        public static LayoutElement[] GetActiveLayoutElements(this GameObject g)
        {
            List<LayoutElement> elements = new List<LayoutElement>();
            Debug.Log("seacrihg " + g.transform.childCount);
            for (int i = 0; i < g.transform.childCount; i++)
            {
                GameObject thisChild = g.transform.GetChild(i).gameObject;
                LayoutElement le = thisChild.GetComponent<LayoutElement>();
                if (le == null) Debug.Log(" NO LAYUT ELEMENT ON GAMEOBJECT " + thisChild.name, thisChild);
                else
                {
                    //     LayoutHelper lh = thisChild.GetComponent<LayoutHelper>();
                    //     if (lh != null) Debug.Log("lh present");
                    //     else
                    if (!le.ignoreLayout)
                        elements.Add(le);
                }

            }


            return elements.ToArray();
        }


        public static void AddLayoutElement(this GameObject go, bool ignore = true)
        {
            LayoutElement layoutElement = go.GetComponent<LayoutElement>();
            if (layoutElement == null) layoutElement = go.AddComponent<LayoutElement>();
            layoutElement.ignoreLayout = ignore;
            //    layoutElement.CollapseComponent();


        }
        public static void SetAsSiblingTo(this GameObject thisGameObject, GameObject tarGet)
        {
            thisGameObject.transform.SetAsSiblingTo(tarGet.transform);


        }
        public static void SetAsSiblingTo(this Transform thisTransform, GameObject tarGet)
        {
            thisTransform.SetAsSiblingTo(tarGet.transform);
        }
        public static void SetAsSiblingTo(this Transform thisTransform, Transform tarGet)
        {
            int tarGetIndex = tarGet.GetSiblingIndex();
            thisTransform.SetParent(tarGet.parent);
            thisTransform.SetSiblingIndex(tarGetIndex + 1);


        }

        public static void StretchToParent(this GameObject go, float margin = 0)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            if (rect != null) rect.StretchToParent(margin);

        }
        public static void StretchToParent(this RectTransform rect, float margin = 0)
        {
            if (rect == null)
            {
                Debug.Log("no rect");
                return;
            }
            if (rect.parent == null)
            {
                Debug.Log("no parent");
                return;
            }
            RectTransform parentRect = rect.parent.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(margin, margin);
            rect.anchorMax = new Vector2(1 - margin, 1 - margin);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;


        }
        public static bool isCanvas(this GameObject g)
        {
            return g.GetComponent<Canvas>() != null;
        }
        public static bool hasCanvasParent(this GameObject g)
        {
            if (g.GetComponent<Canvas>() != null) return false;
            return (g.GetComponentInParent<Canvas>() != null);
        }

        public static bool hasVerticalLayout(this GameObject g)
        {
            return g.GetComponent<VerticalLayoutGroup>() != null;
        }

        public static bool hasHorizontalLayout(this GameObject g)
        {
            return g.GetComponent<HorizontalLayoutGroup>() != null;
        }


    }

}