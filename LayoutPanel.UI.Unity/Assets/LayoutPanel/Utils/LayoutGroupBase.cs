using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LayoutPanelDependencies;
namespace zUI
{
    #if not

    [ExecuteInEditMode]
    public abstract class LayoutGroupBase : MonoBehaviourWithBg
    {
           public LayoutElement spacer;
        public virtual bool isHorizontal { get { return false; } }

        void OnEnable()
        {
            LayoutSettings.onBorderSizeChange += UpdateLayout;
            UpdateLayout();
        }
        void OnDisable()
        {
            LayoutSettings.onBorderSizeChange -= UpdateLayout;
        }

        protected virtual void OnValidate()
        {
            if (spacer != null) spacer.name = LayoutPanel.spacerName;
            UpdateLayout();
            HandleSpacer();
        }
        void Reset()
        {
            HandleSpacer();
            UpdateLayout();
            if (spacer == null)
                spacer = LayoutCreator.CreateSpacer(gameObject);
            Image image = GetComponent<Image>();
            if (image != null) image.enabled = false;
        }
        public void NotifyOfChange(LayoutPanel source)
        {
            if (Application.isPlaying)
                StartCoroutine(RescanNextFrame());
            else
                UpdateLayout();

        }
        IEnumerator RescanNextFrame()
        {
            yield return null;
            // Rescan();
            HandleSpacer();
        }
        void Start()
        {
            // Rescan();
            HandleSpacer();
            UpdateLayout();
        }
        protected void FindSpacer()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var childLayoutElement = transform.GetChild(i).GetComponent<LayoutElement>();
                if (childLayoutElement != null && childLayoutElement.GetComponent<LayoutPanel>() == null)
                    if ((childLayoutElement.name.Contains(LayoutPanel.spacerName)) ||
                     childLayoutElement.flexibleHeight > 0)
                        spacer = childLayoutElement;
            }
        }
        protected virtual void UpdateLayout()
        {

        }
        protected void HandleSpacer()
        {
            return;
            int spacerIndex = -1;
            if (spacer == null)
                FindSpacer();
            for (int i = 0; i < transform.childCount; i++)
            {
                LayoutElement element = transform.GetChild(i).GetComponent<LayoutElement>();
                if (element != null)
                {
                    if (element.flexibleWidth > 0.1f) element.flexibleWidth = 0.0144f;
                    if (element.flexibleHeight > 0.1f) element.flexibleHeight = 0.0044f;
                }
            }

            if (spacer != null)
            {
                spacerIndex = spacer.transform.GetSiblingIndex();

                for (int i = 0; i < transform.childCount; i++)
                {
                    var thisChild = transform.GetChild(i).gameObject;
                    var thisPanel = thisChild.GetComponent<LayoutPanel>();
                    if (thisPanel != null)
                    {
                        thisPanel.hasSiblingTop = i > 0 && transform.GetChild(i - 1).GetComponent<LayoutPanel>() != null;
                        thisPanel.hasSiblingBottom = i < transform.childCount - 1 && transform.GetChild(i + 1).GetComponent<LayoutPanel>() != null;
                        thisPanel.isAlignedBottom = thisPanel.transform.GetSiblingIndex() > spacerIndex;

                    }
                }
            }
        }

        // protected virtual void Rescan()
        // {
        // }
    
    }
    #endif
}
