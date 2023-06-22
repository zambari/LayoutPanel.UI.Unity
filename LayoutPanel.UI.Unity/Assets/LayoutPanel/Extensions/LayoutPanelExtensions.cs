namespace Z.LayoutPanel
{
	using UnityEngine;
	using UnityEngine.UI;

	using System;
	using System.Collections.Generic;

	using UnityEditor;

	using Random = UnityEngine.Random;

	public static class LayoutPanelExtensions
	{
		private static bool amendName = false;

		private const int padLeft = 4;

		private static int padRight => padLeft;

		public static LayoutPanel CreatePanel(
			this GameObject parent,
			string newName = "NoName",
			bool hideCreatedBordersInHierarchy = false)
		{
			var thisGrandChild = parent.AddImageChild();
			thisGrandChild.name = newName;

			var creator = thisGrandChild.gameObject.AddComponent<LayoutItemCreator>();
			creator.borderHideMode = hideCreatedBordersInHierarchy
				? LayoutBorderHide.BorderHideMode.Hidden
				: LayoutBorderHide.BorderHideMode.Visible;
			creator.ConvertToLayoutPanel();

			var panel = creator.GetComponent<LayoutPanel>();
			var contentle = panel.layoutElement;
			contentle.preferredHeight = UnityEngine.Random.Range(
				LayoutSetupProvider.setup.topHeight * 2,
				LayoutSetupProvider.setup.topHeight * 5);
			creator.RemoveMe();
			thisGrandChild.transform.SetParent(parent.transform);
			return panel;
		}

		public static void AddLayoutElement(this GameObject target, Vector2 preffered)
		{
			var le = target.AddOrGetComponent<LayoutElement>();
			if (preffered.x > 0) le.preferredWidth = preffered.x;
			else
			{
				le.flexibleWidth = 1;
			}

			if (preffered.y > 0)
			{
				le.preferredHeight = preffered.y;
			}
			else le.flexibleHeight = 1;

			le.minHeight = LayoutSetupProvider.setup.topHeight;
			le.minWidth = 3 * LayoutSetupProvider.setup.topHeight;
		}

		public static void AddColumnBorders(this GameObject target, Color color)
		{
			var a = target.AddImageChild();
			var b = target.AddImageChild();
			a.color = color;
			b.color = color;
			var d = a.gameObject.AddComponent<LayoutBorderDragger>();
			var e = b.gameObject.AddComponent<LayoutBorderDragger>();
			e.columnMode = true;
			d.columnMode = true;
			e.side = Side.Bottom;
			d.side = Side.Top;
		}

		public static void AddGroupComponents(this Transform src, bool horizontal)
		{
			if (horizontal)
			{
				var hg = src.gameObject.AddOrGetComponent<HorizontalLayoutGroup>();

				//   hg.SetParams(null);
			}
			else
			{
				var vg = src.gameObject.AddOrGetComponent<VerticalLayoutGroup>();
				vg.SetParams(null);
			}
		}

		public static Text AddTextChild(this GameObject g, string content = "no text")
		{
			Text text = g.AddChildRectTransform().gameObject.AddComponent<Text>();
			text.text = content;
			return text;
		}

		public static Text AddFoldStatus(this Button button, Color textColor, Font font)
		{
			var btnRect = button.GetComponent<RectTransform>();
			var text = btnRect.gameObject.AddTextChild(LayoutFoldController.labelUnfolded);
			text.name = "FoldIndicatorText";

			// text.font = font;
			text.color = textColor;
			text.alignment = TextAnchor.MiddleCenter;
			var textRect = text.GetComponent<RectTransform>();
			textRect.offsetMax = Vector2.zero;
			textRect.offsetMin = Vector2.zero;
			textRect.sizeDelta = new Vector2(14, 14);

			return text;
		}

		public static Button CreateFoldButton(this RectTransform topControl)
		{
			var foldButton = topControl.gameObject.AddImageChild().gameObject;
			if (foldButton == null) return null;

			foldButton.name = "FoldButton";

			var button = foldButton.AddOrGetComponent<Button>();

			//Click on the console to see dump for <color=green>btnRect</color> (copy to cliboard and paste in code):
			RectTransform btnRect = foldButton.GetComponent<RectTransform>();
			btnRect.anchorMin = new Vector2(1f, 0.5f);
			btnRect.anchorMax = new Vector2(1f, 0.5f);
			btnRect.anchoredPosition = new Vector2(-3.109955f, 0f);
			btnRect.sizeDelta = new Vector2(10f, 10f);
			btnRect.pivot = new Vector2(1f, 0.5f);

			button.GetComponent<Image>().enabled = false;

			return button;
		}

		public static void FoldComponent(this Component c)
		{
#if UNITY_EDITOR
			UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
		}

		private static void SetColors(this List<LayoutPanel> panels, Color startColor, Color endColor)
		{
			for (int i = 0; i < panels.Count; i++)
			{
				var bh = panels[i].GetComponent<LayoutBorderHide>();

				// bh.borderColor = Color.Lerp(startColor, endColor, (float) i / panels.Count);
				// bh.editColors = true;
				// bh.editColors = false;
				var img = panels[i].GetComponent<Image>();
				img.color = Color.Lerp(startColor, endColor, 1 - (float)i / panels.Count);
			}
		}

		public static List<T> AddComponentsToAllGameObjects<T>(this List<GameObject> target) where T : Component
		{
			var list = new List<T>();
			for (int i = 0; i < target.Count; i++)
			{
				var thisChild = target[i];
				list.Add(thisChild.AddOrGetComponent<T>());
			}
#if UNITY_EDITOR
			UnityEditor.Selection.objects = target.ToArray();
#endif
			return list;
		}

		public static List<T> AddComponentsToAllComponents<T>(this List<Component> target) where T : Component
		{
			var list = new List<T>();
			for (int i = 0; i < target.Count; i++)
			{
				var thisChild = target[i];
				list.Add(thisChild.gameObject.AddOrGetComponent<T>());
			}
#if UNITY_EDITOR
			UnityEditor.Selection.objects = target.ToArray();
#endif
			return list;
		}

		public static List<GameObject> AddBorders(
			this Component src,
			LayoutBorderHide.BorderHideMode borderHideMode = LayoutBorderHide.BorderHideMode.Visible)
		{
			return src.GetComponent<RectTransform>().AddBorders();
		}

		public static List<GameObject> AddBorders(
			this RectTransform frame,
			LayoutBorderHide.BorderHideMode borderHideMode = LayoutBorderHide.BorderHideMode.Visible)
		{
			var list = new List<GameObject>();

			int count = System.Enum.GetNames(typeof(Side)).Length;
			for (int i = 0; i <= 3; i++)
			{
				var thisChild = frame.AddImageChild();

				// thisChild.transform.SetAsFirstSibling();
				var d = thisChild.gameObject.AddOrGetComponent<LayoutBorderDragger>();
				list.Add(thisChild.gameObject);
				var side = (Side)i;
				d.side = side;

				// Debug.Log($" for {i} {side} the name is {d.name}", thisChild.gameObject);
#if UNITY_EDITOR
				UnityEditor.Undo.RegisterCreatedObjectUndo(thisChild.gameObject, "borders");
#endif
			}

			return list;
		}

		public static LayoutTopControl AddTop(this RectTransform src, Color topColor)
		{
			var topControl = src.GetComponentInChildren<LayoutTopControl>();
			if (topControl == null)
			{
				var topImage = src.AddImageChild();
				topImage.name = "Header";

				//if (layoutBorderHide != null) topImage.color =Layr.Randomize ();
				topControl = topImage.gameObject.AddOrGetComponent<LayoutTopControl>();
				topImage.color = topColor;

#if UNITY_EDITOR
				UnityEditor.Undo.RegisterCreatedObjectUndo(topControl.gameObject, "top");
#endif
			}

			topControl.transform.SetAsFirstSibling();

			return topControl;
		}

		public static LayoutPanel ConvertToLayoutPanel(this GameObject obj)
		{
			return obj.GetComponent<RectTransform>().ConvertToLayoutPanel();
		}

		public static LayoutPanel CreateLayoutPanel(this RectTransform rect)
		{
			float w = rect.rect.width;
			float h = rect.rect.height;
			rect.anchorMin = Vector2.one / 2;
			rect.anchorMax = Vector2.one / 2;
			rect.sizeDelta = new Vector2(h, w);
			var le = rect.gameObject.AddOrGetComponent<LayoutElement>();
			le.minHeight = LayoutSetupProvider.setup.topHeight;
			le.preferredWidth = w;

			// content.minWidth = 26;
			le.flexibleWidth = 0.001f;

			var panel = rect.gameObject.AddOrGetComponent<LayoutPanel>();

			var hg = rect.gameObject.GetComponent<HorizontalLayoutGroup>();
			if (hg != null) GameObject.DestroyImmediate(hg);
			VerticalLayoutGroup verticalLayout = rect.gameObject.AddOrGetComponent<VerticalLayoutGroup>();
			var pad = verticalLayout.padding;
			pad.top = LayoutSetupProvider.setup.topHeight;
			pad.left = padLeft;
			pad.right = padRight;
			verticalLayout.padding = pad;
			verticalLayout.SetChildControl(2);
			verticalLayout.FoldComponent();

			return panel;
		}

		static Text CreateTopLabel(RectTransform topControl, string name, Color textColor, Font font)
		{
			Text labelText = null;
			if (labelText == null)
			{
				labelText = topControl.gameObject.AddTextChild();
				labelText.name = "Panel Label";
				labelText.text = name;

				//	labelText.font = font;
				labelText.alignment = TextAnchor.MiddleLeft;
				labelText.color = textColor;
				var textRect = labelText.GetComponent<RectTransform>();

				textRect.anchorMin = new Vector2(0f, 0f);
				textRect.anchorMax = new Vector2(1f, 1f);
				textRect.anchoredPosition = new Vector2(-7.5f, 0f);
				textRect.sizeDelta = new Vector2(-25f, 4f);
				textRect.pivot = new Vector2(0.5f, 0.5f);
			}

			return labelText;
		}

		// public static Font defaultFont
		// {
		// 	get { return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); }
		// }

		public static LayoutPanel ConvertToLayoutPanel(this Component component)
		{
			return component.GetComponent<RectTransform>().ConvertToLayoutPanel(Color.white, null);
		}

		public static Text HandleTextRaycastCatchers(GameObject a, Color textColor, float margin, Font font = null)
		{
			var text = a.gameObject.AddTextChild();
			text.text = null;
			text.supportRichText = false;
			text.GetComponent<RectTransform>().sizeDelta = new Vector2(2 * margin, 2 * margin);
			text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			text.name = "raycast catcher";

			// if (font == null) font = defaultFont;
			// text.font = font;
			text.color = textColor;
			return text;
		}

		public static RectTransform AddFillingChild(
			this RectTransform parentRect,
			int margin = 5,
			string name = "unnamed")
		{
			GameObject go = new GameObject(name, typeof(RectTransform));
			RectTransform rect = go.GetComponent<RectTransform>();
			go.transform.SetParent(parentRect);

			//rect.setan
			rect.anchorMin = new Vector2(0, 0);
			rect.anchorMax = new Vector2(1, 1);

			//rect.sizeDelta = new Vector2(10, 10);
			rect.offsetMin = new Vector2(margin, margin);
			rect.offsetMax = new Vector2(-margin, -margin);
			rect.localPosition = Vector2.zero;
			rect.localScale = Vector3.one;
			return rect;
		}

		public static LayoutPanel ConvertToLayoutPanel(this RectTransform rect, Color textColor, Font font = null)
		{
			var frame = rect.AddFillingChild(0, "Frame");
			frame.AddIgnoreLayoutElement();

			var topControl = frame.AddTop(Color.black * .4f);
			var toprect = topControl.GetComponent<RectTransform>();
			var layoutPanel = CreateLayoutPanel(rect);
			var labelText = CreateTopLabel(topControl.GetComponent<RectTransform>(), rect.name, textColor, font);
			layoutPanel.text = labelText;

			var button = toprect.CreateFoldButton();
			var foldStatusText = button.AddFoldStatus(textColor, font);

			var foldController = topControl.gameObject.AddOrGetComponent<SimpleFoldHelper>();
			foldController.foldButton = button;
			foldController._foldLabelText = foldStatusText;
			layoutPanel.foldControl = foldController;
			foldController.FoldComponent();
			rect.FoldComponent();
			frame.AddBorders(LayoutBorderHide.BorderHideMode.Visible);
			var contentSizeFitter = rect.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			return layoutPanel;
		}

		public static void AddRandomTexts(this RectTransform rect)
		{
			for (int i = 0; i < Random.Range(3, 9); i++)
			{
				var t = rect.gameObject.AddTextChild();
				t.gameObject.name = "_some_content_" + i;
				string k = "";
				for (int j = 0; j < Random.Range(3, 6); j++) k += (Random.Range(3, 8)) + " ";
				t.text = k;
				t.fontSize = Random.Range(6, 16);
				t.color = new Color(Random.value, Random.value, Random.value);
			}
		}

		internal static void AmendName(GameObject where)
		{
			if (!amendName) return;

			var nameHelper = where.AddOrGetComponent<LayoutNameHelper>();
			nameHelper.UpdateName();
		}

		public static List<T> CreateHorizontalLayoutWithComponents<T>(this GameObject where, int thisSplitCount)
			where T : Component
		{
			var vg = where.GetComponent<VerticalLayoutGroup>();
			if (vg != null) GameObject.DestroyImmediate(vg);
			var list = AddComponentsToAllGameObjects<T>(
				where.GetComponent<RectTransform>().SplitToLayout(true, thisSplitCount));
			AmendName(where);
			return list;
		}

		public static List<T> CreateVerticalLayoutWithComponents<T>(this GameObject where, int thisSplitCount)
			where T : Component
		{
			var hg = where.GetComponent<HorizontalLayoutGroup>();
			if (hg != null) GameObject.DestroyImmediate(hg);
			var list = AddComponentsToAllGameObjects<T>(
				where.GetComponent<RectTransform>().SplitToLayout(false, thisSplitCount));

			AmendName(where);
			return list;
		}
	}
}
