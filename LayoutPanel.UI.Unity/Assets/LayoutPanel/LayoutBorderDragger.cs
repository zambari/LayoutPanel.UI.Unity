using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LayoutPanelDependencies;
using zUI;

namespace Z.LayoutPanel
{

    [RequireComponent(typeof(LayoutElement))]
    [ExecuteInEditMode]
    public class LayoutBorderDragger : MonoBehaviourWithBg, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropTarget
    {

        public bool columnMode;
        public static string baseName { get { return "[Panel]"; } }
        LayoutPanel panel;
        public bool freeResizeMode
        {
            get
            {
                if (panel == null) return true;
                return panel.detachedMode;
            }
        }
        //[HideInInspector] 
        public VerticalLayoutGroup groupToDisableWhenDragging;
        [SerializeField]
        protected Texture2D hoverCursor;
        [SerializeField] Side _side;

        LayoutFoldController foldController;
        Color savedColor;

        [SerializeField] public bool bordersPlacedInside;
        RectTransform rect { get { if (_rect == null) _rect = GetComponent<RectTransform>(); return _rect; } }
        RectTransform _rect;
        Image image { get { if (_image == null) _image = GetComponent<Image>(); return _image; } }
        Image _image;
        public bool isHorizontal { get { return side == Side.Left || side == Side.Right; } }
        public static Vector2 cursorCenter { get { return Vector2.one * 16; } }

        bool isFolded;
        static float alphaMultiplier = 1.5f;
        RectTransform targetRect;
        public bool enableDrag
        {
            get
            {
                if (isFolded) return false;
                // if (elementToResize == null) return false;
                if (panel == null) panel = GetComponentInParent<LayoutPanel>();
                if (panel != null)
                {
                    if (panel.detachedMode) return true;
                    // if (side == Side.Top) return panel.isAlignedBottom;
                    // if (side == Side.Bottom) return !panel.isAlignedBottom;
                }
                return true;
            }
        }
        public bool isPanelHorizontal
        {
            get
            {
                if (panel == null) return false;
                return panel.isInHorizontalGroup;
            }
        }

        bool restorePivotAfterMove = true;
        Vector2 savedPivot;

        public LayoutElement elementToResize
        {
            get
            {
                if (panel == null) panel = GetComponentInParent<LayoutPanel>();
                if (panel == null) return null;
                return panel.GetTargetElementForSide(side);
            }
        }
        public Transform dropTarget
        {
            get
            {
                if (panel == null) return transform.transform.parent;
                return panel.GetTargetTransformForSide(side);
            }
        }

        void Start()
        {
            ScrollRect scroll = transform.parent.GetComponentInChildren<ScrollRect>();
            if (scroll != null)
            {
                if (scroll.content != null)
                {
                    VerticalLayoutGroup vertical = scroll.content.GetComponentInChildren<VerticalLayoutGroup>();
                    if (vertical != null) groupToDisableWhenDragging = vertical;
                }
            }
        }

        void SetSize()
        {
            float border = columnMode ? 1.1f * LayoutSettings.borderSize : LayoutSettings.borderSize;
            Vector2 newSize = Vector2.zero;

            if (side.isHorizontal()) newSize = new Vector2(border, 0);
            else
            {
                newSize = new Vector2((bordersPlacedInside ? -2 : 2 )* border, border);
            }
            if (LayoutTopControl.draggedItem != null && LayoutDropTarget.currentTargetObject == gameObject)
            {
           //     if (isHorizontal) newSize.x += border; else newSize.y += border;

            }
            rect.sizeDelta = newSize;
        }
        public Side side
        {
            get { return _side; }
            set
            {
                Vector3 newAnchoredPosition = Vector3.zero;
                _side = value;
                rect.anchorMin = _side.GetAnchorMin();
                rect.anchorMax = _side.GetAnchorMax();
                rect.pivot = _side.GetPivot(bordersPlacedInside);

                rect.anchoredPosition = Vector2.zero;
                name = side.GetObjectLabel();
                if (columnMode)
                {
                    if (_side == Side.Top)
                        newAnchoredPosition += new Vector3(0, LayoutSettings.borderSizeColumnOffset);
                    if (_side == Side.Bottom)
                        newAnchoredPosition += new Vector3(0, -LayoutSettings.borderSizeColumnOffset);
                    rect.anchoredPosition = newAnchoredPosition;
                    GetCursor();
                }
                SetSize();
            }
        }
        public int targetDropIndex
        {
            get
            {
                if (columnMode)
                {
                    if (side == Side.Top)
                        return 0;
                    else
                        return -1;
                }
                var thissib = panel.transform.GetSiblingIndex();
                if (side == Side.Bottom) thissib++;
                return thissib;
            }
        }
        public bool isHorizontalBar { get { return isHorizontal; } }

        void GetCursor()
        {
            if (isHorizontal)
                hoverCursor = zResourceLoader.horizontalCursor;
            else
                hoverCursor = zResourceLoader.vertialCursor;
        }

        void OnValidate()
        {
            var le = gameObject.AddOrGetComponent<LayoutElement>();
            if (le != null) le.ignoreLayout = true;

            GetCursor();
            GetTargets();
            side = side;
        }

        void GetTargets()
        {
            name = side.GetObjectLabel();
            if (panel == null) panel = GetComponentInParent<LayoutPanel>();
            //   enableDrag = (elementToResize != null);
        }
        void OnEnable()
        {
            LayoutSettings.onBorderSizeChange += SetSize;
            SetSize();
            GetTargets();
            foldController = GetComponentInParent<LayoutFoldController>();
            if (foldController != null)
            {
                foldController.onFold += OnFoldToggle;
                OnFoldToggle(foldController.isFolded);
            }
            var vh = GetComponentInParent<LayoutBorderHide>();
            if (vh != null)
            {
                image.color = vh.borderColor;
            }
        }

        void OnFoldToggle(bool b)
        {
            isFolded = b;
            side = side;
        }
        void OnDisable()
        {
            LayoutSettings.onBorderSizeChange -= SetSize;
            if (foldController != null)
                foldController.onFold -= OnFoldToggle;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!enableDrag)
            {
                Debug.Log("drag not enabled");
                return;
            }

            if (groupToDisableWhenDragging != null) groupToDisableWhenDragging.enabled = false;
            if (hoverCursor == null) GetCursor();
            if (freeResizeMode)
            {
                targetRect = transform.parent.GetComponent<RectTransform>();
                savedPivot = targetRect.pivot;
            }
            else
            {
                if (elementToResize == null)
                {
                    Debug.Log("did not find elementToResize");
                    return;
                }

                if (isHorizontal)
                {
                    if (elementToResize.preferredWidth == -1)
                        elementToResize.preferredWidth = elementToResize.GetComponent<RectTransform>().rect.width / 3; //hahha
                }
                else
                {
                    if (elementToResize.preferredHeight == -1)
                        elementToResize.preferredHeight = elementToResize.GetComponent<RectTransform>().rect.height / 3;//hahha

                }
            }
        }
        static Vector3[] corners = new Vector3[4];
        float GetActualWidth(RectTransform rect)
        {
            rect.GetWorldCorners(corners);
            return Mathf.Abs(corners[0].x - corners[2].x);

        }
        float GetActualHeight(RectTransform rect)
        {
            rect.GetWorldCorners(corners);
            return Mathf.Abs(corners[0].y - corners[1].y);

        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (groupToDisableWhenDragging != null) groupToDisableWhenDragging.enabled = true;
            if (targetRect != null && restorePivotAfterMove)
                targetRect.SetPivot(savedPivot);

        }
        public void OnDrag(PointerEventData thisEventData)
        {
            if (!enableDrag)
                return;
            Vector2 delta = thisEventData.delta;

            if (elementToResize == null && !freeResizeMode)
            {
                Debug.Log("sorry, no target element");
                return;
            }
            if (!freeResizeMode)
            {
                delta = side.SideDelta(delta);
                elementToResize.preferredWidth += delta.x;
                elementToResize.preferredHeight += delta.y;
                if (elementToResize.preferredWidth <= 1) elementToResize.preferredWidth = 32;
            }
            else
            {
                targetRect.SetPivot(side.GetPivot());
                targetRect.sizeDelta = targetRect.sizeDelta + side.SideDelta(delta);
                while (targetRect.rect.width < LayoutSettings.minRectWidth)
                    targetRect.sizeDelta += Vector2.right * 5;
                while (targetRect.rect.height < LayoutSettings.minRectHeight)
                    targetRect.sizeDelta += Vector2.up * 5;
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            //      Debug.Log("pointerneterd " + name, gameObject);
            if (enableDrag && !columnMode && elementToResize != null)
                Cursor.SetCursor(hoverCursor, cursorCenter, CursorMode.Auto);
            savedColor = image.color;
            image.color = new Color(savedColor.r, savedColor.g, savedColor.b, savedColor.a * alphaMultiplier);
            if (LayoutTopControl.draggedItem != null && LayoutTopControl.draggedItem.transform.parent != transform.parent)
            {
                // if (panel!=null)
                // if (panel.isInHorizontalGroup && isHorizontal)
                // {

                // }
                // if (panel.isInVerticalGroup)
                // if (!isHorizontal)
                {
                    LayoutDropTarget.currentTargetObject = gameObject;
                    if (panel != null)
                    {
                        if ((panel.isInHorizontalGroup && !isHorizontal) ||
                        (panel.isInVerticalGroup && isHorizontal))
                            image.color = LayoutSettings.dropTargetColorWhenSplit;
                        else
                            image.color = LayoutSettings.dropTargetColor;
                    }
                    else
                    {
                        Debug.Log("no panel");
                    }
                }
            }
            SetSize();

        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (LayoutTopControl.draggedItem != null && LayoutDropTarget.currentTargetObject == gameObject)
                LayoutDropTarget.currentTargetObject = null;
            image.color = savedColor;
            SetSize();
            Cursor.SetCursor(null, cursorCenter, CursorMode.Auto);
        }
#if UNITY_EDITOR
        [ExposeMethodInEditor]
        void ManageVisibility()
        {
            LayoutBorderHide bh = transform.parent.gameObject.AddOrGetComponent<LayoutBorderHide>();
            UnityEditor.Selection.activeGameObject = bh.gameObject;
        }
#endif

    }

}