namespace zUI.LayoutPanelTools.Editor
{
	using UnityEditor;

	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Menu items for Layout Panel
	/// </summary>
	public class LayoutCreatorMenu
	{
		private const string LayoutMenuRoot = ToolsMenu + "LayoutPanel/";

		private const string GameObjectMenu = "GameObject/UI/";

		private const string ToolsMenu = "Tools/";

		[MenuItem(LayoutMenuRoot + "Add LayoutPanel")]
		private static void CreateLayoutPanelCanvas()
		{
			LayoutPanelCanvasCreator.CreateCanvasLayout();
		}

		[MenuItem(GameObjectMenu + "Add LayoutPanel Child Item")]
		private static RectTransform CreatePanelChildLayout()
		{
			if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<RectTransform>() == null)
			{
				Debug.Log("Please select a RectTransform first");
				return null;
			}

			RectTransform rect = CreatePanelChild();
			if (rect != null) Selection.activeGameObject = rect.gameObject;
			Undo.RegisterCreatedObjectUndo(rect.gameObject, "Create object");
			rect.gameObject.AddComponent<LayoutItemCreator>().ConvertToLayoutPanel();
			rect.gameObject.AddComponent<LayoutItemCreator>().RemoveMe();
			return rect;
		}

		[MenuItem(GameObjectMenu + "Add LayoutPanel Child Item", true)]
		private static bool CheckIfMainMethodIsValid()
		{
			return (Selection.activeGameObject != null &&
					Selection.activeGameObject.GetComponent<RectTransform>() == null);
		}

		[MenuItem(LayoutMenuRoot + "Layout H<->V Conversion", true)]
		private static bool CheckConvertLayout()
		{
			return (Selection.activeGameObject != null &&
					(Selection.activeGameObject.GetComponentInChildren<VerticalLayoutGroup>() != null ||
					 Selection.activeGameObject.GetComponent<HorizontalLayoutGroup>()));
		}

		[MenuItem(LayoutMenuRoot + "Layout H<->V Conversion")]
		private static void ConvertLayout()
		{
			if (Selection.activeGameObject == null)
			{
				Debug.Log("nothing selected");
				return;
			}

			VerticalLayoutGroup vg = Selection.activeGameObject.GetComponentInChildren<VerticalLayoutGroup>();
			HorizontalLayoutGroup hg = Selection.activeGameObject.GetComponent<HorizontalLayoutGroup>();

			if (vg == null && hg == null)
			{
				Debug.Log(" no layout group");
				return;
			}

			if (vg != null)
			{
				vg.ToHorizontal();
			}
			else hg.ToVeritical();
		}

		[MenuItem(LayoutMenuRoot + "Layout Horizontal Layuout from selected objects", true)]
		[MenuItem(LayoutMenuRoot + "Layout group Vertical from Selected", true)]
		private static bool CheckIfGroupToLayouVt()
		{
			return Selection.activeGameObject != null;
		}

		[MenuItem(LayoutMenuRoot + "Layout group Vertical from Selected")]
		private static void GroupToLayouVt()
		{
			GameObject g = Selection.activeGameObject;
			Undo.RecordObject(g, "Layout");
			if (g == null) return;

			RectTransform rect = LayoutEditorUtilities.CreateGroup();
			if (rect != null)
			{
				VerticalLayoutGroup layout = rect.gameObject.AddComponent<VerticalLayoutGroup>();
				layout.SetMargin(LayoutEditorUtilities.defaultSpacing);
				layout.SetChildControl(LayoutEditorUtilities.defaultSpacing);
				rect.AddContentSizeFitter();
			}
		}

		[MenuItem(LayoutMenuRoot + "Layout Horizontal Layuout from selected objects")]
		private static void GroupToLayoutH()
		{
			GameObject g = Selection.activeGameObject;
			if (g == null) return;

			RectTransform rect = LayoutEditorUtilities.CreateGroup();
			if (rect != null)
			{
				HorizontalLayoutGroup layout = rect.gameObject.AddComponent<HorizontalLayoutGroup>();
				layout.SetMargin(LayoutEditorUtilities.defaultSpacing);
				layout.SetChildControl(LayoutEditorUtilities.defaultSpacing);
				rect.AddContentSizeFitter();
			}
		}

		[MenuItem(GameObjectMenu + "Add Horizontal Layout to selected RectTransform")]
		public static void CreateHorizontalLayout()
		{
			Selection.activeObject = LayoutEditorUtilities.CreateHoritontalOrVertical(
				Selection.activeGameObject.GetComponent<RectTransform>(),
				LayoutDirection.Horizontal,
				3);
		}

		[MenuItem(GameObjectMenu + "Add Vertical Layout to selected RectTransform")]
		public static void CreateVerticalLayout()
		{
			Selection.activeObject = LayoutEditorUtilities.CreateHoritontalOrVertical(
				Selection.activeGameObject.GetComponent<RectTransform>(),
				LayoutDirection.Vertical,
				3);
		}

		[MenuItem(GameObjectMenu + "Add Layout Spacer")]
		public static void AddFlexibleSpacer()
		{
			var img = Selection.activeGameObject.AddChildRectTransform();
			var le = img.gameObject.AddComponent<LayoutElement>();
			le.flexibleHeight = 1;
			le.flexibleWidth = 1;
			le.name = LayoutPanel.spacerName;

			Undo.RegisterCreatedObjectUndo(le.gameObject, "SPACER");
			Selection.activeGameObject = le.gameObject;
		}

		private static RectTransform CreatePanelChild()
		{
			GameObject go = new GameObject("Panel");
			if (Selection.activeGameObject != null) go.transform.SetParent(Selection.activeGameObject.transform);
			go.transform.localPosition = Vector2.zero;
			go.transform.localScale = Vector3.one;
			RectTransform rect = go.AddComponent<RectTransform>();
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.sizeDelta = Vector2.zero;
			Image image = go.AddComponent<Image>();
			image.color = new Color(0, 0, 0, 0.2f);
			image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			image.type = Image.Type.Sliced;
			rect.localScale = Vector3.one;

			Undo.RegisterCreatedObjectUndo(go, "Create object");
			return rect;
		}
	}
}
