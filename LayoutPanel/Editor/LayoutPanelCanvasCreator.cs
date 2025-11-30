namespace zUI.LayoutPanelTools.Editor
{
	using UnityEditor;

	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class LayoutPanelCanvasCreator
	{
		internal static void CreateCanvasLayout()
		{
			if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Canvas>() != null)
			{
				AddToCanvas(Selection.activeGameObject.GetComponentInParent<Canvas>());
			}
			else
			{
				AddToCanvas(GetCanvas());
			}

			Canvas GetCanvas()
			{
				var foundCanvas = Object.FindFirstObjectByType<Canvas>();
				if (foundCanvas != null) return foundCanvas;

				var foundEventSystem = Object.FindFirstObjectByType<EventSystem>();
				if (foundEventSystem == null)
				{
					var eventSystem = new GameObject(
						nameof(EventSystem),
						typeof(EventSystem),
						typeof(StandaloneInputModule));
					
				}

				var canvasGo = new GameObject(
					nameof(Canvas),
					typeof(Canvas),
					typeof(CanvasScaler),
					typeof(GraphicRaycaster));
				var canvas = canvasGo.GetComponent<Canvas>();
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				var rect = canvasGo.GetComponent<RectTransform>();
				if (rect.sizeDelta.x < 300) rect.sizeDelta = new Vector2(400, 300);
				return canvasGo.GetComponent<Canvas>();
			}

			void AddToCanvas(Canvas canvas)
			{
				var child = new GameObject("Layout Panel Root", typeof(RectTransform));
				var rectTransform = child.GetComponent<RectTransform>();
				rectTransform.SetParent(canvas.transform);

				rectTransform.localScale = Vector3.one;
				rectTransform.localPosition = Vector3.zero;
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.offsetMax = Vector2.zero;
				rectTransform.offsetMin = Vector2.zero;
				rectTransform.sizeDelta = Vector2.zero;
				AddToRectTransform(rectTransform);
			}

			void AddToRectTransform(RectTransform rectTransform)
			{
				rectTransform.gameObject.AddComponent<LayoutCreator>();
				var image = rectTransform.gameObject.AddComponent<Image>();
				image.color = Color.red;
				image.enabled = false;
				Selection.activeGameObject = rectTransform.gameObject;
			}
		}
	}
}
