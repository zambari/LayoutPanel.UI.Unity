using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Z.LayoutPanel;

// this class has been  automatically generated using UIDumpParams.cs tool

public class ConvertToScrollTest : MonoBehaviour
{
	// Start is called before the first frame update

	public RectTransform target;

	// [ExposeMethodInEditor]
	void TestCreate()
	{
		var scrollbarbgColor = new Color(1f, 1f, 1f, .3f);
		var scrollbarHandleColor = new Color(0f, 1f, .5f, .8f);
		float handleSize = 10;

		// GAMEOBJECT  [0: Scroll View (1)] 

		var scrollview1 = new GameObject(
			"Scroll View",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Image),
			typeof(UnityEngine.UI.ScrollRect),
			typeof(UnityEngine.UI.LayoutElement));
		scrollview1.transform.SetParent(target);

		// Image 
		var scrollview1_image = scrollview1.GetComponent<Image>();
		scrollview1_image.color = new Color(0f, 0f, 0f, 0.1137255f);
		scrollview1_image.type = Image.Type.Sliced;

		// RectTransform 
		var rectTransform_scrollview1 = scrollview1.GetComponent<RectTransform>();
		rectTransform_scrollview1.anchorMin = new Vector2(0f, 0f);
		rectTransform_scrollview1.anchorMax = new Vector2(1f, 1f);
		rectTransform_scrollview1.offsetMin = new Vector2(0, 0);
		rectTransform_scrollview1.offsetMax = new Vector2(0, 0);

		// LayoutElement 
		var scrollview1_LayoutElement = scrollview1.GetComponent<LayoutElement>();
		scrollview1_LayoutElement.preferredHeight = 168.4f;
		scrollview1_LayoutElement.flexibleWidth = 0.1f;
		scrollview1_LayoutElement.flexibleHeight = 0.1f;
		var scrollview1_scrollRect = scrollview1.GetComponent<ScrollRect>();

		rectTransform_scrollview1.anchoredPosition = Vector2.zero;

		// GAMEOBJECT  [1: Viewport] 

		var viewport = new GameObject(
			"Viewport",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Image),
			typeof(UnityEngine.UI.Mask));

		viewport.transform.SetParent(scrollview1.transform);

		// Image 
		var viewport_image = viewport.GetComponent<Image>();
		viewport_image.color = new Color(0f, 0f, 0f, 1f);
		viewport_image.type = Image.Type.Sliced;

		// RectTransform 
		var rectTransform_viewport = viewport.GetComponent<RectTransform>();
		rectTransform_viewport.anchorMin = new Vector2(0f, 0f);
		rectTransform_viewport.anchorMax = new Vector2(1f, 1f);
		rectTransform_viewport.offsetMin = new Vector2(0f, 0f);
		rectTransform_viewport.offsetMax = new Vector2(-handleSize, 0f);
		rectTransform_viewport.anchoredPosition = new Vector2(0f, 0f);
		rectTransform_viewport.pivot = new Vector2(0f, 1f);

		// GAMEOBJECT  [2: Content] 

		var content = new GameObject(
			"Content",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.UI.VerticalLayoutGroup),
			typeof(UnityEngine.UI.ContentSizeFitter));

		content.transform.SetParent(viewport.transform);
		content.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

		// RectTransform 
		var rectTransform_content = content.GetComponent<RectTransform>();
		rectTransform_content.anchorMin = new Vector2(0f, 1f);
		rectTransform_content.anchorMax = new Vector2(1f, 1f);
		rectTransform_content.offsetMin = new Vector2(0f, -250f);
		rectTransform_content.offsetMax = new Vector2(0f, 0f);
		rectTransform_content.anchoredPosition = new Vector2(0f, 0f);
		rectTransform_content.pivot = new Vector2(0f, 1f);
		var vertlayout = content.GetComponent<VerticalLayoutGroup>();
		vertlayout.childControlHeight = true;
		vertlayout.childForceExpandHeight = false;

		// GAMEOBJECT  [12: Scrollbar Vertical] 

		var scrollbarvertical = new GameObject(
			"Scrollbar Vertical",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Image),
			typeof(UnityEngine.UI.Scrollbar));
		var scrollRect_scrollbarvertical = scrollbarvertical.GetComponent<Scrollbar>();
		scrollRect_scrollbarvertical.direction = Scrollbar.Direction.BottomToTop;

		scrollbarvertical.transform.SetParent(scrollview1.transform);

		// Image SCROLLBAR BG
		var scrollbarvertical_image = scrollbarvertical.GetComponent<Image>();
		scrollbarvertical_image.color = scrollbarbgColor;
		scrollbarvertical_image.sprite = LayoutResourceLoader.scrollbarBg;
		scrollbarvertical_image.type = Image.Type.Tiled;
		scrollbarvertical_image.pixelsPerUnitMultiplier = 0.5f;

		// RectTransform 
		var rectTransform_scrollbarvertical = scrollbarvertical.GetComponent<RectTransform>();
		rectTransform_scrollbarvertical.anchorMin = new Vector2(1f, 0f);
		rectTransform_scrollbarvertical.anchorMax = new Vector2(1f, 1f);
		rectTransform_scrollbarvertical.offsetMin = new Vector2(-handleSize, 0f);
		rectTransform_scrollbarvertical.offsetMax = new Vector2(0f, 0f);
		rectTransform_scrollbarvertical.anchoredPosition = new Vector2(0f, 0f);
		rectTransform_scrollbarvertical.pivot = new Vector2(1f, 1f);

		// Scrollbar 

		// GAMEOBJECT  [13: Sliding Area] 

		var slidingarea = new GameObject("Sliding Area", typeof(UnityEngine.RectTransform));

		slidingarea.transform.SetParent(scrollbarvertical.transform);

		// RectTransform 
		var rectTransform_slidingarea = slidingarea.GetComponent<RectTransform>();
		rectTransform_slidingarea.anchorMin = new Vector2(0f, 0f);
		rectTransform_slidingarea.anchorMax = new Vector2(1f, 1f);
		rectTransform_slidingarea.offsetMin = new Vector2(0f, 0f);
		rectTransform_slidingarea.offsetMax = new Vector2(0f, 0f);
		rectTransform_slidingarea.anchoredPosition = new Vector2(0f, 0f);

		// GAMEOBJECT  [14: Handle] 

		var handle = new GameObject(
			"Handle",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Image));

		handle.transform.SetParent(slidingarea.transform);

		// Image 
		var handle_image = handle.GetComponent<Image>();
		handle_image.color = scrollbarHandleColor;
		handle_image.type = Image.Type.Sliced;

		// RectTransform 
		var rectTransform_handle = handle.GetComponent<RectTransform>();
		rectTransform_handle.anchorMin = new Vector2(0f, 0);
		rectTransform_handle.anchorMax = new Vector2(1f, 1f);
		rectTransform_handle.offsetMin = new Vector2(0f, 0f);
		rectTransform_handle.offsetMax = new Vector2(0f, 0f);
		rectTransform_handle.anchoredPosition = new Vector2(0f, 0f);

		scrollRect_scrollbarvertical.handleRect = rectTransform_handle;
		scrollview1_scrollRect.content = rectTransform_content;
		scrollview1_scrollRect.verticalScrollbar = scrollRect_scrollbarvertical;

		AddSomeContent(content.transform);
	}

	void AddSomeContent(Transform content)
	{
		// GAMEOBJECT  [3: _some_content_0] 

		var _some_content_0 = new GameObject(
			"_some_content_0",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Text));

		_some_content_0.transform.SetParent(content.transform);

		// RectTransform 
		var rectTransform__some_content_0 = _some_content_0.GetComponent<RectTransform>();
		rectTransform__some_content_0.anchorMin = new Vector2(0f, 1f);
		rectTransform__some_content_0.anchorMax = new Vector2(0f, 1f);
		rectTransform__some_content_0.offsetMin = new Vector2(0f, -26f);
		rectTransform__some_content_0.offsetMax = new Vector2(95f, 0f);
		rectTransform__some_content_0.anchoredPosition = new Vector2(47.5f, -13f);

		// Text 
		var text__some_content_0 = _some_content_0.GetComponent<Text>();
		text__some_content_0.text = "7 3 3 5 5 ";
		text__some_content_0.fontSize = 43;
		text__some_content_0.color = new Color(0.02542818f, 0.6284005f, 0.4058201f, 1f);
		text__some_content_0.alignment = TextAnchor.UpperLeft;

		// GAMEOBJECT  [4: _some_content_1] 

		var _some_content_1 = new GameObject(
			"_some_content_1",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Text));

		_some_content_1.transform.SetParent(content.transform);

		// RectTransform 
		var rectTransform__some_content_1 = _some_content_1.GetComponent<RectTransform>();
		rectTransform__some_content_1.anchorMin = new Vector2(0f, 1f);
		rectTransform__some_content_1.anchorMax = new Vector2(0f, 1f);
		rectTransform__some_content_1.offsetMin = new Vector2(0f, -54f);
		rectTransform__some_content_1.offsetMax = new Vector2(57f, -28f);
		rectTransform__some_content_1.anchoredPosition = new Vector2(28.5f, -41f);

		// Text 
		var text__some_content_1 = _some_content_1.GetComponent<Text>();
		text__some_content_1.text = "7 5 7 ";
		text__some_content_1.fontSize = 43;
		text__some_content_1.color = new Color(0.6711648f, 0.9906445f, 0.7094344f, 1f);
		text__some_content_1.alignment = TextAnchor.UpperLeft;

		// GAMEOBJECT  [5: _some_content_2] 

		var _some_content_2 = new GameObject(
			"_some_content_2",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Text));

		_some_content_2.transform.SetParent(content.transform);

		// RectTransform 
		var rectTransform__some_content_2 = _some_content_2.GetComponent<RectTransform>();
		rectTransform__some_content_2.anchorMin = new Vector2(0f, 1f);
		rectTransform__some_content_2.anchorMax = new Vector2(0f, 1f);
		rectTransform__some_content_2.offsetMin = new Vector2(0f, -82f);
		rectTransform__some_content_2.offsetMax = new Vector2(57f, -56f);
		rectTransform__some_content_2.anchoredPosition = new Vector2(28.5f, -69f);

		// Text 
		var text__some_content_2 = _some_content_2.GetComponent<Text>();
		text__some_content_2.text = "7 5 7 ";
		text__some_content_2.fontSize = 23;
		text__some_content_2.color = new Color(0.707297f, 0.4310536f, 0.933858f, 1f);
		text__some_content_2.alignment = TextAnchor.UpperLeft;

		// GAMEOBJECT  [6: _some_content_3] 

		var _some_content_3 = new GameObject(
			"_some_content_3",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Text));

		_some_content_3.transform.SetParent(content.transform);

		// RectTransform 
		var rectTransform__some_content_3 = _some_content_3.GetComponent<RectTransform>();
		rectTransform__some_content_3.anchorMin = new Vector2(0f, 1f);
		rectTransform__some_content_3.anchorMax = new Vector2(0f, 1f);
		rectTransform__some_content_3.offsetMin = new Vector2(0f, -110f);
		rectTransform__some_content_3.offsetMax = new Vector2(57f, -84f);
		rectTransform__some_content_3.anchoredPosition = new Vector2(28.5f, -97f);

		// Text 
		var text__some_content_3 = _some_content_3.GetComponent<Text>();
		text__some_content_3.text = "7 6 4 ";
		text__some_content_3.fontSize = 23;
		text__some_content_3.color = new Color(0.119211f, 0.2509667f, 0.5752056f, 1f);
		text__some_content_3.alignment = TextAnchor.UpperLeft;

		// GAMEOBJECT  [7: _some_content_2 (1)] 

		var _some_content_21 = new GameObject(
			"_some_content_2 (1)",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Text));

		_some_content_21.transform.SetParent(content.transform);

		// RectTransform 
		var rectTransform__some_content_21 = _some_content_21.GetComponent<RectTransform>();
		rectTransform__some_content_21.anchorMin = new Vector2(0f, 1f);
		rectTransform__some_content_21.anchorMax = new Vector2(0f, 1f);
		rectTransform__some_content_21.offsetMin = new Vector2(0f, -138f);
		rectTransform__some_content_21.offsetMax = new Vector2(57f, -112f);
		rectTransform__some_content_21.anchoredPosition = new Vector2(28.5f, -125f);

		// Text 
		var text__some_content_21 = _some_content_21.GetComponent<Text>();
		text__some_content_21.text = "7 5 7 ";
		text__some_content_21.fontSize = 23;
		text__some_content_21.color = new Color(0.707297f, 0.4310536f, 0.933858f, 1f);
		text__some_content_21.alignment = TextAnchor.UpperLeft;

		// GAMEOBJECT  [8: _some_content_2 (2)] 

		var _some_content_22 = new GameObject(
			"_some_content_2 (2)",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Text));

		_some_content_22.transform.SetParent(content.transform);

		// RectTransform 
		var rectTransform__some_content_22 = _some_content_22.GetComponent<RectTransform>();
		rectTransform__some_content_22.anchorMin = new Vector2(0f, 1f);
		rectTransform__some_content_22.anchorMax = new Vector2(0f, 1f);
		rectTransform__some_content_22.offsetMin = new Vector2(0f, -166f);
		rectTransform__some_content_22.offsetMax = new Vector2(57f, -140f);
		rectTransform__some_content_22.anchoredPosition = new Vector2(28.5f, -153f);

		// Text 
		var text__some_content_22 = _some_content_22.GetComponent<Text>();
		text__some_content_22.text = "7 5 7 ";
		text__some_content_22.fontSize = 23;
		text__some_content_22.color = new Color(0.707297f, 0.4310536f, 0.933858f, 1f);
		text__some_content_22.alignment = TextAnchor.UpperLeft;

		// GAMEOBJECT  [9: _some_content_0 (1)] 

		var _some_content_01 = new GameObject(
			"_some_content_0 (1)",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Text));

		_some_content_01.transform.SetParent(content.transform);

		// RectTransform 
		var rectTransform__some_content_01 = _some_content_01.GetComponent<RectTransform>();
		rectTransform__some_content_01.anchorMin = new Vector2(0f, 1f);
		rectTransform__some_content_01.anchorMax = new Vector2(0f, 1f);
		rectTransform__some_content_01.offsetMin = new Vector2(0f, -194f);
		rectTransform__some_content_01.offsetMax = new Vector2(95f, -168f);
		rectTransform__some_content_01.anchoredPosition = new Vector2(47.5f, -181f);

		// Text 
		var text__some_content_01 = _some_content_01.GetComponent<Text>();
		text__some_content_01.text = "7 3 3 5 5 ";
		text__some_content_01.fontSize = 23;
		text__some_content_01.color = new Color(0.02542818f, 0.6284005f, 0.4058201f, 1f);
		text__some_content_01.alignment = TextAnchor.UpperLeft;

		// GAMEOBJECT  [10: _some_content_0 (2)] 

		var _some_content_02 = new GameObject(
			"_some_content_0 (2)",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Text));

		_some_content_02.transform.SetParent(content.transform);

		// RectTransform 
		var rectTransform__some_content_02 = _some_content_02.GetComponent<RectTransform>();
		rectTransform__some_content_02.anchorMin = new Vector2(0f, 1f);
		rectTransform__some_content_02.anchorMax = new Vector2(0f, 1f);
		rectTransform__some_content_02.offsetMin = new Vector2(0f, -222f);
		rectTransform__some_content_02.offsetMax = new Vector2(95f, -196f);
		rectTransform__some_content_02.anchoredPosition = new Vector2(47.5f, -209f);

		// Text 
		var text__some_content_02 = _some_content_02.GetComponent<Text>();
		text__some_content_02.text = "7 3 3 5 5 ";
		text__some_content_02.fontSize = 23;
		text__some_content_02.color = new Color(0.02542818f, 0.6284005f, 0.4058201f, 1f);
		text__some_content_02.alignment = TextAnchor.UpperLeft;

		// GAMEOBJECT  [11: _some_content_0 (3)] 

		var _some_content_03 = new GameObject(
			"_some_content_0 (3)",
			typeof(UnityEngine.RectTransform),
			typeof(UnityEngine.CanvasRenderer),
			typeof(UnityEngine.UI.Text));

		_some_content_03.transform.SetParent(content.transform);

		// RectTransform 
		var rectTransform__some_content_03 = _some_content_03.GetComponent<RectTransform>();
		rectTransform__some_content_03.anchorMin = new Vector2(0f, 1f);
		rectTransform__some_content_03.anchorMax = new Vector2(0f, 1f);
		rectTransform__some_content_03.offsetMin = new Vector2(0f, -250f);
		rectTransform__some_content_03.offsetMax = new Vector2(95f, -224f);
		rectTransform__some_content_03.anchoredPosition = new Vector2(47.5f, -237f);

		// Text 
		var text__some_content_03 = _some_content_03.GetComponent<Text>();
		text__some_content_03.text = "7 3 3 5 5 ";
		text__some_content_03.fontSize = 23;
		text__some_content_03.color = new Color(0.02542818f, 0.6284005f, 0.4058201f, 1f);
		text__some_content_03.alignment = TextAnchor.UpperLeft;
	}
}
/*/-*/
