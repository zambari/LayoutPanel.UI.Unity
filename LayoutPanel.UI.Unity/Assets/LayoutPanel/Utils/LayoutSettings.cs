using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z.LayoutPanel
{
    public static class LayoutSettings
    {

        public static readonly int minRectWidth = 50; // won't allow scaling lower than that
        public static readonly int minRectHeight = 30; // won't allow scaling lower than that

        public static Color dropTargetColor { get { return new Color(0.2f, 1, 0.2f, 0.8f); } }
        public static Color dropTargetColorWhenSplit { get { return new Color(.7f, 0.2f, 0.7f, 0.8f); } }

        const float columnModeOffset = 3f;

        // defaults
#if UNITY_ANDROID && !UNITY_EDITOR
        static int __borderSpacing = 5;
#else
        static int __borderSpacing = 2;
#endif
        // static int _verticalSpacing = 1;
        static int _topHeight = 25;
#if UNITY_ANDROID && !UNITY_EDITOR
        static int _borderSize =9;
#else
        static int _borderSize = 7;

#endif
        public static float borderSizeColumnOffset { get { return _borderSize * columnModeOffset; } } // set { _borderSize = value; if (onBorderSizeChange != null) onBorderSizeChange(); } }

        public static int borderSize { get { return _borderSize; } set { _borderSize = value; if (onBorderSizeChange != null) onBorderSizeChange(); } }
        public static int groupSpacing
        {
            get
            {
                //  var x = 2 * _borderSize + borderSpacing; ;
                return 5;
                //  return x;
            }
            set
            {
                value -= 2 * borderSize;
                if (value < 0) return;
                borderSpacing = value;
                if (onBorderSizeChange != null) onBorderSizeChange();
            }
        }
        public static int borderSpacing { get { return __borderSpacing; } set { __borderSpacing = value; if (onBorderSizeChange != null) onBorderSizeChange(); } }
        // public static int verticalSpacing { get { return _verticalSpacing; } set { _verticalSpacing = value; if (onBorderSizeChange != null) onBorderSizeChange(); } }
        //static int _topHeight = 9;
        public static int topHeight { get { return _topHeight; } set { _topHeight = value; if (onBorderSizeChange != null) onBorderSizeChange.Invoke(); } }
        public static int minWidth { get { return topHeight * 4; } }
        public static System.Action onBorderSizeChange;
        public static int internalGroupPadding { get { return 5; } }
        public static int groupPaddigNoPanel { get { return 0; } }
        public static int groupPaddigPanel { get { return borderSize + internalGroupPadding; } }
        static RectOffset GetRectOffset(LayoutPanel panel)
        {
            RectOffset padding = new RectOffset();
            int size = (panel == null) ? groupPaddigNoPanel : groupPaddigPanel;
            padding.top = size;
            padding.left = size;
            padding.bottom = size;
            padding.right = size;
            return padding;
        }

        public static void SetPadding(this HorizontalLayoutGroup layoutGroup, LayoutPanel panel)
        {
            layoutGroup.padding = GetRectOffset(panel);
        }

        public static void SetPadding(this VerticalLayoutGroup layoutGroup, LayoutPanel panel)
        {


            layoutGroup.padding = GetRectOffset(panel);
        }
        public static void SetSpacing(this VerticalLayoutGroup group)
        {
            group.spacing = LayoutSettings.groupSpacing;
        }
        public static void SetSpacing(this HorizontalLayoutGroup group)
        {
            group.spacing = LayoutSettings.groupSpacing;
        }
        public static void SetMins(this LayoutElement le)
        {
            le.minHeight = topHeight;
            le.minWidth = minWidth;
        }

        public static List<GameObject> SplitToLayout(this GameObject container, bool horizontal, int count, float flex = -2)
        {
            return container.GetComponent<RectTransform>().SplitToLayout(horizontal, count, flex);

        }
        public static void SetChildControlFromPanel(this HorizontalLayoutGroup layout, float spacing = 0)

        {
            if (layout == null) return;
            layout.childForceExpandHeight = true;
            layout.childForceExpandWidth = false;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.spacing = spacing;
        }
        public static void SetParams(this HorizontalLayoutGroup group, LayoutPanel panel)
        {
            group.SetChildControlFromPanel();

            group.SetPadding(panel);
            group.SetSpacing();
        }
        public static void SetParams(this VerticalLayoutGroup group, LayoutPanel panel)
        {
            group.SetChildControlFromPanel();
            group.SetPadding(panel);
            group.SetSpacing();
        }
        public static void SetChildControlFromPanel(this VerticalLayoutGroup layout, float spacing = 0)

        {
            if (layout == null) return;
            layout.childForceExpandHeight = false; // ?
            layout.childForceExpandWidth = true;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.spacing = spacing;
        }
        public static List<GameObject> SplitToLayout(this RectTransform container, bool horizontal, int count, float flex = -2)
        {

            if (!horizontal)
            {
                var oldgroup = container.gameObject.GetComponent<HorizontalLayoutGroup>();
                if (oldgroup != null) GameObject.DestroyImmediate(oldgroup);
                var group = container.gameObject.AddOrGetComponent<VerticalLayoutGroup>();
                group.SetChildControlFromPanel();
                group.SetSpacing();
            }
            else
            {
                var oldgroup = container.gameObject.GetComponent<VerticalLayoutGroup>();
                if (oldgroup != null) GameObject.DestroyImmediate(oldgroup);
                var group = container.gameObject.AddOrGetComponent<HorizontalLayoutGroup>();
                group.SetChildControlFromPanel();
                group.SetSpacing();
            }
            List<GameObject> cretedObjects = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                RectTransform child = container.AddChild();

#if UNITY_EDITOR

                UnityEditor.Undo.RegisterCreatedObjectUndo(child, "layoutt");
#endif
                cretedObjects.Add(child.gameObject);
                child.anchorMin = new Vector2(0, 0);
                child.anchorMax = new Vector2(1, 1);
                child.offsetMin = new Vector2(0, 0);
                child.offsetMax = new Vector2(0, 0);
                Image im = child.gameObject.AddComponent<Image>();
                im.color = im.color.Random();
                child.name = "Item " + (i + 1);
                LayoutElement le = child.gameObject.AddComponent<LayoutElement>();

                if (flex == -2)
                {
                    le.flexibleHeight = (!horizontal ? 1f / count : 1);
                    le.flexibleWidth = (!horizontal ? 1 : 1f / count);
                }
                else
                {
                    le.flexibleHeight = flex;
                    le.flexibleWidth = flex;
                }
                child.localScale = Vector3.one; //why do we need this
            }
            container.name = (horizontal ? "HorizontalLayout" : "VerticalLayout");
            return cretedObjects;
        }
    }
}
