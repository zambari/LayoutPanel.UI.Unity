//#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LayoutPanelDependencies;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;
using Z;
namespace Z.LayoutPanel
{


    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class LayoutItemCreator : MonoBehaviourWithBg
    {

        [HideInInspector] [SerializeField] LayoutTopControl topControl;
        [HideInInspector] [SerializeField] LayoutPanel panel;
        [HideInInspector] [SerializeField] LayoutFoldController foldController;
        [HideInInspector] [SerializeField] Button foldButton;
        [HideInInspector] [SerializeField] LayoutElement content;
        [HideInInspector] [SerializeField] LayoutBorderHide layoutBorderHide;

        public LayoutBorderHide.BorderHideMode borderHideMode;
        [Header("If present will text raycast catcher")]
        [SerializeField] float borderOverScan = 0;
        Color textColor = Color.white * 0.8f;
        LayoutCreatorAdvanced layoutCreator;
        public static Font font { get { return Resources.GetBuiltinResource<Font>("Arial.ttf"); } }
        public bool bordersPlacedInside = true;
        public bool removeMeWhenDone = false;
        public bool removeMeNow = false;

        [SerializeField]
        [HideInInspector]
        LayoutNameHelper nameHelper;
        [Range(2, 5)]
        public int subdivideCount = 2;
        void OnValidate()
        {
            TryToGetReferences();
            if (removeMeNow)
                RemoveMe();
        }

        void TryToGetReferences()
        {
            if (topControl == null) topControl = GetComponentInChildren<LayoutTopControl>();
            if (panel == null) panel = gameObject.GetComponent<LayoutPanel>();
            if (foldController == null) foldController = gameObject.GetComponent<LayoutFoldController>();
            if (foldController != null) foldButton = foldController.foldButton;
            if (layoutBorderHide == null) layoutBorderHide = GetComponent<LayoutBorderHide>();
        }
        void Reset()
        {

            // Debug.Log("reset");

            var allcoponents = gameObject.GetComponents<Component>();
            int thispos = 0;
            for (int i = 0; i < allcoponents.Length; i++)
            {
                if (allcoponents[i] == this) thispos = i;
            }
            this.MoveComponent(-thispos);

            RandomizeColor();
            subdivideCount = 2 + Random.Range(0, 2);//';//' + Random.value > 0.8f ? 1 : 0;
            UpdateNameHelper();
        }


        void OnEnable()
        {

            UpdateNameHelper();
            // NameTagHandling(showDepth);
        }

        void RandomizeColor()
        {
            Image image = GetComponent<Image>();
            if (image != null && image.color == Color.white)
            {
                Undo.RegisterCompleteObjectUndo(image, "color");
                Color color = (Color.black + new Color(0, Random.value * .2f, 0) + Color.white * 0.4f);// Color.white * 0.2f).Randomize(1,1,.3f);
                color.a = 0.7f;
                image.color = color.Randomize(2, .4f, .3f);
            }
        }
        void Awake()
        {
            layoutCreator = GetComponentInParent<LayoutCreatorAdvanced>();
            if (layoutCreator != null)
            {
                textColor = layoutCreator.textColor;
            }
        }
        public void AddTop()
        {
            topControl = GetComponentInChildren<LayoutTopControl>();
            if (topControl == null)
            {
                var topImage = gameObject.AddImageChild();
                if (layoutBorderHide != null) topImage.color = layoutBorderHide.borderColor.Randomize();
                topControl = topImage.gameObject.AddOrGetComponent<LayoutTopControl>();
                if (layoutCreator != null)
                {
                    topImage.color = layoutCreator.topColor;
                }
#if UNITY_EDITOR
                Undo.RegisterCreatedObjectUndo(topControl.gameObject, "top");
#endif
            }
            topControl.transform.SetAsFirstSibling();
        }

        void UpdateNameHelper()
        {
            if (nameHelper == null) nameHelper = gameObject.AddOrGetComponent<LayoutNameHelper>();
            nameHelper.UpdateName();
        }
        [ExposeMethodInEditor]
        public void SubDivdeLayout()
        {
            _SubDivdeLayout(subdivideCount);
        }
        public List<LayoutItemCreator> _SubDivdeLayout(int count)
        {
            Image image = GetComponent<Image>();
            if (image != null) image.enabled = false;
            var list = new List<LayoutItemCreator>();

            var n = gameObject.AddOrGetComponent<LayoutNameHelper>();
            var hg = transform.parent.GetComponent<HorizontalLayoutGroup>();
            var vg = transform.parent.GetComponent<VerticalLayoutGroup>();
            bool horizontal = vg != null;
            var subs = gameObject.SplitToLayout(horizontal, count);
            foreach (var g in subs)
            {
                list.Add(g.AddComponent<LayoutItemCreator>());
            }

#if UNITY_EDITOR


            Selection.objects = subs.ToArray();
            EditorApplication.delayCall += () => { if (nameHelper != null) nameHelper.UpdateName(); };
#else
            UpdateNameHelper();
#endif

            RemoveMe();
            return list;
        }

        [ExposeMethodInEditor]
        public void AddBordersOnly()
        {
            layoutBorderHide = gameObject.AddOrGetComponent<LayoutBorderHide>();

            layoutBorderHide.borderColor = layoutBorderHide.borderColor.Randomize(01f, 1f, 0.2f);

            layoutBorderHide.borderHideMode = borderHideMode;

            // borders
            int count = System.Enum.GetNames(typeof(Side)).Length;
            for (int i = count - 1; i >= 0; i--)
            {
                var thisChild = gameObject.AddImageChild();
                thisChild.transform.SetAsFirstSibling();
                var d = thisChild.gameObject.AddOrGetComponent<LayoutBorderDragger>();
                d.bordersPlacedInside = bordersPlacedInside;
                if (borderOverScan > 0)
                    HandleTextRaycastCatchers(thisChild.gameObject);

                d.side = (Side)i;
#if UNITY_EDITOR
                Undo.RegisterCreatedObjectUndo(thisChild.gameObject, "borders");
#endif
            }
            UpdateNameHelper(); ;

        }

        public bool addRandomTexstOnCreate = false;
        [ExposeMethodInEditor]
        public void ConvertToLayoutPanel()
        {
            if (name.Contains(LayoutPanel.spacerName))
                name = "Item " + LayoutExt.RandomString(4);
            AddBordersOnly();
            AddTop();
            HandleContent();
            HandleThisObject();
            HandleTopLabel();
            var rect = GetComponent<RectTransform>();
            Fold(rect);
            TryToGetReferences();
            if (addRandomTexstOnCreate)
            {
                AddRandomTexts();
                var content = transform.Find("CONTENT");
                if (content != null)
                {
                    Debug.Log("destroying content");
                    DestroyImmediate(content.gameObject);
                }
            }
            UpdateNameHelper();

        }
        static int randCount;
        [ExposeMethodInEditor]
        void AddRandomTexts()
        {
            for (int i = 0; i < Random.Range(3, 9); i++)
            {
                var t = gameObject.AddTextChild();
                t.gameObject.name = "RND " + randCount;
                randCount++;
                string k = "";
                for (int j = 0; j < Random.Range(3, 6); j++) k += zExt.RandomString(Random.Range(3, 8)) + " ";
                t.text = k;
                t.fontSize = Random.Range(10, 30);
                t.color = new Color(Random.value, Random.value, Random.value);
            }

#if UNITY_EDITOR
            if (Selection.activeGameObject == gameObject)
                EditorGUIUtility.PingObject(content);
            //Selection.activeObject = content;
            RemoveMe();
#endif
        }




        public void RemoveMe()
        {
            if (!removeMeWhenDone) return;
#if UNITY_EDITOR
            EditorApplication.delayCall += () =>
            {
                if (this != null)
                {
                    if (nameHelper != null)
                    {
                        EditorApplication.delayCall += () => { if (nameHelper != null) nameHelper.UpdateName(); };
                    }
                    Undo.DestroyObjectImmediate(this);

                }
            };
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
            var button = foldButton.AddOrGetComponent<Button>();

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
#if UNITY_EDITOR

        [ExposeMethodInEditor]
        void RemoveBorders()
        {
            var panel = GetComponent<LayoutPanel>();
            if (panel != null)
            {
                if (panel.resizableElement != null)
                    Undo.DestroyObjectImmediate(panel.resizableElement.gameObject);
                Undo.DestroyObjectImmediate(panel);
            }
            var bh = GetComponent<LayoutBorderHide>();

            if (bh != null)
            {
                bh.borderHideMode = LayoutBorderHide.BorderHideMode.Visible;
                Undo.DestroyObjectImmediate(bh);
            }
            for (int i = transform.childCount - 1; i >= 0; i--)
            {

                var thisChild = transform.GetChild(i);
                if (thisChild.GetComponent<LayoutTopControl>() != null || thisChild.GetComponent<LayoutBorderDragger>() != null)
                {
                    Undo.DestroyObjectImmediate(thisChild.gameObject);
                }
            }

        }

        void HandleTextRaycastCatchers(GameObject a)
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
        void HandleTopLabel()
        {
            Text labelText = null;//panel.labelText; ;//= topControl.GetComponentInChildren<Text>();
            if (labelText == null)
            {
                //                Debug.Log("creating text", gameObject);
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
            panel.labelText = labelText;
        }
        void HandleThisObject()
        {
            RectTransform rect = gameObject.AddOrGetComponent<RectTransform>();
            float w = rect.rect.width;
            float h = rect.rect.height;
            rect.anchorMin = Vector2.one / 2;
            rect.anchorMax = Vector2.one / 2;
            rect.sizeDelta = new Vector2(h, w);
            var fold = gameObject.AddOrGetComponent<LayoutFoldController>();
            if (fold.foldButton == null)
            {
                foldButton = CreateFoldButton();
                foldButton.name = "FoldButton";
                fold.foldButton = foldButton;

            }
            else
                foldButton = fold.foldButton;
            var foldStatusText = fold.foldButton.GetComponentInChildren<Text>();
            foldStatusText.name = "FoldStatusText";
            fold.foldLabelText = foldStatusText;
            Fold(fold);
            foldStatusText.text = fold.GetFoldString();
            VerticalLayoutGroup group = gameObject.AddOrGetComponent<VerticalLayoutGroup>();
            group.SetChildControl(2);
            Fold(group);
            panel = gameObject.AddOrGetComponent<LayoutPanel>();
            if (panel.resizableElement == null)
                panel.resizableElement = content;
        }
        void HandleContent()
        {
            content = gameObject.AddImageChild().AddOrGetComponent<LayoutElement>();
#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(content.gameObject, "content");
#endif

            content.minHeight = LayoutSettings.topHeight;
            content.minWidth = 26;
            content.flexibleWidth = 0.001f;
            content.GetComponent<Image>().enabled = false;
            content.name = "CONTENT";
            Fold(content);
        }
#endif

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
            rect.gameObject.AddComponent<LayoutItemCreator>().ConvertToLayoutPanel();
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


} //namespace