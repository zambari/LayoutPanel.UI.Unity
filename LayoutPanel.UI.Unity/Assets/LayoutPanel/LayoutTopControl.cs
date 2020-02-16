using System.Collections;
using System.Collections.Generic;
using LayoutPanelDependencies;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace zUI
{

    //v.02 canvas scalefactor

    [ExecuteInEditMode]
    public class LayoutTopControl : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public DrawInspectorBg draw;

        Canvas canvas;
        RectTransform rectToModify;
        public static LayoutTopControl draggedItem;
        public bool dragEnabled = true;
        public bool freeDrag { get { return panel.freeMode; } }
        int savedSibilingIndex;
        LayoutPanel panel;
        Transform savedParent;

        float dragOpacity = 0.3f;
        float lastClickTime;
        float timeToDubleCick = 0.5f;
        bool wasIgnore;
        bool savedFreeDragState;

        public void OnPointerClick(PointerEventData e)
        {
            if (Time.unscaledTime - lastClickTime < timeToDubleCick)
            {
                var fold = GetComponentInParent<LayoutFoldController>();
                if (fold != null) fold.ToggleFold();
            }
            lastClickTime = Time.unscaledTime;
        }
        LayoutElement le;

        public void OnBeginDrag(PointerEventData e)
        {
            if (canvas == null) canvas = GetComponentInParent<Canvas>();
            if (!dragEnabled) return;
            if (panel == null) panel = GetComponentInParent<LayoutPanel>();

            if (panel != null)
            {
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
            }

            draggedItem = this;
            if (!freeDrag)
            {
                panel.AddOrGetComponent<CanvasGroup>().blocksRaycasts = false;
                panel.AddOrGetComponent<CanvasGroup>().alpha = dragOpacity;
                panel.PlaceDropTarget(canvas.transform, -1);
            }
            savedFreeDragState = panel.freeMode;
            panel.freeMode = true; /// !!!11
            Cursor.SetCursor(zResourceLoader.panCursor, LayoutBorderDragger.cursorCenter, CursorMode.Auto);
        }
        public void OnDrag(PointerEventData e)
        {
            if (!dragEnabled) return;


            if (rectToModify == null)
            {
                if (panel != null)
                    rectToModify = panel.GetComponent<RectTransform>();
                else
                    rectToModify = transform.parent.GetComponent<RectTransform>();

            }
            var currentpos = rectToModify.localPosition;
            currentpos += new Vector3(e.delta.x / canvas.scaleFactor, e.delta.y / canvas.scaleFactor, 0);
            rectToModify.localPosition = currentpos;
            if (LayoutDropTarget.currentTarget != null)
            {
                // Debug.Log(LayoutDropTarget.currentTarget.name);
            }
        }

        public void OnEndDrag(PointerEventData e)
        {
            if (!dragEnabled) return;
            panel.AddOrGetComponent<CanvasGroup>().blocksRaycasts = true;
            panel.gameObject.AddOrGetComponent<CanvasGroup>().alpha = 1;
             panel.freeMode = savedFreeDragState;
           
            if (!freeDrag)
            {
                Debug.Log("was non freedrag");
                if (LayoutDropTarget.currentTarget != null)
                {
                    Debug.Log("current target was " + LayoutDropTarget.currentTarget.name);
                    panel.PlaceDropTarget(LayoutDropTarget.currentTarget.dropTarget, LayoutDropTarget.currentTarget.targetDropIndex);
                }
                else
                {
                    Debug.Log("was no target");
                    panel.PlaceDropTarget(savedParent, savedSibilingIndex);
                }
                var ll = panel.GetComponent<LayoutElement>();
                if (ll != null) ll.ignoreLayout = wasIgnore;
            }
            else
            {
                Debug.Log("was freedrag");
            }
            Debug.Log("ended drag");
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
            name = "┌─╢  ╟─┐";

            UpdateSize();
            var rect = GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(0f, 0f);
            rect.sizeDelta = new Vector2(0f, 20f);
            rect.pivot = new Vector2(0f, 1f);

        }
        void UpdateSize()
        {
            if (le == null) le = gameObject.AddOrGetComponent<LayoutElement>();
            le.ignoreLayout = false;
            le.minHeight = LayoutPanel.topHeight;
            le.flexibleWidth = 0.001f;
            le.minWidth = LayoutPanel.topHeight * 2;
            le.preferredWidth = LayoutPanel.topHeight * 2;
        }

        void OnEnable()
        {
            LayoutPanel.onBorderSizeChange += UpdateSize;
        }
        void OnDisable()
        {
            LayoutPanel.onBorderSizeChange -= UpdateSize;

        }
    }

}