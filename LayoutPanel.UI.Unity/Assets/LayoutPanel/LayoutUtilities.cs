
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace zUI
{

    // v0.2  duzo dobrego
    // v0.3  napespace
    // v.0.4 undo??
    // v.0.5 scale fix
    // v.0.6 incorporeated into layout panel
    public class LayoutEditorUtilities
    {
        const int defaultSpacing = 5;
        public enum LayoutDirection { Horizontal, Vertical };


        // [MenuItem("GameObject/UI/Create layout panel child")]
        // static RectTransform CreaatePanelChildMenu()
        // {
        //     RectTransform rect = CreaatePanelChild();
        //     if (rect != null)
        //         Selection.activeGameObject = rect.gameObject;
        //     Undo.RegisterCreatedObjectUndo(rect.gameObject, "Create object");
        //     return rect;
        // }



        static Transform CreateCanvasIfNotPresent()
        {
            //  if (Selection.activeGameObject != null || Selection.activeGameObject.GetComponentInParent<Canvas>() != null) return Selection.activeGameObject.transform; //GetComponentInParent<Canvas>().transform;
            Canvas can = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
            if (can != null)
                return can.transform;
            else
            {
                GameObject c = new GameObject("Canvas", typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasScaler));
#if UNITY_EDITOR
                Undo.RegisterCreatedObjectUndo(c, "layouyt");
#endif
                c.AddComponent<Canvas>();
                c.AddComponent<GraphicRaycaster>();
                c.AddComponent<CanvasScaler>();
                // Selection.activeGameObject = c;
                EventSystem e = GameObject.FindObjectOfType<EventSystem>();
                if (e == null)
                {
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }

                return c.transform;
            }
        }
#if UNITY_EDITOR

        [MenuItem("GameObject/UI/Add Horizontal Layout to selected RectTransform")]
        public static void CreateHorizontalLayout()
        {
            Selection.activeObject = CreateHoritontalOrVertical(Selection.activeGameObject.GetComponent<RectTransform>(), LayoutDirection.Horizontal, 3);
        }
        [MenuItem("GameObject/UI/Add Vertical Layout to selected RectTransform")]
        public static void CreateVerticalLayout()
        {
            Selection.activeObject = CreateHoritontalOrVertical(Selection.activeGameObject.GetComponent<RectTransform>(), LayoutDirection.Vertical, 3);
        }


        [MenuItem("GameObject/UI/Add Layout Spacer")]
        public static LayoutElement AddFlexibleSpacer()
        {
            var img = Selection.activeGameObject.AddChildRectTransform();
            var le = img.gameObject.AddComponent<LayoutElement>();
            le.flexibleHeight = 1;
            le.flexibleWidth = 1;
            le.name = LayoutPanel.spacerName;

            Undo.RegisterCreatedObjectUndo(le.gameObject, "SPACER");
            Selection.activeGameObject = le.gameObject;

            return le;
            //Selection.activeObject = CreateHoritontalOrVertical(LayoutDirection.Horizontal, 3);
        }
#endif

        public static RectTransform CreateHoritontalOrVertical(RectTransform target, LayoutDirection dir, int count, float flex = -2)
        {


            if (target == null)
#if UNITY_EDITOR

                if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<RectTransform>() != null)
                {
                    target = Selection.activeGameObject.GetComponent<RectTransform>();
                    Undo.RecordObject(target, "adding layout 1");
                    Undo.RecordObject(Selection.activeGameObject, "Adding layout");
                }
#else
            return null;

#endif

            return PopulateLayout(target.AddImageChild().GetComponent<RectTransform>(), dir, count, flex);


            // var canvas = CreateCanvasIfNotPresent();


            // RectTransform container = canvas.GetComponent<RectTransform>();
            // {
            //     Debug.Log("rect selection");
            //     container = Selection.activeGameObject.GetComponent<RectTransform>();

            //     var newObj = new GameObject("Layout", typeof(RectTransform), typeof(Image));
            //     Undo.RegisterCreatedObjectUndo(newObj, "Layout");

            //     newObj.transform.SetParent(container);
            //     container = newObj.GetComponent<RectTransform>();
            //     Selection.activeObject = container;
            //     //            container=container.gameObject.AddChildRectTransform();
            // }

            // if (container == null)
            // {
            //     Debug.Log("no container");
            //     var newCont = new GameObject("RectContainer", typeof(RectTransform), typeof(Image));
            //     Undo.RegisterCreatedObjectUndo(newCont, "Layout");
            //     container = newCont.GetComponent<RectTransform>();
            // }

            // Image a = container.GetComponent<Image>();
            // if (a != null) a.enabled = false;

        }
        static RectTransform PopulateLayout(RectTransform container, LayoutDirection dir, int count, float flex = -2)
        {
            bool vertical = dir == LayoutDirection.Vertical;

            if (vertical)
            {
               var group=  container.gameObject.AddComponent<VerticalLayoutGroup>();
               group .SetChildControl();
               group.spacing=10;
            }
            else
            {
               var group= container.gameObject.AddComponent<HorizontalLayoutGroup>();
               group .SetChildControl();
               group.spacing=10;
            }
            List<GameObject> cretedObjects = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                RectTransform child = container.AddChild();
                
#if UNITY_EDITOR

                Undo.RegisterCreatedObjectUndo(child, "layoutt");
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
                    le.flexibleHeight = (vertical ? 1f / count : 1);
                    le.flexibleWidth = (vertical ? 1 : 1f / count);
                }
                else
                {
                    le.flexibleHeight = flex;
                    le.flexibleWidth = flex;
                }
                child.localScale = Vector3.one; //why do we need this

            }
            container.name = (vertical ? "VerticalLayout" : "HorizontalLayout");
            return container;
        }

#if UNITY_EDITOR

        [MenuItem("Tools/Layout H<->V Converion")]
        static void ConvertLayout()
        {

            if (Selection.activeGameObject == null) { Debug.Log("nothing selected"); return; }
            VerticalLayoutGroup vg = Selection.activeGameObject.GetComponentInChildren<VerticalLayoutGroup>();
            HorizontalLayoutGroup hg = Selection.activeGameObject.GetComponent<HorizontalLayoutGroup>();

            if (vg == null && hg == null) { Debug.Log(" no layout group"); return; }


            if (vg != null)
            {
                vg.ToHorizontal();


            }
            else
                hg.ToVeritical();

        }


        [MenuItem("Tools/Layout group Vertical from Selected")]
        static void GroupToLayouVt()
        {
            GameObject g = Selection.activeGameObject;
            Undo.RecordObject(g, "Layout");
            if (g == null) return;
            RectTransform rect = CreateGroup();
            if (rect != null)
            {
                VerticalLayoutGroup layout = rect.gameObject.AddComponent<VerticalLayoutGroup>();
                layout.SetMargin(defaultSpacing);
                layout.SetChildControl(defaultSpacing);
                rect.AddContentSizeFitter();
            }
        }

        [MenuItem("Tools/Layout Horizontal Layuout from selected objects")]
        static void GroupToLayoutH()
        {
            GameObject g = Selection.activeGameObject;
            if (g == null) return;
            RectTransform rect = CreateGroup();
            if (rect != null)
            {
                HorizontalLayoutGroup layout = rect.gameObject.AddComponent<HorizontalLayoutGroup>();
                layout.SetMargin(defaultSpacing);
                layout.SetChildControl(defaultSpacing);
                rect.AddContentSizeFitter();
            }
        }


        static RectTransform CreateGroup()
        {
            if (Selection.activeGameObject == null) { Debug.Log("nothing selected"); return null; }
            if (Selection.activeGameObject.transform.parent == null) { Debug.Log("no parent "); return null; }
            RectTransform rect = CreaatePanelChild();
            rect.anchorMin = Vector2.one / 2;
            rect.anchorMax = Vector2.one / 2;
            rect.SetParent(Selection.activeGameObject.transform.parent);
            // Debug.Log("pareting "+rect.name+" to "+)
            rect.SetSiblingIndex(Selection.activeGameObject.transform.GetSiblingIndex());
            float maxH = 10;
            float maxW = 10;
            float sumH = 0;
            int count = 0;
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                GameObject g = Selection.objects[i] as GameObject;
                if (g != null)
                {
                    RectTransform grect = g.GetComponent<RectTransform>();
                    if (grect == null) continue;
                    //                float h = grect.GetHeight();
                    //              float w = grect.GetWidth();
                    count++;
                    float w = grect.sizeDelta.x;
                    float h = grect.sizeDelta.y;
                    sumH += h;

                    if (h > maxH) maxH = h;
                    if (w > maxW) maxW = w;
                    LayoutElement le = grect.GetComponent<LayoutElement>();
                    if (le == null)
                    {

                        le = grect.gameObject.AddComponent<LayoutElement>();
                        le.preferredWidth = w;
                        le.preferredHeight = h;
                        le.flexibleWidth = 1;
                    }
                    grect.SetParent(rect);
                }
            }
            rect.SetSizeXY(maxW, sumH + (2 + count) * defaultSpacing);
            //rect.sizeDelta=new Vector2(maxW,maxH);
            return rect;
        }
#endif

        static RectTransform CreaatePanelChild()
        {
            GameObject go = new GameObject("Panel");
#if UNITY_EDITOR
            if (Selection.activeGameObject != null) go.transform.SetParent(Selection.activeGameObject.transform);
            else
#endif
                go.transform.SetParent(CreateCanvasIfNotPresent());
            go.transform.localPosition = Vector2.zero;
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            Image image = go.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.2f);
#if UNITY_EDITOR
            image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
#endif
            image.type = Image.Type.Sliced;
            rect.localScale = Vector3.one;
            //  Selection.activeGameObject = go;
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(go, "Create object");
#endif

            return rect;
        }

    }

}
