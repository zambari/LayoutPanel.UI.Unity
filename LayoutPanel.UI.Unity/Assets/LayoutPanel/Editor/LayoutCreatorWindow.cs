using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

using Z.LayoutPanel;
using Z.LayoutPanel.Editor;

namespace Z.LayoutPanel.EditorCreators
{
	using System.Runtime.Remoting.Messaging;

	using UnityEditor.Graphs;

	using Button = UnityEngine.UIElements.Button;
	using Toggle = UnityEngine.UIElements.Toggle;

	public class LayoutCreatorWindow : EditorWindow
	{
		[MenuItem("Tools/Layout Panel Helper Tool")]
		private static void Init()
		{
			var window = GetWindow<LayoutCreatorWindow>();
			window.titleContent = new GUIContent("Layout Editor");
			window.minSize = new Vector2(400, 500);
		}

		private VisualElement root => rootVisualElement;

		private GameObject gameObject;

		private int currentSplitCouint = 3;

		private LayoutGroupType layoutType;

		public void CreateGUI()
		{
			root.Add(new Label("Welcome to LayoutPanel").Larger());
		}

		void OnSelection(GameObject gameObject)
		{
			this.gameObject = gameObject;
			root.Clear();
			if (gameObject == null)
			{
				root.Add(new Label("Nothing selected"));

				var canvas = GameObject.FindObjectOfType<Canvas>();
				root.AddButton("Select Canvas", () => Selection.activeObject = canvas);
				return;
			}

			root.Add(new Label("Selected" + gameObject.name));
			root.AddButton(
				"Ping " + gameObject.name,
				() => { EditorGUIUtility.PingObject(gameObject); });
			var rect = gameObject.GetComponent<RectTransform>();
			if (gameObject.GetComponent<Canvas>() != null)
			{
				Debug.Log("canvas selected");
				root.AddButton(
					"Create a Panel",
					() =>
					{
						var panel = rect.AddFillingChild();
						Selection.activeObject = panel;
					});
				return;
			}

			if (rect == null) return;

			root.Add(GetImageEnabledToggle(gameObject));
			root.Add(GetLayoutSettings(gameObject));
			root.Add(GetScrollButton(gameObject));
			root.Add(GetCreatePanelButton(gameObject));
			root.Add(GetConvertToPanelButton(gameObject));
		}

		VisualElement GetScrollButton(GameObject target)
		{
			var button = new Button();
			button.clicked += () =>
							  {
								  var scroll = target.CreateScrollView();
								  Selection.activeGameObject = scroll.gameObject;
							  };

			button.text = "Create Scroll";
			return button;
		}

		VisualElement GetConvertToPanelButton(GameObject target)
		{
			var button = new Button();
			button.clicked += () =>
							  {
								  Undo.RecordObject(target, "convert");
								  var panel = target.ConvertToLayoutPanel();
								  Selection.activeGameObject = panel.gameObject;
							  };

			button.text = "Convert to Panel";
			return button;
		}

		VisualElement GetCreatePanelButton(GameObject target)
		{
			var buttons = new VisualElement();
			buttons.style.flexDirection = FlexDirection.Row;
			buttons.Add(GetCreatePanelButton(target, true));
			buttons.Add(GetCreatePanelButton(target, false));
			return buttons;
		}

		VisualElement GetCreatePanelButton(GameObject target, bool scroll)
		{
			var button = new Button();
			button.clicked += () =>
							  {
								  var child = target.GetComponent<RectTransform>().AddChild();
								  child.name = "[LayoutPanel]";
#if UNITY_EDITOR
								  Undo.RegisterCreatedObjectUndo(child.gameObject, "Panel");
#endif
								  var panel = child.ConvertToLayoutPanel(scroll, Color.white);

								  // if (addRandomTexstOnCreate)
								  {
									  panel.content.AddRandomTexts();
									  var contentSize = child.gameObject.AddComponent<ContentSizeFitter>();
									  contentSize.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
								  }

								  Undo.RecordObject(target, "convert");
								  Selection.activeGameObject = panel.gameObject;
							  };

			button.text = "Create child Panel " + (scroll ? "SC" : "No scroll");
			return button;
		}

		VisualElement GetImageEnabledToggle(GameObject target)
		{
			VisualElement cnt = new VisualElement();
			var image = target.GetComponent<UnityEngine.UI.Image>();
			if (image != null)
			{
				var toggle = new Toggle("Image enabled");
				toggle.value = image.enabled;
				toggle.RegisterValueChangedCallback((x) => image.enabled = x.newValue);
				cnt.Add(toggle);
			}

			return cnt;
		}

		void ChangeLayoutType(LayoutGroupType groupType)
		{
			this.layoutType = groupType;
			gameObject.SetLayoutType(groupType, 5);
		}

		VisualElement GetSplitSlider()
		{
			var slider = new SliderInt(2, 10);
			slider.label = "Splid Count:";
			slider.value = currentSplitCouint;
			slider.RegisterValueChangedCallback((x) => currentSplitCouint = x.newValue);
			return slider;
		}

		VisualElement GetLayoutSettings(GameObject source)
		{
			VisualElement content = new VisualElement();
			layoutType = source.GetLayoutGroupType();

			// var currentLayoutType=source.g
			var tabLayoutType = new LayoutTab("Layout type: ", layoutType, (x) => ChangeLayoutType(layoutType));

			// tabLayoutType.value = (int)layoutType;
			content.Add(tabLayoutType);
			content.Add(GetSplitSlider());
			var splitButton = new Button();
			splitButton.text = "Split";
			splitButton.clicked += () =>
								   {
									   var subs = gameObject.SplitToLayout(
										   tabLayoutType.value == (int)LayoutGroupType.HorizontalLayout,
										   currentSplitCouint);
									   Selection.activeGameObject = subs.FirstOrDefault();
								   };

			// }

			content.Add(splitButton);
			return content;
		}

		private void OnSelectionChange()
		{
			OnSelection(Selection.activeGameObject);
		}

		void OnDisable()
		{
			Selection.selectionChanged += OnSelectionChange;
		}

		void OnEnable()
		{
			Selection.selectionChanged -= OnSelectionChange;
		}
	}
}
