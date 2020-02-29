using System.Collections;
using System.Collections.Generic;
using LayoutPanelDependencies;
using UnityEngine;
using UnityEngine.UI;
using Z;
using zUI;
namespace Z.LayoutPanel
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class LayoutNameHelper : MonoBehaviourWithBg
    {
        public LayoutPanel layoutPanel;
        // public LayoutItemCreator itemCrator;
        // LayoutCreator layoutCreator;
        public int myDepth;
        public bool parentHasHorizontal;
        public bool parentHasVertical;
        public bool IHaveHorizontal;
        public bool IHaveVertical;
        [ReadOnly] [SerializeField] string statusString;
        [SerializeField] bool showDepth = true;
        LayoutNameHelperSettings settings = new LayoutNameHelperSettings();
        void Awake()
        {
            UpdateName();
        }
        void OnValidate()
        {
            //   if (Time.time > 1)
            UpdateName();
        }
        public void UpdateNameF()
        {
            lastUpdateFrame = -1;
            var oldpanel = layoutPanel;
            layoutPanel = GetComponent<LayoutPanel>();
            if (layoutPanel != oldpanel)
            {
                Debug.Log("it was worth the effor");
            }
            UpdateName();
        }
        int lastUpdateFrame = -1;
        public void UpdateName()
        {
            if (Time.frameCount == lastUpdateFrame)
            {
                //            Debug.Log("returning " + lastUpdateFrame, gameObject);
                return;
            }
            lastUpdateFrame = Time.frameCount;

            NameTagHandling();
            // if (layoutPanel == null) layoutPanel = GetComponent<LayoutPanel>();
            // parentHasHorizontal = transform.parent.GetComponent<HorizontalLayoutGroup>();
            // parentHasVertical = transform.parent.GetComponent<VerticalLayoutGroup>();
            // if (!parentHasHorizontal && !parentHasHorizontal)
            // {
            //     statusString = "No parent layout :(";
            // }
            // else
            // {
            //     if (parentHasHorizontal) statusString = " Parent is Horizontal ";
            //     if (parentHasVertical) statusString = " Parent is vertical ";
            // }
            // if (layoutPanel != null)
            // {
            //     UpdaeFromPanel(layoutPanel);
            // }
            // else
            // {


            // }

        }

        public void NameTagHandling()
        {
            // showDepth = settings.tagObjectNamesWithDepth;
            string n = name;
            //if (show)
            string depthString = "";

            myDepth = GetDepth();
            if (myDepth == 1)
                depthString += "";//"╂ ";
            if (layoutPanel == null)
                layoutPanel = GetComponent<LayoutPanel>();
            if (layoutPanel == null)
            {
                if (settings.visualLayoutHelpers)
                {
                    parentHasHorizontal = transform.parent.GetComponent<HorizontalLayoutGroup>();
                    parentHasVertical = transform.parent.GetComponent<VerticalLayoutGroup>();
                    if (parentHasHorizontal) depthString += "╥─";//"║ ";
                    else // ⒣
                    if (parentHasVertical) depthString += "╞─";//"▤< ";//═ ";
                    else
                    {
                        depthString += "▩";
                    }
                }
            }
            else //  ⒱
            {
                depthString += "▣ ▞"; //▩ 
                layoutPanel.CheckGroups();
                parentHasHorizontal = transform.parent.GetComponent<HorizontalLayoutGroup>();
                if (parentHasVertical) parentHasVertical = false;
                parentHasVertical = transform.parent.GetComponent<VerticalLayoutGroup>();
                if (parentHasHorizontal != layoutPanel.isInHorizontalGroup)
                {
                    Debug.Log("INCONSISTENCY");
                }
                if (parentHasVertical != layoutPanel.isInVerticalGroup)
                {
                    Debug.Log("INCONSISTENCY");
                }

                //https://en.wikipedia.org/wiki/Geometric_Shapes

                if (layoutPanel.isInHorizontalGroup)
                    depthString += " ╤ ";
                else
                if (layoutPanel.isInVerticalGroup)
                    depthString += " ╟ ";
                else
                    depthString += "  ┇  ";
            }
            //  depthString += "░ ";
            var hg = GetComponent<HorizontalLayoutGroup>();

            IHaveHorizontal = hg != null;

            if (IHaveHorizontal)
            {
                IHaveVertical = false;
                hg.SetParams(layoutPanel);
            }
            else
            {
                var vg = GetComponent<VerticalLayoutGroup>();
                IHaveVertical = vg != null;
                if (vg != null)
                    vg.SetParams(layoutPanel);
            }
            if (settings.showCreators)
            {
                if (GetComponent<LayoutItemCreator>() != null)
                    depthString += " ❆ ";
            }
            statusString = depthString;


            //  else
            //      n = Z.NameUtils.RemovePreTag(name);
            n = Z.NameUtils.SetPreTag(n, depthString);
            string endtag = "";
            if (settings.showLayoutGroupComponents)
            {
                if (IHaveHorizontal) endtag += " ▥ ";
                else
                if (IHaveVertical) endtag += " ▤ ";
                // else
                //     endtag += "□ ";
            }
            if (settings.tagObjectNamesWithDepth)
            {
                endtag += " " + myDepth.ToFunnyNumber();
            }
            n = Z.NameUtils.SetTag(n, endtag);
            name = n;
        }
        void OnDisable()
        {
            gameObject.RemoveAllTags();
        }
        int GetDepth()
        {
            var creator = GetComponentInParent<LayoutCreator>();
            Transform lookFor = null;
            if (creator != null) lookFor = creator.transform;
            int thisDepth = 1;
            Transform thisTransform = transform;
            if (thisTransform == lookFor)
            {
                return 1;
            }
            while (true)
            {
                thisDepth++;
                if (thisTransform.parent == null || thisTransform.parent == lookFor) return thisDepth;
                thisTransform = thisTransform.parent;
            }

        }

        void OnEnable()
        {
            var settingsprovider = GetComponentInParent<IProvideLayoutNameHelperSettings>();
            if (settingsprovider != null) settings = settingsprovider.GetSettings();
            UpdateNameF();
        }


    }

}