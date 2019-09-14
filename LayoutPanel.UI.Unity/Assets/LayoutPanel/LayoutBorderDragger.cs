using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace zUI
{
   public interface IDropTarget
    {
        int targetDropIndex { get; }
        Transform dropTarget { get; }
        Transform transform { get; }
        GameObject gameObject { get; }
        string name { get; }
    }

	public static class LayoutDropTarget
	{
		public static GameObject currentTargetObject;
		public static IDropTarget currentTarget {get { if (currentTargetObject==null) return null; else return currentTargetObject.GetComponent<IDropTarget>();}}
	}

    [RequireComponent(typeof(LayoutElement))]
    [ExecuteInEditMode]
    public class LayoutBorderDragger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IDropTarget
    {
        public DrawInspectorBg draw;
        public bool columnMode;
        public static string baseName { get { return "[BRDR]"; } }
        bool positionOutside = true;
        [SerializeField]
        protected Texture2D hoverCursor;
        public static Color dropTargetColor { get { return new Color(0.2f, 1, 0.2f, 0.9f); } }
        public enum Side { Left, Right, Top, Bottom };
        [SerializeField] Side _side;
        LayoutPanel panel;
        static int _borderSize = 6;
        const float columnModeOffset = 3f;
        bool isFolded;
        public bool enableDrag
        {
            get
            {
                if (isFolded) return false;
                if (elementToResize == null) return false;
                if (panel == null) panel = GetComponentInParent<LayoutPanel>();
                if (panel != null)
                {
                    if (side == Side.Top) return panel.isAlignedBottom;
                    if (side == Side.Bottom) return !panel.isAlignedBottom;
                }
                return true;
            }
        }
        public static int borderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = value;
                if (redraw != null) redraw();
            }
        }
        static System.Action redraw;
        [SerializeField] bool sideStrechToBottomCorner = true;

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
        [ReadOnly] [SerializeField] bool customElementToModify;

        LayoutFoldController foldController;
        Color savedColor;

        [HideInInspector] [SerializeField] bool sideInside;
        RectTransform rect { get { if (_rect == null) _rect = GetComponent<RectTransform>(); return _rect; } }
        RectTransform _rect;
        Image image { get { if (_image == null) _image = GetComponent<Image>(); return _image; } }
        Image _image;
        public bool isHorizontal { get { return side == Side.Left || side == Side.Right; } }
        public static Vector2 cursorCenter { get { return Vector2.one * 16; } }
        public Side side
        {
            get { return _side; }
            set
            {
                Vector3 newAnchoredPosition = Vector3.zero; ;
                float border = columnMode ? 1.1f * borderSize : borderSize;
                _side = value;
                if (_side == Side.Left)
                {
                    rect.anchorMin = new Vector2(0, 0);
                    rect.anchorMax = new Vector2(0, 1);
                    if (sideInside)
                        rect.pivot = new Vector2(0, .5f);
                    else
                        rect.pivot = new Vector2(1, .5f);
                    rect.sizeDelta = new Vector2(border, 0);
                }
                if (_side == Side.Right)
                {
                    rect.anchorMin = new Vector2(1, 0);
                    rect.anchorMax = new Vector2(1, 1);
                    if (sideInside)
                        rect.pivot = new Vector2(1, .5f);
                    else
                        rect.pivot = new Vector2(0, .5f);
                    rect.sizeDelta = new Vector2(border, 0);

                }
                if (sideStrechToBottomCorner && (_side == Side.Left || _side == Side.Right))
                    rect.sizeDelta = new Vector2(border, 2 * border);

                if (_side == Side.Top)
                {
                    rect.anchorMin = new Vector2(0, 1);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.pivot = new Vector2(0, positionOutside ? 0 : 1);
                    if (columnMode)
                    {
                        rect.pivot = new Vector2(0.5f, .5f);
                        rect.sizeDelta = new Vector2(border * 2, border * 2);
                    }
                    else
                        rect.sizeDelta = new Vector2(0, border);
                }
                if (_side == Side.Bottom)
                {
                    rect.anchorMin = new Vector2(0, 0);
                    rect.anchorMax = new Vector2(1, 0);
                    rect.pivot = new Vector2(0, positionOutside ? 1 : 0);
                    if (columnMode)
                    {
                        rect.pivot = new Vector2(0.5f, .5f);
                        rect.sizeDelta = new Vector2(border * 2, border * 2);
                    }
                    else
                        rect.sizeDelta = new Vector2(0, border);
                }

                if (LayoutTopControl.draggedItem != null && LayoutDropTarget.currentTargetObject == gameObject)
                {
                    rect.sizeDelta = rect.sizeDelta * 2;// isHorizontal ? rect.sizeDelta = new Vector2(borderSize * 2, 0) : rect.sizeDelta = new Vector2(0, borderSize * 2);
                }
                if (columnMode)
                {
                    if (_side == Side.Top)
                    {
                        newAnchoredPosition += new Vector3(0, borderSize * columnModeOffset);
                    }
                    if (_side == Side.Bottom)
                    {
                        newAnchoredPosition += new Vector3(0, -borderSize * columnModeOffset);
                    }
                }
                name = LayoutBorderDragger.baseName +" " + _side + " dragger";
                rect.anchoredPosition = newAnchoredPosition;
                GetCursor();
            }
        }
        public int targetDropIndex
        {
            get
            {
                if (columnMode)
                {
                    if (side == LayoutBorderDragger.Side.Top)
                        return 0;
                    else
                        return -1;
                }
                var thissib = panel.transform.GetSiblingIndex();
                if (side == LayoutBorderDragger.Side.Bottom) thissib++;
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
            GetCursor();
            GetTargets();
            Redraw();
          
            //  GetTargetElement();
        }
        void Redraw()
        {   var le = gameObject.AddOrGetComponent<LayoutElement>();
            if (le != null) le.ignoreLayout = true;
            side = side;
        }
        void GetTargets()
        {
            if (panel == null) panel = GetComponentInParent<LayoutPanel>();
            //   enableDrag = (elementToResize != null);
        }
        void OnEnable()
        {
            redraw += Redraw;
            Redraw();
            GetTargets();
            foldController = GetComponentInParent<LayoutFoldController>();
            if (foldController != null)
            {
                foldController.onFold += OnFoldToggle;
                OnFoldToggle(foldController.isFolded);
            }
        }

        void OnFoldToggle(bool b)
        {
            isFolded = b;
            side = side;
            // if (isHorizontal) image.enabled = !b;
        }
        void OnDisable()
        {
            redraw -= Redraw;
            if (foldController != null)
                foldController.onFold -= OnFoldToggle;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!enableDrag) return;
            if (elementToResize == null)
            {
                Debug.Log("did not find elementToResize");
            }
            if (hoverCursor == null) GetCursor();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!enableDrag) return;
            if (elementToResize == null)
            {
                Debug.Log("sorry, no target element");
                return;
            }

            if (side == Side.Left || side == Side.Right)
            {
                if (side == Side.Left)
                    elementToResize.preferredWidth -= eventData.delta.x / 2;

                if (side == Side.Right)
                    elementToResize.preferredWidth += eventData.delta.x / 2;
                if (elementToResize.preferredWidth <= 1) elementToResize.preferredWidth = 32;

            }
            if (side == Side.Top)
                elementToResize.preferredHeight += eventData.delta.y;
            if (side == Side.Bottom)
                if (elementToResize.preferredHeight > eventData.delta.y)
                    elementToResize.preferredHeight -= eventData.delta.y;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableDrag && !columnMode && elementToResize != null)
                Cursor.SetCursor(hoverCursor, cursorCenter, CursorMode.Auto);
            savedColor = image.color;
            if (LayoutTopControl.draggedItem != null)
            {
                if (!isHorizontal)
                {
                    LayoutDropTarget.currentTargetObject = gameObject;
                    image.color = dropTargetColor;
                }
            }
            Redraw();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (LayoutTopControl.draggedItem != null)
            {
                if (LayoutDropTarget.currentTargetObject == gameObject)
                {
                    LayoutDropTarget.currentTargetObject = null;
                    image.color = savedColor;
                }
            }
            Redraw();
            Cursor.SetCursor(null, cursorCenter, CursorMode.Auto);
            image.color = savedColor;
        }

    }


}