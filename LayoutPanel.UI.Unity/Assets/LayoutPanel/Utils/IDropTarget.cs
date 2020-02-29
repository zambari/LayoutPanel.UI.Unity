using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z.LayoutPanel
{
    public interface IDropTarget
    {
        int targetDropIndex { get; }
        Transform dropTarget { get; }
        Transform transform { get; }
        GameObject gameObject { get; }
        bool isHorizontalBar { get; }
        bool isPanelHorizontal { get; }
        string name { get; }
    }


    public static class LayoutDropTarget
    {
        public static GameObject currentTargetObject;

        public static LayoutPanel dropTargetPanel
        {

            get
            {
                if (currentTargetObject == null) return null;
                return currentTargetObject.GetComponentInParent<LayoutPanel>();
            }
        }
        // public static bool isBarHorizontal
        // {
        //     get
        //     {
        //         if (currentTargetObject == null) return false;
        //         var panel = currentTargetObject.GetComponentInParent<LayoutPanel>();
        //         if (panel == null)
        //         {
        //             Debug.Log("no panel"); return false;
        //         }
        //         var group = panel.transform.parent.GetComponent<LayoutGroupBase>();
        //         if (group == null)
        //         {
        //             Debug.Log("NO LAYOUT GROUP!"); return false;
        //         }
        //         return group.isHorizontal;
        //     }
        // }

        public static IDropTarget currentTarget
        {
            get
            {
                if (currentTargetObject == null) return null;
                else
                    return currentTargetObject.GetComponent<IDropTarget>();
            }
        }
    }
}