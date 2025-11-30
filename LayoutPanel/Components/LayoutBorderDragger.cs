namespace zUI.LayoutPanelTools
{
#if UNITY_EDITOR
	using UnityEditor;
#endif
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	[RequireComponent(typeof(LayoutElement))]
	[ExecuteInEditMode]
	public class LayoutBorderDragger : MonoBehaviour,
									   IPointerEnterHandler,
									   IPointerExitHandler,
									   IDragHandler,
									   IBeginDragHandler,
									   IEndDragHandler,
									   IDropTarget
	{
		public DrawInspectorBg draw;

		const float columnModeOffset = 3f;

		private int minRectWidth = 50; // won't allow scaling lower than that

		private int minRectHeight = 30; // won't allow scaling lower than that

		public bool columnMode;

		private LayoutPanel panel;

		public VerticalLayoutGroup groupToDisableWhenDragging;

		[SerializeField]
		protected Texture2D hoverCursor;

		[SerializeField]
		private Side _side;

		private LayoutFoldController foldController;

		private Color savedColor;

		[SerializeField]
		public bool bordersPlacedInside;

		private bool isFolded;

		private static float alphaMultiplier = 1.5f;

		private RectTransform targetRect;

		private bool restorePivotAfterMove = true;

		private Vector2 savedPivot;

		public static string baseName
		{
			get { return "[Panel]"; }
		}

		public bool freeResizeMode
		{
			get
			{
				if (panel == null) return true;

				return panel.freeMode;
			}
		}

		public static Color dropTargetColor
		{
			get { return new Color(0.2f, 1, 0.2f, 0.9f); }
		}

		RectTransform rect
		{
			get
			{
				if (_rect == null) _rect = GetComponent<RectTransform>();
				return _rect;
			}
		}

		RectTransform _rect;

		Image image
		{
			get
			{
				if (_image == null) _image = GetComponent<Image>();
				return _image;
			}
		}

		Image _image;

		public bool isHorizontal
		{
			get { return side == Side.Left || side == Side.Right; }
		}

		public static Vector2 cursorCenter
		{
			get { return Vector2.one * 16; }
		}

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

		private void Start()
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

		private void SetSize()
		{
			float border = columnMode ? 1.1f * LayoutPanel.borderSize : LayoutPanel.borderSize;
			Vector2 newSize = side.isHorizontal()
				? new Vector2(border, 0)
				: new Vector2(bordersPlacedInside ? -2 : 2 * border, border);

			if (LayoutTopControl.draggedItem != null && LayoutDropTarget.currentTargetObject == gameObject)
			{
				if (isHorizontal) newSize.x += border;
				else newSize.y += border;
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
					if (side == Side.Top) return 0;
					else return -1;
				}

				var thissib = panel.transform.GetSiblingIndex();
				if (side == Side.Bottom) thissib++;
				return thissib;
			}
		}

		private void GetCursor()
		{
			if (isHorizontal) hoverCursor = ResourceLoader.horizontalCursor;
			else hoverCursor = ResourceLoader.vertialCursor;
		}
#if UNITY_EDITOR
		private void OnValidate()
		{
			var le = gameObject.AddOrGetComponent<LayoutElement>();
			if (le != null) le.ignoreLayout = true;
			EditorApplication.delayCall += () =>
										   {
											   try
											   {
												   GetCursor();
												   GetTargets();
												   side = side;
											   }
											   catch (MissingReferenceException)
											   {
												   // object was destroyed in the meantime, sorry
											   }
										   };
		}
#endif

		private void GetTargets()
		{
			try
			{
				if (panel == null) panel = GetComponentInParent<LayoutPanel>();
			}
			catch (MissingReferenceException)
			{
				// object was destroyed in the meantime, sorry
			}
		}

		private void OnEnable()
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

		private void OnFoldToggle(bool b)
		{
			isFolded = b;
			side = side;
		}

		private void OnDisable()
		{
			LayoutPanel.onBorderSizeChange -= SetSize;
			if (foldController != null) foldController.onFold -= OnFoldToggle;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (!enableDrag) return;

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
			if (targetRect != null && restorePivotAfterMove) targetRect.SetPivot(savedPivot);
		}

		public void OnDrag(PointerEventData thisEventData)
		{
			if (!enableDrag) return;

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
				while (targetRect.rect.width < minRectWidth) targetRect.sizeDelta += Vector2.right * 5;
				while (targetRect.rect.height < minRectHeight) targetRect.sizeDelta += Vector2.up * 5;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			//      Debug.Log("pointerneterd " + name, gameObject);
			if (enableDrag && !columnMode && elementToResize != null)
				Cursor.SetCursor(hoverCursor, cursorCenter, CursorMode.Auto);
			savedColor = image.color;
			image.color = new Color(savedColor.r, savedColor.g, savedColor.b, savedColor.a * alphaMultiplier);
			if (LayoutTopControl.draggedItem != null &&
				LayoutTopControl.draggedItem.transform.parent != transform.parent)
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
		private void ManageVisibility()
		{
			LayoutBorderHide bh = transform.parent.gameObject.AddOrGetComponent<LayoutBorderHide>();
			UnityEditor.Selection.activeGameObject = bh.gameObject;
		}
#endif
	}
}
