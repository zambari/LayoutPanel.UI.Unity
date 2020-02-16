using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LayoutPanelDependencies;
namespace zUI
{
    public interface IDropTarget
    {
        int targetDropIndex { get; }
        Transform dropTarget { get; }
        Transform transform { get; }
        GameObject gameObject { get; }
        bool isHorizontal {get;}
        string name { get; }
    }


    public static class LayoutDropTarget
    {
        public static GameObject currentTargetObject;
        public static IDropTarget currentTarget
        {
            get
            {
                if (currentTargetObject == null) return null;
                else return currentTargetObject.GetComponent<IDropTarget>();
            }
        }
    }

    [RequireComponent(typeof(LayoutElement))]
    [ExecuteInEditMode]
    public class LayoutBorderDragger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropTarget
    {

        public DrawInspectorBg draw;

        int minRectWidth = 50; // won't allow scaling lower than that
        int minRectHeight = 30; // won't allow scaling lower than that
        public bool columnMode;
        public static string baseName { get { return "[Panel]"; } }
        LayoutPanel panel;
        public bool freeResizeMode
        {
            get
            {
                if (panel == null) return true;
                return panel.freeMode;
            }
        }
        //[HideInInspector] 
        public VerticalLayoutGroup groupToDisableWhenDragging;
        [SerializeField]
        protected Texture2D hoverCursor;
        public static Color dropTargetColor { get { return new Color(0.2f, 1, 0.2f, 0.9f); } }

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
        const float columnModeOffset = 3f;
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
                    if (panel.freeMode) return true;
                    if (side == Side.Top) return panel.isAlignedBottom;
                    if (side == Side.Bottom) return !panel.isAlignedBottom;
                }
                return true;
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
            float border = columnMode ? 1.1f * LayoutPanel.borderSize : LayoutPanel.borderSize;
            Vector2 newSize = side.isHorizontal() ? new Vector2(border, 0) : new Vector2(bordersPlacedInside ? -2 : 2 * border, border);


            if (LayoutTopControl.draggedItem != null && LayoutDropTarget.currentTargetObject == gameObject)
            {
                if (isHorizontal) newSize.x += border; else newSize.y += border;

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

                if (columnMode)
                {
                    if (_side == Side.Top)
                        newAnchoredPosition += new Vector3(0, LayoutPanel.borderSize * columnModeOffset);
                    if (_side == Side.Bottom)
                        newAnchoredPosition += new Vector3(0, -LayoutPanel.borderSize * columnModeOffset);
                    //  if
                    //name = LayoutBorderDragger.baseName + " " + _side + " dragger";
                    name = side.GetObjectLabel();
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
            if (panel == null) panel = GetComponentInParent<LayoutPanel>();
            //   enableDrag = (elementToResize != null);
        }
        void OnEnable()
        {
            LayoutPanel.onBorderSizeChange += SetSize;
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
            LayoutPanel.onBorderSizeChange -= SetSize;
            if (foldController != null)
                foldController.onFold -= OnFoldToggle;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!enableDrag)
                return;

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
                }
            }
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
                while (targetRect.rect.width < minRectWidth)
                    targetRect.sizeDelta += Vector2.right * 5;
                while (targetRect.rect.height < minRectHeight)
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
               if (!isHorizontal)
                {
                    LayoutDropTarget.currentTargetObject = gameObject;
                    image.color = dropTargetColor;
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