namespace zUI
{
	using zUI.LayoutPanelTools;

	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	//v.02 canvas scalefactor

	[ExecuteInEditMode]
	public class LayoutTopControl : MonoBehaviour,
									IDragHandler,
									IBeginDragHandler,
									IEndDragHandler,
									IPointerClickHandler
	{
		public DrawInspectorBg draw;

		private Canvas canvas;

		private RectTransform rectToModify;

		public static LayoutTopControl draggedItem;

		public bool dragEnabled = true;

		public bool freeDrag => panel.freeMode;

		private int savedSibilingIndex;

		private LayoutPanel panel;

		private Transform savedParent;

		private float dragOpacity = 0.3f;

		private float lastClickTime;

		private float timeToDubleCick = 0.5f;

		private bool wasIgnore;

		private bool savedFreeDragState;

		LayoutElement le;

		public void OnPointerClick(PointerEventData e)
		{
			if (Time.unscaledTime - lastClickTime < timeToDubleCick)
			{
				var fold = GetComponentInParent<LayoutFoldController>();
				if (fold != null) fold.ToggleFold();
			}

			lastClickTime = Time.unscaledTime;
		}

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
			panel.freeMode = true;
			Cursor.SetCursor(ResourceLoader.panCursor, LayoutBorderDragger.cursorCenter, CursorMode.Auto);
		}

		public void OnDrag(PointerEventData e)
		{
			if (!dragEnabled) return;

			if (rectToModify == null)
			{
				if (panel != null) rectToModify = panel.GetComponent<RectTransform>();
				else rectToModify = transform.parent.GetComponent<RectTransform>();
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
			// debug logs commented out, not removed, as they might be useful for debugging
			if (!freeDrag)
			{
				if (LayoutDropTarget.currentTarget != null)
				{
					// Debug.Log("current target was " + LayoutDropTarget.currentTarget.name);
					panel.PlaceDropTarget(
						LayoutDropTarget.currentTarget.dropTarget,
						LayoutDropTarget.currentTarget.targetDropIndex);
				}
				else
				{
					// Debug.Log("was no target");
					panel.PlaceDropTarget(savedParent, savedSibilingIndex);
				}

				var ll = panel.GetComponent<LayoutElement>();
				if (ll != null) ll.ignoreLayout = wasIgnore;
			}
			else
			{
				// Debug.Log("was freedrag");
			}

			draggedItem = null;
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		private void Start()
		{
			if (panel == null) panel = GetComponentInParent<LayoutPanel>();
			OnValidate();
		}

		public void OnValidate()
		{
			if (!isActiveAndEnabled) return;
#if UNITY_EDITOR
			name = "┌─╢  ╟─┐";
			UnityEditor.EditorApplication.delayCall += () =>
													   {
														   try
														   {
															   UpdateSize();
															   var rect = GetComponent<RectTransform>();
															   rect.anchorMin = new Vector2(0f, 1f);
															   rect.anchorMax = new Vector2(1f, 1f);
															   rect.anchoredPosition = new Vector2(0f, 0f);
															   rect.sizeDelta = new Vector2(0f, 20f);
															   rect.pivot = new Vector2(0f, 1f);
														   }
														   catch (MissingReferenceException)
														   {
															   // object was destroyed in the meantime, sorry
														   }
													   };
#endif
		}

		private void UpdateSize()
		{
			if (le == null) le = gameObject.AddOrGetComponent<LayoutElement>();
			le.ignoreLayout = false;
			le.minHeight = LayoutPanel.topHeight;
			le.flexibleWidth = 0.001f;
			le.minWidth = LayoutPanel.topHeight * 2;
			le.preferredWidth = LayoutPanel.topHeight * 2;
		}

		private void OnEnable()
		{
			LayoutPanel.onBorderSizeChange += UpdateSize;
		}

		private void OnDisable()
		{
			LayoutPanel.onBorderSizeChange -= UpdateSize;
		}
	}
}
