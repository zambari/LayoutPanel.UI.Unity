using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LayoutPanelDependencies;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Z.LayoutPanel
{
    [RequireComponent(typeof(LayoutNameHelper))]
    [DisallowMultipleComponent]
    public class LayoutCreatorAdvanced : MonoBehaviourWithBg
    {

        public Color startColor = new Color(0, 0.408f, 0.945f, 0.559f);
        public Color endColor = new Color(0.000f, 0.851f, 0.302f, 0.578f);
        [HideInInspector] public Color textColor = Color.white * 0.8f;
        [HideInInspector] public Color topColor = Color.white * 0.4f;

        public bool startVertical;
        public int startWithsteps = 3;

        [Header("Visible borders")]
        public bool hideCreatedBordersInHierarchy = true;
        public bool bordersPlacedInside = false;
        // float flexHeight = 0.2f;
        // [Header("something,something\n\nehruie")]
        // public bool tagObjectNamesWithDepth;
        [SerializeField] [HideInInspector] bool lastTag;
        LayoutNameHelper nameHelper;
        int contName;
        int splitCount = 3;

        void OnValidate()
        {
            if (startWithsteps < 1) startWithsteps = 1;
            if (startWithsteps > 4) startWithsteps = 4;
            // if (lastTag != tagObjectNamesWithDepth)
            // {
            //     lastTag = tagObjectNamesWithDepth;
            //     var itemcreators = GetComponentsInChildren<LayoutNameHelper>();
            //     foreach (var i in itemcreators)
            //         i.NameTagHandling(tagObjectNamesWithDepth);
            // }
        }

        void AddGroupComponents(Transform src, bool horizontal)
        {
            if (horizontal)
            {
                var hg = src.gameObject.AddOrGetComponent<HorizontalLayoutGroup>();
                hg.SetParams(null);

            }
            else
            {
                var vg = src.gameObject.AddOrGetComponent<VerticalLayoutGroup>();
                vg.SetParams(null);

            }
        }
        List<Image> createdImages;
        List<Transform> FillWithPanels(Transform src, int count, Vector2 preferredSize)
        {
            List<Transform> list = new List<Transform>();
            for (int i = 0; i < count; i++)
            {
                GameObject newChild = new GameObject("item " + contName, typeof(Image));
                createdImages.Add(newChild.GetComponent<Image>());
                AddLayoutElement(newChild, preferredSize);
                newChild.transform.SetParent(src);
                list.Add(newChild.transform);
                contName++;
            }
            return list;
        }
        void AddLayoutElement(GameObject target, Vector2 preffered)
        {
            var le = target.AddOrGetComponent<LayoutElement>();
            if (preffered.x > 0)
                le.preferredWidth = preffered.x;
            else
            {
                le.flexibleWidth = 1;
            }
            if (preffered.y > 0)
            {
                le.preferredHeight = preffered.y;
            }
            else
                le.flexibleHeight = 1;
            le.minHeight = LayoutSettings.topHeight;
            le.minWidth = 3 * LayoutSettings.topHeight;
        }
        void AddPanelBorders(Transform src, bool fullPanel)
        {
            var creator = src.gameObject.AddComponent<LayoutItemCreator>();
            if (fullPanel) creator.ConvertToLayoutPanel();
            else creator.AddBordersOnly();
            if (creator != null) DestroyImmediate(creator);

        }
        public void CreateSetupNewExperimentalPart1()
        {
            if (GetComponentInParent<Canvas>() == null)
            {
                Debug.Log("Please add to a panel");
                return;
            }

            bool currentDirHorizontal = !startVertical;
            int thisStepCount = startWithsteps;
            RectTransform thisRect = Prepare();

            Vector2 prefferedSize = new Vector2(thisRect.rect.width, thisRect.rect.height);
            Debug.Log("current preffered " + prefferedSize);
            // AddGroupComponents(thisRect, currentDirHorizontal);

            //    prefferedSize /= thisStepCount;
            var thisLevel = new List<Transform>();
            thisLevel.Add(thisRect.transform);
            int leveel = 0;
            while (thisStepCount >= 0 && leveel < 4)
            {
                leveel++;
                prefferedSize /= thisStepCount;
                Debug.Log("level is " + leveel + " current preffered " + prefferedSize + " created =" + createdImages.Count);
                currentDirHorizontal = !currentDirHorizontal;
                thisLevel = HandleStep(thisLevel, currentDirHorizontal, thisStepCount, prefferedSize);
                thisStepCount--;
            }

            for (int i = 0; i < createdImages.Count; i++)
            {

                createdImages[i].color = Color.Lerp(startColor, endColor, 1f * i / createdImages.Count);
                LayoutItemCreator creator = createdImages[i].AddOrGetComponent<LayoutItemCreator>();
            }

        }
        public void CreateSetupNewExperimentalPart2()
        {
            Debug.Log("we have " + createdImages.Count + "images ");
            for (int i = 0; i < createdImages.Count; i++)
            {

            }
        }
        List<Transform> HandleStep(List<Transform> transforms, bool stephorizontal, int thisStepCount, Vector2 prefferedSize)
        {
            List<Transform> list = new List<Transform>();
            if (stephorizontal) prefferedSize.x = -1; else prefferedSize.y = -1;
            for (int i = 0; i < transforms.Count; i++)
            {
                AddGroupComponents(transforms[i], stephorizontal);
                var panels = FillWithPanels(transforms[i], thisStepCount, prefferedSize);
                list.AddRange(panels);
            }
            return list;
        }

        RectTransform Prepare()
        {
            createdImages = new List<Image>();
            contName = 1;
            var image = GetComponent<Image>();
            if (image != null) image.enabled = false;

            var hg = GetComponent<HorizontalLayoutGroup>();
            var vg = GetComponent<VerticalLayoutGroup>();
            if (hg != null) DestroyImmediate(hg);
            if (vg != null) DestroyImmediate(vg);

            var thisRect = GetComponent<RectTransform>().AddImageChild(0.8f).GetComponent<RectTransform>();
            thisRect.anchorMax = Vector2.one;
            thisRect.anchorMin = Vector2.zero;

            while (thisRect.rect.width < 100) thisRect.sizeDelta = thisRect.sizeDelta + new Vector2(10, 0);
            while (thisRect.rect.height < 100) thisRect.sizeDelta = thisRect.sizeDelta + new Vector2(0, 10);
            return thisRect;
        }

        void ChangeToObject(GameObject obj)
        {
#if UNITY_EDITOR
            Debug.Log("chaned");
            DestroyImmediate(this);
            Selection.activeGameObject = obj;
            obj.AddComponent<LayoutCreator>();
#endif
        }

        void Reset()
        {


#if UNITY_EDITOR
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.Log("Please add me to a panel to your canvas (start with UI image/panel), currently no canvas is found in parent");
                DestroyImmediate(this);
                return;

            }
            if (canvas.transform == transform)
            {
                var newObj = canvas.GetComponent<RectTransform>().AddImageChild();
                EditorApplication.delayCall += () => ChangeToObject(newObj.gameObject);
                return;
            }
#endif
            Image image = GetComponent<Image>();
            if (image.color == Color.white)
            {
                Undo.RegisterCompleteObjectUndo(image, "color");
                image.color = (Color.black + Color.white * 0.2f).Randomize();
            }
            RectTransform rect = GetComponent<RectTransform>();
            if (rect.rect.width == 100 && rect.rect.height == 100) rect.sizeDelta = new Vector2(300, 400);
        }
        void UpdateBorders()
        {
            var borders = gameObject.GetComponentsInChildren<LayoutBorderDragger>();
            foreach (var b in borders) b.side = b.side;
        }
        void AddRandomChildren(GameObject parent, int count, ref List<LayoutPanel> panels)
        {
            for (int j = 0; j < count; j++)
            {
                var panel = CreatePanel(parent, "Item [" + (panels.Count + 1) + " ] " + zExtensionPrimitives.RandomString(3));
                //                panel.detachedMode = false;
                panels.Add(panel);
            }

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
        public static LayoutElement CreateSpacer(GameObject thisChild, float flexWidth = 0.2f, float flexHeight = 0.2f)
        {
            var img = thisChild.AddChildRectTransform();
            var le = img.gameObject.AddComponent<LayoutElement>();
            le.flexibleHeight = flexHeight;
            le.flexibleWidth = flexWidth;
            le.name = LayoutPanel.spacerName;
            return le;
        }
        void UpdateNames()
        {
            var nameH = GetComponentsInChildren<LayoutNameHelper>();
            foreach (var h in nameH)
                h.UpdateName();
        }
        LayoutPanel CreatePanel(GameObject parent, string newName = "NoName")
        {

            var thisGrandChild = parent.AddImageChild();
            thisGrandChild.name = newName;

            var creator = thisGrandChild.gameObject.AddComponent<LayoutItemCreator>();
            creator.bordersPlacedInside = bordersPlacedInside;
            creator.borderHideMode = hideCreatedBordersInHierarchy ? LayoutBorderHide.BorderHideMode.Hidden : LayoutBorderHide.BorderHideMode.Visible;
            creator.ConvertToLayoutPanel();

            var panel = creator.GetComponent<LayoutPanel>();
            var contentle = panel.resizableElement;
            contentle.preferredHeight = Random.Range(LayoutSettings.topHeight, LayoutSettings.topHeight * 5);
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
            e.side = Side.Bottom;
            d.side = Side.Top;
        }

        [ExposeMethodInEditor]
        public void CreateHorizontalLayoutSlpit()
        {
            CreateHorizontalLayout();
        }
        public List<LayoutItemCreator> CreateHorizontalLayout(int thisSplitCount = -1)
        {
            if (thisSplitCount == -1) thisSplitCount = splitCount;
            var vg = GetComponent<VerticalLayoutGroup>();
            if (vg != null) DestroyImmediate(vg);
            var list = AddItemCreatorsToChildren(GetComponent<RectTransform>().SplitToLayout(true, thisSplitCount));
            AmendName();
            return list;
        }
        [ExposeMethodInEditor]
        public void CreateVerticalLayoutSlpit()
        {
            CreateVerticalLayout();
        }
        public List<LayoutItemCreator> CreateVerticalLayout(int thisSplitCount = -1)
        {
            // var col = gameObject.GetComponent<LayoutRow>();
            // if (col != null) DestroyImmediate(col);
            if (thisSplitCount == -1) thisSplitCount = splitCount;
            var hg = GetComponent<HorizontalLayoutGroup>();
            if (hg != null) DestroyImmediate(hg);
            var list = AddItemCreatorsToChildren(GetComponent<RectTransform>().SplitToLayout(false, thisSplitCount));
            // gameObject.AddComponent<LayoutColumn>();
            AmendName();
            return list;
        }
        void AmendName()
        {
            if (nameHelper == null) nameHelper = gameObject.AddOrGetComponent<LayoutNameHelper>();
            nameHelper.UpdateName();
        }

        List<LayoutItemCreator> AddItemCreatorsToChildren(List<GameObject> target)
        {
            var list = new List<LayoutItemCreator>();
            for (int i = 0; i < target.Count; i++)
            {
                var thisChild = target[i];
                list.Add(thisChild.AddOrGetComponent<LayoutItemCreator>());

            }
#if UNITY_EDITOR
            UnityEditor.Selection.objects = target.ToArray();
#endif
            return list;
        }

        [ExposeMethodInEditor]
        void RemoveAllCreators()
        {
            var cr = GetComponentsInChildren<LayoutItemCreator>();
            foreach (var c in cr) Undo.DestroyObjectImmediate(c.gameObject);
            EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(this);
        }

        [ExposeMethodInEditor]
        public void RemoveAllChildren()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
#if UNITY_EDITOR
                Undo.DestroyObjectImmediate(transform.GetChild(i).gameObject);
#else
            DestroyImmediate (transform.GetChild (i).gameObject);
#endif
        }
#if UNITY_EDITOR
        public bool onlyCreateBorders = false;
        public bool checkForChildren = true;
        bool CheckCreateCondition(LayoutItemCreator c)
        {
            if (checkForChildren)
                return c.transform.childCount == 0;
            return
            c.transform.parent.GetComponent<LayoutItemCreator>() == null;
        }
        [ExposeMethodInEditor]
        public void LaunchItemCreators()
        {
            var itemcr = GetComponentsInChildren<LayoutItemCreator>();
            int processed = 0;
            int notprocssed = 0;
            foreach (var i in itemcr)
            {
                if (CheckCreateCondition(i))
                {
                    if (onlyCreateBorders)
                    {
                        i.AddBordersOnly();
                    }
                    else
                    {
                        i.ConvertToLayoutPanel();
                    }
                    processed++;
                }
                else
                    notprocssed++;
            }
            // Debug.Log("Processed " + processed + " not processed " + notprocssed);
        }
        [ExposeMethodInEditor]
        public void SelectAllCreators()
        {
            List<GameObject> games = new List<GameObject>();
            var itemcr = GetComponentsInChildren<LayoutItemCreator>();
            foreach (var i in itemcr) games.Add(i.gameObject);
            Selection.objects = itemcr;
        }
#endif
       
    }
}