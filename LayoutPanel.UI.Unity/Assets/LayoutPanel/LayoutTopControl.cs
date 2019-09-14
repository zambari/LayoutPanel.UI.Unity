using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace zUI
{

      //v.02 canvas scalefactor

    [ExecuteInEditMode]
    public class LayoutTopControl : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
       public DrawInspectorBg draw;

        Canvas canvas;
        RectTransform rectToModify;
        public static LayoutTopControl draggedItem;
        public bool dragEnabled = true;
        int savedSibilingIndex;
        LayoutPanel panel;
        Transform savedParent;
        public static int topHeight { get { return 20; } }
        float dragOpacity = 0.3f;
        bool wasIgnore;
        public void OnBeginDrag(PointerEventData e)
        {
            if (canvas == null) canvas = GetComponentInParent<Canvas>();
            if (!dragEnabled) return;
            if (panel == null) panel=GetComponentInParent<LayoutPanel>();

            savedParent = panel.transform.parent;
            savedSibilingIndex = panel.transform.GetSiblingIndex();
            var ll = panel.GetComponent<LayoutElement>();
            {
                if (ll != null)
                {
                    wasIgnore = ll.ignoreLayout;
                    ll.ignoreLayout = true;
                }
            }
            panel.AddOrGetComponent<CanvasGroup>().blocksRaycasts = false;
            panel.AddOrGetComponent<CanvasGroup>().alpha = dragOpacity;
            draggedItem = this;
            panel.PlaceDropTarget(canvas.transform, -1);
            Cursor.SetCursor(zResourceLoader.panCursor, LayoutBorderDragger.cursorCenter, CursorMode.Auto);
        }

        public void OnDrag(PointerEventData e)
        {
            if (!dragEnabled) return;
            if (rectToModify == null) rectToModify = panel.GetComponent<RectTransform>();
            var currentpos = rectToModify.localPosition;
            currentpos += new Vector3(e.delta.x / canvas.scaleFactor, e.delta.y / canvas.scaleFactor, 0);
            rectToModify.localPosition = currentpos;

        }

        public void OnEndDrag(PointerEventData e)
        {
            if (!dragEnabled) return;
            panel.AddOrGetComponent<CanvasGroup>().blocksRaycasts = true;
            panel.gameObject.AddOrGetComponent<CanvasGroup>().alpha = 1;
            if (LayoutDropTarget.currentTarget != null)
                panel.PlaceDropTarget(LayoutDropTarget.currentTarget.dropTarget, LayoutDropTarget.currentTarget.targetDropIndex);
            else
            {
                panel.PlaceDropTarget(savedParent, savedSibilingIndex);
            }
            var ll = panel.GetComponent<LayoutElement>();
            if (ll != null) ll.ignoreLayout = wasIgnore;
            draggedItem = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        void Start()
        {
            if (panel == null) panel = GetComponentInParent<LayoutPanel>();
            OnValidate();
        }
        void OnValidate()
        {

            if (!isActiveAndEnabled) return;
            name = LayoutBorderDragger.baseName + " Top Handle";
            var le = gameObject.AddOrGetComponent<LayoutElement>();
            le.ignoreLayout = false;
            le.minHeight = topHeight;
            le.flexibleWidth = 0.001f;
            le.minWidth = topHeight * 2;
            le.preferredWidth = topHeight * 2;
            var rect = GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(0f, 20f);
            rect.pivot = new Vector2(0f, 1f);
 
        }


    }

}