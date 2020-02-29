using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LayoutPanelDependencies;
namespace zUI
{
#if not
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    [ExecuteInEditMode]
    public class LayoutRow : LayoutGroupBase
    {
        public override bool isHorizontal { get { return true; } }
        HorizontalLayoutGroup group;


        protected override void UpdateLayout()
        {
            
         if (group == null) group = gameObject.AddOrGetComponent<HorizontalLayoutGroup>();
            group.SetChildControl();
            group.childForceExpandHeight = true;
            // group.spacing = LayoutSettings.groupSpacing;
            group.spacing =  LayoutSettings.groupSpacing;
            var padding = group.padding;
            padding.top = (int)group.spacing;
            padding.bottom = (int)group.spacing;
            group.padding = padding;
        }
    }
#endif
}
