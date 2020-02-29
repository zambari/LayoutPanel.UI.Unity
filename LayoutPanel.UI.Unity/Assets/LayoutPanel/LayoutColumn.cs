using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LayoutPanelDependencies;
namespace zUI
{
    #if not
    [RequireComponent(typeof(VerticalLayoutGroup))]
    [ExecuteInEditMode]
    public class LayoutColumn : LayoutGroupBase
    {
        VerticalLayoutGroup group;

        protected override void UpdateLayout()
        {
            if (group == null) group = gameObject.AddOrGetComponent<VerticalLayoutGroup>();
            group.SetChildControl();
            group.spacing = LayoutSettings.groupSpacing;
        }
    }
    #endif
}
