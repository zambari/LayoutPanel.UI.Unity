namespace zUI
{
	using UnityEngine;

	using zUI.LayoutPanelTools;
#if UNITY_EDITOR
	using UnityEditor;
#endif
	using UnityEngine.UI;

	[ExecuteInEditMode]
	public class LayoutItemCreator : MonoBehaviour
	{
		public DrawInspectorBg draw;

		public BorderHideMode borderHideMode;

		public bool bordersPlacedInside;

		public bool removeMeWhenDone = true;

		[SerializeField]
		private LayoutTopControl topControl;

		[SerializeField]
		private LayoutPanel panel;

		[SerializeField]
		private LayoutFoldController foldController;

		[SerializeField]
		private Button foldButton;

		[SerializeField]
		private LayoutElement content;

		[SerializeField]
		private LayoutBorderHide layoutBorderHide;

		[Header("If present will text raycast catcher")]
		[SerializeField]
		private float borderOverScan = 0;

		private Color textColor = Color.white * 0.8f;

		private LayoutCreator layoutCreator;

		public static Font font => ResourceLoader.DefaultFont;

		private void OnValidate()
		{
			TryToGetReferences();
		}

		private void TryToGetReferences()
		{
			if (topControl == null) topControl = GetComponentInChildren<LayoutTopControl>();
			if (panel == null) panel = gameObject.GetComponent<LayoutPanel>();
			if (foldController == null) foldController = gameObject.GetComponent<LayoutFoldController>();
			if (foldController != null) foldButton = foldController.foldButton;
			if (layoutBorderHide == null) layoutBorderHide = GetComponent<LayoutBorderHide>();
		}

		private void Reset()
		{
			RandomizeColor();
		}

		private void RandomizeColor()
		{
			Image image = GetComponent<Image>();
			if (image != null)
			{
#if UTNIY_EDITOR
				Undo.RegisterCompleteObjectUndo(image, "color");
#endif
				Color color =
					(Color.black +
					 new Color(0, Random.value * .2f, 0) +
					 Color.white * 0.4f); // Color.white * 0.2f).Randomize(1,1,.3f);
				color.a = 0.7f;
				image.color = color.Randomize(2, .4f, .3f);
			}
		}

		private void Awake()
		{
			layoutCreator = GetComponentInParent<LayoutCreator>();
			if (layoutCreator != null)
			{
				textColor = layoutCreator.textColor;
			}
		}

		public void AddTop()
		{
			topControl = GetComponentInChildren<LayoutTopControl>();
			if (topControl == null)
			{
				var topImage = gameObject.AddImageChild();
				if (layoutBorderHide != null) topImage.color = layoutBorderHide.borderColor.Randomize();
				topControl = topImage.gameObject.AddOrGetComponent<LayoutTopControl>();
				if (layoutCreator != null)
				{
					topImage.color = layoutCreator.topColor;
				}
#if UNITY_EDITOR
				Undo.RegisterCreatedObjectUndo(topControl.gameObject, "top");
#endif
			}

			topControl.transform.SetAsFirstSibling();
		}

		[ExposeMethodInEditor]
		private void AddBordersOnly()
		{
			layoutBorderHide = gameObject.AddOrGetComponent<LayoutBorderHide>();

			layoutBorderHide.borderColor = layoutBorderHide.borderColor.Randomize(01f, 1f, 0.2f);

			layoutBorderHide.borderHideMode = borderHideMode;

			// borders
			int count = System.Enum.GetNames(typeof(Side)).Length;
			for (int i = count - 1; i >= 0; i--)
			{
				var thisChild = gameObject.AddImageChild();
				thisChild.transform.SetAsFirstSibling();
				var d = thisChild.gameObject.AddOrGetComponent<LayoutBorderDragger>();
				d.bordersPlacedInside = bordersPlacedInside;
				if (borderOverScan > 0) HandleTextRaycastCatchers(thisChild.gameObject);

				d.side = (Side)i;
#if UNITY_EDITOR
				Undo.RegisterCreatedObjectUndo(thisChild.gameObject, "borders");
#endif
			}
		}

		private void HandleTextRaycastCatchers(GameObject a)
		{
			var text = a.gameObject.AddTextChild();
			text.text = null;
			text.supportRichText = false;
			text.GetComponent<RectTransform>().sizeDelta = new Vector2(borderOverScan, borderOverScan);
			text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			text.name = "raycast catcher";
			text.font = font;
			text.color = textColor;
		}

		void HandleTopLabel()
		{
			Text labelText = null; //panel.labelText; ;//= topControl.GetComponentInChildren<Text>();
			if (labelText == null)
			{
				//                Debug.Log("creating text", gameObject);
				labelText = topControl.gameObject.AddTextChild();
				labelText.name = "Panel Label";
				labelText.text = name;
				labelText.font = font;
				labelText.alignment = TextAnchor.MiddleLeft;
				labelText.color = textColor;
				var textRect = labelText.GetComponent<RectTransform>();

				textRect.anchorMin = new Vector2(0f, 0f);
				textRect.anchorMax = new Vector2(1f, 1f);
				textRect.anchoredPosition = new Vector2(-7.5f, 0f);
				textRect.sizeDelta = new Vector2(-25f, 4f);
				textRect.pivot = new Vector2(0.5f, 0.5f);
			}

			panel.labelText = labelText;
		}

		private void HandleThisObject()
		{
			RectTransform rect = gameObject.AddOrGetComponent<RectTransform>();
			float w = rect.rect.width;
			float h = rect.rect.height;
			rect.anchorMin = Vector2.one / 2;
			rect.anchorMax = Vector2.one / 2;
			rect.sizeDelta = new Vector2(h, w);
			var fold = gameObject.AddOrGetComponent<LayoutFoldController>();
			if (fold.foldButton == null)
			{
				foldButton = CreateFoldButton();
				foldButton.name = "FoldButton";
				fold.foldButton = foldButton;
			}
			else foldButton = fold.foldButton;

			var foldStatusText = fold.foldButton.GetComponentInChildren<Text>();
			foldStatusText.name = "FoldStatusText";
			if (foldStatusText.font == null) foldStatusText.font = ResourceLoader.DefaultFont;
			fold.foldLabelText = foldStatusText;
			Fold(fold);
			foldStatusText.text = fold.GetFoldString();
			VerticalLayoutGroup group = gameObject.AddOrGetComponent<VerticalLayoutGroup>();
			group.SetChildControl(2);
			Fold(group);
			panel = gameObject.AddOrGetComponent<LayoutPanel>();
			if (panel.resizableElement == null) panel.resizableElement = content;
		}

		private void HandleContent()
		{
			content = gameObject.AddImageChild().AddOrGetComponent<LayoutElement>();
#if UNITY_EDITOR
			Undo.RegisterCreatedObjectUndo(content.gameObject, "content");
#endif

			content.minHeight = LayoutPanel.topHeight;
			content.minWidth = 26;
			content.flexibleWidth = 0.001f;
			content.GetComponent<Image>().enabled = false;
			content.name = "CONTENT";
			Fold(content);
		}

		[ExposeMethodInEditor]
		public void ConvertToLayoutPanel()
		{
			if (name.Contains(LayoutPanel.spacerName)) name = "Item " + LayoutExt.RandomString(4);
			AddBordersOnly();
			AddTop();
			HandleContent();
			HandleThisObject();
			HandleTopLabel();
			Fold(GetComponent<RectTransform>());
			TryToGetReferences();

#if UNITY_EDITOR
			if (Selection.activeGameObject == gameObject) EditorGUIUtility.PingObject(content);

			if (removeMeWhenDone)
				EditorApplication.delayCall += () => EditorApplication.delayCall += () =>
											   {
												   if (this != null) Undo.DestroyObjectImmediate(this);
											   };
#endif
		}

		[ExposeMethodInEditor]
		public void RemoveMe()
		{
#if UNITY_EDITOR
			Undo.DestroyObjectImmediate(this);
#endif
		}

		private void Fold(Component c)
		{
#if UNITY_EDITOR
			UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
		}

		private Button CreateFoldButton()
		{
			var foldButton = topControl.gameObject.AddImageChild().gameObject;
			if (foldButton == null) return null;

			var button = foldButton.AddOrGetComponent<Button>();

			//Click on the console to see dump for <color=green>btnRect</color> (copy to cliboard and paste in code):
			RectTransform btnRect = foldButton.GetComponent<RectTransform>();
			btnRect.anchorMin = new Vector2(1f, 0.5f);
			btnRect.anchorMax = new Vector2(1f, 0.5f);
			btnRect.anchoredPosition = new Vector2(-3.109955f, 0f);
			btnRect.sizeDelta = new Vector2(10f, 10f);
			btnRect.pivot = new Vector2(1f, 0.5f);

			button.GetComponent<Image>().enabled = false;
			var text = btnRect.gameObject.AddTextChild(LayoutFoldController.labelUnfolded);
			text.alignment = TextAnchor.MiddleCenter;
			var textRect = text.GetComponent<RectTransform>();
			;
			textRect.offsetMax = Vector2.zero;
			textRect.offsetMin = Vector2.zero;
			text.color = textColor;
			textRect.sizeDelta = new Vector2(14, 14);
			text.name = "FoldIndicatorText";
			text.font = font;
			return button;
		}
#if UNITY_EDITOR
		[ExposeMethodInEditor]
		private void RemoveBorders()
		{
			var bh = GetComponent<LayoutBorderHide>();
			if (bh != null)
			{
				bh.borderHideMode = BorderHideMode.Visible;
				Undo.DestroyObjectImmediate(bh);
			}

			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				var thisChild = transform.GetChild(i);
				if (thisChild.GetComponent<LayoutTopControl>() != null ||
					thisChild.GetComponent<LayoutBorderDragger>() != null)
				{
					Undo.DestroyObjectImmediate(thisChild.gameObject);
				}
			}
		}
#endif
	}
}
