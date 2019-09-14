//#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
namespace zUI
{



    [ExecuteInEditMode]
    public class LayoutItemCreator : MonoBehaviour
    {
        public DrawInspectorBg draw;
        [SerializeField] LayoutTopControl topControl;

        [SerializeField] LayoutPanel panel;
        [SerializeField] LayoutFoldController foldController;
        [SerializeField] Button foldButton;
        [SerializeField] LayoutElement content;
        [SerializeField] LayoutBorderHide layoutBorderHide;
        [Header("adds a text raycast catcher")]
        [SerializeField] float borderOverScan = 10;
        Color textColor = Color.white * 0.8f;
        LayoutCreator layoutCreator;
        void OnValidate()
        {
            TryToGetReferences();
        }

        void TryToGetReferences()
        {
            if (topControl == null) topControl = GetComponentInChildren<LayoutTopControl>();
            if (panel == null) panel = gameObject.GetComponent<LayoutPanel>();
            if (foldController == null) foldController = gameObject.GetComponent<LayoutFoldController>();
            if (foldController != null) foldButton = foldController.foldButton;
            if (layoutBorderHide == null) layoutBorderHide = GetComponent<LayoutBorderHide>();
        }

        void Awake()
        {
            layoutCreator = GetComponentInParent<LayoutCreator>();
            if (layoutCreator != null)
            {
                textColor = layoutCreator.textColor;
            }
        }
        [ExposeMethodInEditor]
        public void AddBordersAndSampleContent()
        {



            if (name.Contains(LayoutPanel.spacerName)) name = "Item " + zExt.RandomString(4);
            AddBordersOnly();
            var fold = gameObject.AddOrGetComponent<LayoutFoldController>();
            if (fold.foldButton == null)
            {
                foldButton = CreateFoldButton();
                fold.foldButton = foldButton;
            }
            else foldButton = fold.foldButton;
            Fold(fold);
            VerticalLayoutGroup group = gameObject.AddOrGetComponent<VerticalLayoutGroup>();
            group.SetChildControl(2);
            Fold(group);
            content = gameObject.AddImageChild().AddOrGetComponent<LayoutElement>();
            content.minHeight = LayoutTopControl.topHeight;
            content.minWidth = 26;
            content.flexibleWidth = 0.001f;
            content.GetComponent<Image>().enabled = false;
            content.name = "CONTENT";
            Fold(content);
            layoutBorderHide = gameObject.AddOrGetComponent<LayoutBorderHide>();
#if UNITY_EDITOR
            Selection.activeObject = content;
#endif

            Fold(GetComponent<RectTransform>());
            TryToGetReferences();

            var panel = gameObject.AddOrGetComponent<LayoutPanel>();
            if (panel.resizableElement == null)
                panel.resizableElement = content;


        }

        [ExposeMethodInEditor]
        public void AddBordersOnly()
        {
            int count = System.Enum.GetNames(typeof(LayoutBorderDragger.Side)).Length;
            for (int i = 0; i < count; i++)
            {
                var a = gameObject.AddImageChild();
                var d = a.gameObject.AddComponent<LayoutBorderDragger>();
                if (borderOverScan > 0)
                {
                    var text = a.gameObject.AddTextChild();
                    text.text = null;
                    text.supportRichText = false;
                    text.GetComponent<RectTransform>().sizeDelta = new Vector2(borderOverScan, borderOverScan);
                    text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    text.name = "raycast catcher";
                    text.font = font;
                    text.color = textColor;
                }

                d.side = (LayoutBorderDragger.Side)i;
#if UNITY_EDITOR
                Undo.RegisterCreatedObjectUndo(a.gameObject, "borders");
#endif

            }
            topControl = GetComponentInChildren<LayoutTopControl>();
            if (topControl == null)
            {
                var topImage = gameObject.AddImageChild();
                topControl = topImage.gameObject.AddOrGetComponent<LayoutTopControl>();
                if (layoutCreator != null)
                {
                    topImage.color = layoutCreator.topColor;
                }

            }
            Text labelText = topControl.GetComponentInChildren<Text>();

            if (labelText == null)
            {

                labelText = topControl.gameObject.AddTextChild();

                labelText.name = "Panel Label";
                labelText.text = name;
                labelText.font = font;
                labelText.alignment = TextAnchor.MiddleLeft;
                labelText.color = textColor;
                var textRect = labelText.GetComponent<RectTransform>();

                textRect.anchorMin = new Vector2(0f, 0f);
                textRect.anchorMax = new Vector2(1f, 1f);
                textRect.anchoredPosition = new Vector2(-7.5f, 0f);
                textRect.sizeDelta = new Vector2(-25f, 4f);
                textRect.pivot = new Vector2(0.5f, 0.5f);


            }

        }
        public static Font font { get { return Resources.GetBuiltinResource<Font>("Arial.ttf"); } }

        [ExposeMethodInEditor]
        public void RemoveMe()
        {
#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(this);
#endif
        }
        void Fold(Component c)
        {
#if UNITY_EDITOR
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
        }
        Button CreateFoldButton()
        {
            var foldButton = topControl.gameObject.AddImageChild().gameObject;
            if (foldButton == null) return null;
            var button = foldButton.AddComponent<Button>();

            //Click on the console to see dump for <color=green>btnRect</color> (copy to cliboard and paste in code):
            RectTransform btnRect = foldButton.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(1f, 0.5f);
            btnRect.anchorMax = new Vector2(1f, 0.5f);
            btnRect.anchoredPosition = new Vector2(-3.109955f, 0f);
            btnRect.sizeDelta = new Vector2(10f, 10f);
            btnRect.pivot = new Vector2(1f, 0.5f);

            button.GetComponent<Image>().enabled = false;
            var text = btnRect.gameObject.AddTextChild(LayoutFoldController.labelUnfolded);
            text.alignment = TextAnchor.MiddleCenter;
            var textRect = text.GetComponent<RectTransform>(); ;
            textRect.offsetMax = Vector2.zero;
            textRect.offsetMin = Vector2.zero;
            text.color = textColor;
            textRect.sizeDelta = new Vector2(14, 14);
            text.name = "FoldIndicatorText";
            text.font = font;
            return button;
        }
    }
#if UNITY_EDITOR

    public class LayoutCreatorMenuAdder
    {

        [MenuItem("GameObject/UI/Add LayoutPanel Item")]
        static RectTransform CreaatePanelChildLayout()
        {
            if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<RectTransform>() == null)
            {
                Debug.Log("Please select a RectTransform first");
                return null;
            }
            RectTransform rect = CreaatePanelChild();
            if (rect != null)
                Selection.activeGameObject = rect.gameObject;
            Undo.RegisterCreatedObjectUndo(rect.gameObject, "Create object");
            rect.gameObject.AddComponent<LayoutItemCreator>().AddBordersAndSampleContent();
            rect.gameObject.AddComponent<LayoutItemCreator>().RemoveMe();
            return rect;
        }

        static RectTransform CreaatePanelChild()
        {
            GameObject go = new GameObject("Panel");
            if (Selection.activeGameObject != null) go.transform.SetParent(Selection.activeGameObject.transform);
            //  else
            //    go.transform.SetParent(CreateCanvasIfNotPresent());
            go.transform.localPosition = Vector2.zero;
            go.transform.localScale = Vector3.one;
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            Image image = go.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.2f);
            image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
            image.type = Image.Type.Sliced;
            rect.localScale = Vector3.one;
            //  Selection.activeGameObject = go;

            Undo.RegisterCreatedObjectUndo(go, "Create object");
            return rect;
        }
    }
#endif


}