using System.Collections;
using System.Collections.Generic;
using LayoutPanelDependencies;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class LayoutHorizontalController : MonoBehaviour
    {
        public DrawInspectorBg draw;
        HorizontalLayoutGroup group;
        void OnEnable()
        {
            LayoutPanel.onBorderSizeChange += UpdateBorder;
        }
        void OnDisable()
        {
            LayoutPanel.onBorderSizeChange -= UpdateBorder;
        }
        void UpdateBorder()
        {
            if (group == null) group = GetComponent<HorizontalLayoutGroup>();
            group.spacing = LayoutPanel.borderSize * 3 + 2 * LayoutPanel.borderSpacing;
            var padding = group.padding;
            padding.top = (int)group.spacing;
            padding.bottom = (int)group.spacing;
            group.padding = padding;
        }
    }
}
