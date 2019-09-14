using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace zUI
{
    public class LayoutCreator : MonoBehaviour
    {
        public DrawInspectorBg draw;
        void Reset()
        {
            var image = GetComponent<Image>();
            if (image != null) image.enabled = false;
        }
        public int columns = 4;
        public int maxItemsTop = 4;
        public int maxItemsBottom = 2;
        public Color startColor = new Color(0, 0.408f, 0.945f, 0.559f);
        public Color endColor = new Color(0.000f, 0.851f, 0.302f, 0.578f);
         public Color textColor = Color.white * 0.8f;
        public Color topColor = Color.white * 0.4f;
        float flexHeight = 0.2f;
        float flexWidth = 0.02f;
        public bool leaveLastColumnEmpty = true;


        [ExposeMethodInEditor]
        public void CreateSetup()
        {
            if (columns < 2) columns = 2;
            var columnParentRect = LayoutEditorUtilities.CreateHoritontalOrVertical(GetComponent<RectTransform>(), LayoutEditorUtilities.LayoutDirection.Horizontal, columns, 0.01f);
            columnParentRect.sizeDelta = new Vector2(-2 * LayoutBorderDragger.borderSize, -6 * LayoutBorderDragger.borderSize);
            var layoutGroup = columnParentRect.GetComponent<HorizontalLayoutGroup>();
            layoutGroup.GetComponent<Image>().enabled = false;
            layoutGroup.spacing = LayoutBorderDragger.borderSize * 3;
            var padding = layoutGroup.padding;
            padding.top = LayoutBorderDragger.borderSize * 2;
            padding.left = LayoutBorderDragger.borderSize;
            padding.bottom = LayoutBorderDragger.borderSize * 2;
            padding.right = LayoutBorderDragger.borderSize;
            layoutGroup.padding = padding;
            List<LayoutPanel> panels = new List<LayoutPanel>();
            for (int i = 0; i < columnParentRect.childCount; i++)
            {
                var thisColumn = columnParentRect.GetChild(i).gameObject;
                thisColumn.name = "Column " + i;
                AddColumnBorders(thisColumn, Color.Lerp(startColor, endColor, (float)i / columnParentRect.childCount));
                thisColumn.AddComponent<LayoutBorderHide>();
                thisColumn.GetComponent<Image>().enabled = false;
                if (i < columnParentRect.childCount - 1 || !leaveLastColumnEmpty) AddRandomChildren(thisColumn.gameObject, Random.Range(1, maxItemsTop), ref panels);
                CreateSpacer(thisColumn.gameObject,flexWidth,flexWidth);
                if (i < columnParentRect.childCount - 1 || !leaveLastColumnEmpty) AddRandomChildren(thisColumn.gameObject, Random.Range(0, maxItemsBottom), ref panels);
                thisColumn.AddOrGetComponent<LayoutColumn>();
            }

            SetColors(panels);
            UpdateBorders();
        }
        void UpdateBorders()
        {
            var borders = gameObject.GetComponentsInChildren<LayoutBorderDragger>();
            foreach (var b in borders) b.side = b.side;
        }
        void AddRandomChildren(GameObject parent, int count, ref List<LayoutPanel> panels)
        {
            for (int j = 0; j < count; j++)
                panels.Add(CreatePanel(parent, "Item [" + (panels.Count + 1) + " ] " + zExt.RandomString(3)));

        }


        void SetColors(List<LayoutPanel> panels)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                var bh = panels[i].GetComponent<LayoutBorderHide>();
                bh.borderColor = Color.Lerp(startColor, endColor, (float)i / panels.Count);
                bh.editColors = true;
                bh.editColors = false;
                var img = panels[i].GetComponent<Image>();
                img.color = Color.Lerp(startColor, endColor, 1 - (float)i / panels.Count);
            }
        }
       public static LayoutElement CreateSpacer(GameObject thisChild, float flexWidth=0.2f, float flexHeight=0.2f)
        {
            var img = thisChild.AddChildRectTransform();
            var le = img.gameObject.AddComponent<LayoutElement>();
            le.flexibleHeight = flexHeight;
            le.flexibleWidth = flexWidth;
            le.name = LayoutPanel.spacerName;
            return le;
        }
        LayoutPanel CreatePanel(GameObject parent, string newName = "NoName")
        {

            var thisGrandChild = parent.AddImageChild();
            thisGrandChild.name = newName;

            var creator = thisGrandChild.gameObject.AddComponent<LayoutItemCreator>();
            creator.AddBordersAndSampleContent();

            var panel = creator.GetComponent<LayoutPanel>();
            var contentle = panel.resizableElement;
            #if UNITY_EDITOR
                var canvasrend=panel.GetComponent<CanvasRenderer>();
                

            #endif

            contentle.preferredHeight = Random.Range(LayoutTopControl.topHeight, LayoutTopControl.topHeight * 5);
            creator.RemoveMe();
            thisGrandChild.transform.SetParent(parent.transform);
            return panel;
        }
        void AddColumnBorders(GameObject target, Color color)
        {
            var a = target.AddImageChild();
            var b = target.AddImageChild();
            a.color = color;
            b.color = color;
            var d = a.gameObject.AddComponent<LayoutBorderDragger>();
            var e = b.gameObject.AddComponent<LayoutBorderDragger>();
            e.columnMode = true;
            d.columnMode = true;
            e.side = LayoutBorderDragger.Side.Bottom;
            d.side = LayoutBorderDragger.Side.Top;
        }

        [ExposeMethodInEditor]
        public void CreateHorizontalLayout()
        {
            LayoutEditorUtilities.CreateHoritontalOrVertical(GetComponent<RectTransform>(), LayoutEditorUtilities.LayoutDirection.Horizontal, columns, 0.01f);
        }

        [ExposeMethodInEditor]
        public void CreateVerticalLayout()
        {
            LayoutEditorUtilities.CreateHoritontalOrVertical(GetComponent<RectTransform>(), LayoutEditorUtilities.LayoutDirection.Vertical, columns, 0.01f);
        }

        [ExposeMethodInEditor]
        public void RemoveAllChildren()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
#if UNITY_EDITOR
                Undo.DestroyObjectImmediate(transform.GetChild(i).gameObject);
#else
                DestroyImmediate(transform.GetChild(i).gameObject);
#endif
        }
        //         [ExposeMethodInEditor]

        //         public void RemoveMe()
        //         {
        // #if UNITY_EDITOR
        //             Undo.DestroyObjectImmediate(this);
        // #else
        //             DestroyImmediate(this);
        // #endif
        //         }

    }
}
