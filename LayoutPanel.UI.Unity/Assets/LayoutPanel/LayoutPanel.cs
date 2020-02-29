using System.Collections;
using System.Collections.Generic;
using LayoutPanelDependencies;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using zUI;
using Z;
// 
namespace Z.LayoutPanel
{

    [SelectionBase]
    [DisallowMultipleComponent]
    public class LayoutPanel : MonoBehaviourWithBg
    {

        // [HideInInspector] public bool hasSiblingTop;
        // [HideInInspector] public bool hasSiblingBottom;
        [HideInInspector] public bool isAlignedBottom;
        public static string spacerName { get { return "---"; } }
        public Text labelText;
        public LayoutElement resizableElement;
        public bool detachedMode { get { return freeMoveMode; } }
        [SerializeField] public bool freeMoveMode;
        [HideInInspector] [SerializeField] public bool isBeingDragged;
        [SerializeField] string _panelName;
        [ReadOnly] public bool isInHorizontalGroup;
        [ReadOnly] public bool isInVerticalGroup;
        public bool useOwnLE = true;
        VerticalLayoutGroup verticalLayout;

        public string panelName
        {
            get { return _panelName; }
            set
            {
                value = value.RemoveTag();
                if (labelText == null)
                {
                    var top = GetComponentInChildren<LayoutTopControl>();
                    if (top != null)
                    {
                        var childtop = top.GetComponentInChildren<Text>();
                        if (childtop.transform.parent.GetComponent<Button>() != null)
                        {
                            labelText = childtop;
                            labelText = top.GetComponentInChildren<Text>();
                            labelText.SetText(value);
                            if (labelText == null) Debug.Log("no label", gameObject);
                        }
                    }
                    else Debug.Log("no top control?", gameObject);
                }
                if (labelText != null)

                    labelText.SetText(value);
                _panelName = value;
                name = value;
            }
        }
        public void PlaceDropTarget(Transform target, int newSiblingIndex = -1)
        {
            if (target == null)
            {
                Debug.Log("no target");
                return;
            }
            if (LayoutDropTarget.currentTarget != null)
            {
                LayoutPanel targetPanel = LayoutDropTarget.dropTargetPanel;
                bool isHorizontalbar = LayoutDropTarget.currentTarget.isHorizontalBar;
                bool isPanelHorizontal = LayoutDropTarget.currentTarget.isPanelHorizontal;
                bool isTargetHoritontal = targetPanel.isInHorizontalGroup;
                if (isTargetHoritontal != isHorizontalbar)
                {

                    var cloned = LayoutEditorUtilities.ClonePanel(targetPanel);
                    if (isTargetHoritontal)
                    {
                        verticalLayout = cloned.AddOrGetComponent<VerticalLayoutGroup>();
                        verticalLayout.SetParams(this);
                    }
                    else
                    {
                        var group = cloned.AddOrGetComponent<HorizontalLayoutGroup>();
                        group.SetParams(this);
                    }
                    target = cloned;
                    targetPanel.transform.SetParent(target);
                    var nh = cloned.gameObject.AddOrGetComponent<LayoutNameHelper>();
                    nh.UpdateNameF();
                    Debug.Log("Layout SPLIT ".MakeRed() + " target =" + target);
                }
                else
                {
                    Debug.Log("LayoutDropTarget.isHorizontal=" + isPanelHorizontal + " isHorizontalbar=" + isHorizontalbar + " isPanelHorizontal= " + isPanelHorizontal + " this panel ishoriz=" + isInHorizontalGroup);

                }
            }
            Transform oldParent = transform.parent;
            transform.SetParent(target);
            if (newSiblingIndex != -1)
                transform.SetSiblingIndex(newSiblingIndex);
            else transform.SetAsLastSibling();

            if (oldParent.childCount == 0)
            {
                Debug.Log("removed orphan panel");
                DestroyImmediate(oldParent.gameObject);
            }


        }
        public void SetGroupSettings(LayoutGroupSettings settings)
        {
            groupSettings=settings;
            ApplyGroupSettings();
        }
        public void PlaceTemporary(Transform target, int newSiblingIndex = -1)
        {

            transform.SetParent(target);
            if (newSiblingIndex != -1)
                transform.SetSiblingIndex(newSiblingIndex);
            else transform.SetAsLastSibling();
        }
        LayoutGroupSettings groupSettings = new LayoutGroupSettings();
        void OnEnable()
        {
            GetGroupSettings();
        }
        void GetGroupSettings()

        {
            var groupProvider = GetComponentInParent<IProvideLayoutGroupSettings>();
            if (groupProvider != null) groupSettings = groupProvider.GetGroupSettings();
        }
        public void ApplyGroupSettings()
        {
            Debug.Log("aplygroupsetings");
            if (groupSettings == null)
            {
                GetGroupSettings();
            }
            if (groupSettings != null)
            {
                if (verticalLayout == null) verticalLayout = GetComponent<VerticalLayoutGroup>();
                if (verticalLayout != null) groupSettings.ApplyTo(verticalLayout);

            }
        }

        public void CheckGroups()
        {

            isInHorizontalGroup = transform.parent.GetComponent<HorizontalLayoutGroup>() != null;
            isInVerticalGroup = transform.parent.GetComponent<VerticalLayoutGroup>() != null;
            if (!isInHorizontalGroup && !isInVerticalGroup)
            {
                //   Debug.Log("Set detachedMode "+name);
                freeMoveMode = true;
            }
            else
            {
                //            Debug.Log("set nonfree");
            }


        }


        void Start()
        {
            // var rt = GetComponent<RectTransform>();
            // float w = rt.rect.width;
            // float h = rt.rect.height;
            // Debug.Log(" w=" + w + " h=" + h);

            OnValidate();
            CheckGroups();


        }

        void OnValidate()
        {
            if (resizableElement == null || useOwnLE)
            {
                resizableElement = gameObject.AddOrGetComponent<LayoutElement>();
                var rt = GetComponent<RectTransform>();
                float w = rt.rect.width;
                float h = rt.rect.height;
                if (w > 0 && h > 0)
                {
                    if (isInHorizontalGroup)
                    {
                        resizableElement.flexibleHeight = .1f;
                        resizableElement.preferredWidth = w;
                    }
                    if (isInVerticalGroup)
                    {
                        resizableElement.flexibleWidth = .1f;
                        resizableElement.preferredHeight = h;

                    }
                    //                Debug.Log("ser preferred  w=" + w + " h=" + h);
                }
            }
            if (resizableElement != null)
            {
                if (resizableElement.minHeight <= 0) resizableElement.minHeight = LayoutSettings.topHeight;
                if (resizableElement.minWidth <= 0) resizableElement.minWidth = LayoutSettings.minWidth;
            }
            // var le = GetComponent<LayoutElement>();
            // if (le != null)
            // {
            //     if (le.preferredHeight == 0 || le.preferredHeight == 0)
            //     {

            //         if (w > 0) le.preferredWidth = w;
            //         if (h > 0) le.preferredHeight = h;
            //         Debug.Log("ser preferred  w=" + w + " h=" + h);
            //     }
            // }


            //      gameObject.AddOrGetComponent<PanelSaverHelper>();
            if (string.IsNullOrEmpty(_panelName)) _panelName = name.RemoveAllTags();
            panelName = _panelName;
            if (resizableElement == null)
            {
                var les = this.GetComponentsInDirectChildren<LayoutElement>();
                for (int i = 0; i < les.Length; i++)
                {
                    if (!les[i].ignoreLayout && les[i].flexibleHeight > 0)
                        resizableElement = les[i];
                }
            }
#if UNITY_EDITOR
            var creator = GetComponent<LayoutItemCreator>();

            if (creator != null) UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(creator); };
#endif
            if (nameHelper == null) nameHelper = gameObject.AddOrGetComponent<LayoutNameHelper>();
            nameHelper.UpdateName();

        }
        [SerializeField]
        [HideInInspector]
        LayoutNameHelper nameHelper;
        // void SetGroupPadding()
        // {
        //     var group = gameObject.AddOrGetComponent<VerticalLayoutGroup>();
        //     if (group != null)
        //     {
        //         var padding = group.padding;
        //         if (padding.top < LayoutSettings.topHeight)
        //         {
        //             padding.top = LayoutSettings.topHeight;
        //             group.padding = padding;
        //         }
        //     }

        // }

        public Transform GetTargetTransformForSide(Side side)
        {
            if (transform.parent == null) return transform;
            return transform.parent;
        }
        public LayoutElement GetTargetElementForSide(Side side)
        {
            if ((side.isHorizontal() && isInVerticalGroup) || (!side.isHorizontal() && isInHorizontalGroup))
            {
                //  Debug.Log("returngn parent", gameObject);
                return transform.parent.GetComponent<LayoutElement>();
            }
            else
                return resizableElement;
        }

        void Reset()
        {
            string _panelName = name;
            gameObject.AddOrGetComponent<Canvas>();
            gameObject.AddOrGetComponent<LayoutElement>();
            gameObject.AddOrGetComponent<GraphicRaycaster>();
        }

        // void OnTransformParentChanged()
        // {

        //     var c = transform.GetComponentInParent<LayoutGroupBase>();
        //     if (c != null)
        //         c.NotifyOfChange(this);
        // }
    }

}