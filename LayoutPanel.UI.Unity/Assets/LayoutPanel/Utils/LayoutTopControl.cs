
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Z.LayoutPanel
{
	//v.02 canvas scalefactor

	/// <summary>
	/// Mainly handles panel dragging
	/// </summary>
	[ExecuteInEditMode]
	public class LayoutTopControl : MonoBehaviourWithBg,
									IDragHandler,
									IBeginDragHandler,
									IEndDragHandler,
									IPointerClickHandler,
									IPointerEnterHandler,
									IPointerExitHandler
	{
		Canvas canvas;

		RectTransform rectToModify;

		public static LayoutTopControl draggedItem;

		public bool dragEnabled = true;

		int savedSibilingIndex;

		LayoutPanel panel;

		Transform savedParent;

		float dragOpacity = 0.4f;

		float lastClickTime;

		float timeToDubleCick = 0.5f;

		bool wasIgnore;

		Color savedColor;

		Image image;

		LayoutElement le;

		string unicodename
		{
			get { return "┌─╢  ╟─┐"; }
		}

		public bool updateName;

		public bool updateSize;

		public Text centerText;

		static Color pointerOverColor = new Color(0.15f, 0.15f, 0.15f, 0.15f);

		public void OnPointerClick(PointerEventData e)
		{
			if (Time.unscaledTime - lastClickTime < timeToDubleCick)
			{
				var fold = GetComponentInParent<FoldControlSomething>();
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
			if (!panel.detachedMode)
			{
				panel.AddOrGetComponent<CanvasGroup>().blocksRaycasts = false;
				panel.AddOrGetComponent<CanvasGroup>().alpha = dragOpacity;
				panel.PlaceTemporary(canvas.transform, -1);
			}

			panel.isBeingDragged = true;
			Cursor.SetCursor(LayoutResourceLoader.panCursor, LayoutBorderDragger.cursorCenter, CursorMode.Auto);
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
		}

		public void OnEndDrag(PointerEventData e)
		{
			if (!dragEnabled) return;

			panel.AddOrGetComponent<CanvasGroup>().blocksRaycasts = true;
			panel.gameObject.AddOrGetComponent<CanvasGroup>().alpha = 1;

			//  panel.detachedMode = savedFreeDragState;
			panel.isBeingDragged = false;
			if (!panel.detachedMode)
			{
				// Debug.Log("was non freedrag");
				if (LayoutDropTarget.currentTarget != null)
				{
					// Debug.Log("current target was " + LayoutDropTarget.currentTarget.name);
					panel.PlaceDropTarget(
						LayoutDropTarget.currentTarget.dropTarget,
						LayoutDropTarget.currentTarget.targetDropIndex);
				}
				else
				{
					Debug.Log("was no target");
					panel.PlaceTemporary(savedParent, savedSibilingIndex);
				}

				var ll = panel.GetComponent<LayoutElement>();
				if (ll != null) ll.ignoreLayout = wasIgnore;
			}
			else
			{
				Debug.Log("was freedrag");
			}

			// Debug.Log("ended drag");
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
			if (updateSize) UpdateSize();
			if (!isActiveAndEnabled) return;

			if (updateName && name != unicodename) name = unicodename;
		}

		void Reset()
		{
			UpdateSize();
#if PALETTES
            var color = gameObject.AddOrGetComponent<ColorSync>();
            if (string.IsNullOrEmpty(color.presetName)) color.presetName = "Top";
#endif
			var le = gameObject.AddOrGetComponent<LayoutElement>();
			le.ignoreLayout = true;
			var rect = GetComponent<RectTransform>();
			rect.pivot = new Vector2(0.5f, 1);
			rect.anchorMin = new Vector2(0, 1);
			rect.anchorMax = new Vector2(1, 1);
			rect.offsetMin = new Vector2(0, 0);
			rect.offsetMax = new Vector2(0, 0);
			rect.sizeDelta = new Vector2(0, 15); // sorr for hard coded values
		}

		void UpdateSize()
		{
			if (le == null) le = gameObject.AddOrGetComponent<LayoutElement>();
			le.ignoreLayout = false;
			if (LayoutSetupProvider.isReady)
			{
				le.minHeight = LayoutSetupProvider.setup.topHeight;
				le.flexibleWidth = 0.001f;
				le.minWidth = LayoutSetupProvider.setup.topHeight * 2;
				le.preferredWidth = LayoutSetupProvider.setup.topHeight * 2;
			}

			var rect = GetComponent<RectTransform>();
			rect.anchorMin = new Vector2(0f, 1f);
			rect.anchorMax = new Vector2(1f, 1f);
			rect.anchoredPosition = new Vector2(0f, 0f);

			//    if (updateSize)
			//rect.sizeDelta = new Vector2(2 * LayoutSettings.internalGroupPadding, 2 * LayoutSettings.topHeight + LayoutSettings.internalGroupPadding);
			rect.pivot = new Vector2(0f, 1f);
		}

		void OnEnable()
		{
			// if (updateSize) LayoutSettings.onBorderSizeChange += UpdateSize;
			if (updateSize) UpdateSize();
		}

		void OnDisable()
		{
			//   LayoutSettings.onBorderSizeChange -= UpdateSize;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (image == null) image = GetComponent<Image>();
			savedColor = image.color;
			image.color = savedColor + pointerOverColor;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			image.color = savedColor;
		}
	}
}
