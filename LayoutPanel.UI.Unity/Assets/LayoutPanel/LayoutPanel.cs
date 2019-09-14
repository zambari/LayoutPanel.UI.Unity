using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace zUI
{
    // [RequireComponent(typeof(LayoutElement))]
    [System.Serializable]
    [SelectionBase]
    public class DrawInspectorBg { }
    public class LayoutPanel : MonoBehaviour
    {
        public DrawInspectorBg draw;
        [HideInInspector] public bool hasSiblingTop;
        [HideInInspector] public bool hasSiblingBottom;
        [HideInInspector] public bool isAlignedBottom;
        public static string spacerName { get { return "---"; } }
        public Text labelText;
        public LayoutElement resizableElement;
        public string panelName
        {
            get { return _panelName; }
            set
            {
                if (labelText == null)
                {
                    var top = GetComponentInChildren<LayoutTopControl>();
                    if (top != null)
                    {
                        labelText = top.GetComponentInChildren<Text>();
                        labelText.SetText(value);
                        if (labelText == null) Debug.Log("no label", gameObject);
                    }
                    else Debug.Log("no top control?", gameObject);
                }
                if (labelText != null)

                    labelText.SetText(value);
                _panelName = value;
            }
        }
        [SerializeField] string _panelName;
        public void PlaceDropTarget(Transform target, int newSiblingIndex = -1)
        {
            transform.SetParent(target);
            if (newSiblingIndex != -1)
                transform.SetSiblingIndex(newSiblingIndex);
            else transform.SetAsLastSibling();
        }
        void Start()
        {
            OnValidate();
        }
        void OnValidate()
        {
            if (string.IsNullOrEmpty(_panelName)) _panelName = name;
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
        }
        void SetGroupPadding()
        {
            var group = gameObject.AddOrGetComponent<VerticalLayoutGroup>();
            if (group != null)
            {
                var padding = group.padding;
                if (padding.top < LayoutTopControl.topHeight)
                {
                    padding.top = LayoutTopControl.topHeight;
                    group.padding = padding;
                }
            }

        }
        public Transform GetTargetTransformForSide(LayoutBorderDragger.Side side)
        {
            if (transform.parent == null) return transform;
            return transform.parent;
        }
        public LayoutElement GetTargetElementForSide(LayoutBorderDragger.Side side)
        {
            // if (side == LayoutBorderDragger.Side.Left || side == LayoutBorderDragger.Side.Right)
            //     return transform.parent.GetComponent<LayoutElement>();
            // else
            return resizableElement;
        }
        
        void Reset()
        {
            string _panelName = name;
        }

        void OnTransformParentChanged()
        {
            var c = transform.GetComponentInParent<LayoutColumn>();
            if (c != null)
                c.NotifyOfChange(this);
        }
    }
}