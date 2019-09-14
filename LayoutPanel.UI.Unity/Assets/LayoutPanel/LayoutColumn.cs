using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class LayoutColumn : MonoBehaviour
    {
        public DrawInspectorBg draw;
        public LayoutElement spacer;


        void OnValidate()
        {


            if (spacer != null) spacer.name = LayoutPanel.spacerName;
            Rescan();
        }
        void Reset()
        {
            Rescan();
            if (spacer==null) 
           spacer= LayoutCreator.CreateSpacer(gameObject);
        }
        public void NotifyOfChange(LayoutPanel source)
        {
            if (Application.isPlaying)
                StartCoroutine(RescanNextFrame());
            else
                Rescan();

        }
        IEnumerator RescanNextFrame()
        {
            yield return null;
            Rescan();
        }
        void Start()
        {
            Rescan();
        }
        void FindSpacer()
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
        [ExposeMethodInEditor]
        void Rescan()
        {
            VerticalLayoutGroup verticalLayoutGroup = gameObject.AddOrGetComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.SetChildControl();
            verticalLayoutGroup.spacing = LayoutBorderDragger.borderSize * 3;
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
            else Debug.Log("nospacer");
            gameObject.AddOrGetComponent<VerticalLayoutGroup>().SetChildControl();
            gameObject.AddOrGetComponent<VerticalLayoutGroup>().spacing = LayoutBorderDragger.borderSize * 3;
        }
    }
}
