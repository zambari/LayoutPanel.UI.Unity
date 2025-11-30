namespace zUI.LayoutPanelTools
{
	using System.Collections.Generic;

	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;
#if UNITY_EDITOR
	using UnityEditor;
#endif

	// v0.2  duzo dobrego
	// v0.3  napespace
	// v.0.4 undo??
	// v.0.5 scale fix
	// v.0.6 incorporeated into layout panel
	public static class LayoutEditorUtilities
	{
		public const int defaultSpacing = 5;

		private static Transform CreateCanvasIfNotPresent()
		{
			//  if (Selection.activeGameObject != null || Selection.activeGameObject.GetComponentInParent<Canvas>() != null) return Selection.activeGameObject.transform; //GetComponentInParent<Canvas>().transform;
#if UNITY_6000_1_OR_NEWER
			Canvas can = Object.FindFirstObjectByType(typeof(Canvas)) as Canvas;
#else
			Canvas can = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
#endif
			if (can != null) return can.transform;
			else
			{
				GameObject c = new GameObject("Canvas", typeof(Canvas), typeof(GraphicRaycaster), typeof(CanvasScaler));
#if UNITY_EDITOR
				Undo.RegisterCreatedObjectUndo(c, "layouyt");
#endif
				c.AddComponent<Canvas>();
				c.AddComponent<GraphicRaycaster>();
				c.AddComponent<CanvasScaler>();

#if UNITY_6000_1_OR_NEWER
				EventSystem e = Object.FindFirstObjectByType<EventSystem>();
#else
				EventSystem e = GameObject.FindObjectOfType<EventSystem>();
#endif
				if (e == null)
				{
					new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
				}

				return c.transform;
			}
		}

		public static RectTransform CreateHoritontalOrVertical(
			RectTransform target,
			LayoutDirection dir,
			int count,
			float flex = -2)
		{
			if (target == null)
#if UNITY_EDITOR

				if (Selection.activeGameObject != null &&
					Selection.activeGameObject.GetComponent<RectTransform>() != null)
				{
					target = Selection.activeGameObject.GetComponent<RectTransform>();
					Undo.RecordObject(target, "adding layout 1");
					Undo.RecordObject(Selection.activeGameObject, "Adding layout");
				}
#else
            return null;

#endif

			return PopulateLayout(target.AddImageChild().GetComponent<RectTransform>(), dir, count, flex);
		}

		private static RectTransform PopulateLayout(
			RectTransform container,
			LayoutDirection dir,
			int count,
			float flex = -2)
		{
			bool vertical = dir == LayoutDirection.Vertical;

			if (vertical)
			{
				var group = container.gameObject.AddComponent<VerticalLayoutGroup>();
				group.SetChildControl();
				group.spacing = 10;
			}
			else
			{
				var group = container.gameObject.AddComponent<HorizontalLayoutGroup>();
				group.SetChildControl();
				group.spacing = 10;
			}

			List<GameObject> cretedObjects = new List<GameObject>();
			for (int i = 0; i < count; i++)
			{
				RectTransform child = container.AddChild();

#if UNITY_EDITOR

				Undo.RegisterCreatedObjectUndo(child.gameObject, "Layout");
#endif
				cretedObjects.Add(child.gameObject);
				child.anchorMin = new Vector2(0, 0);
				child.anchorMax = new Vector2(1, 1);
				child.offsetMin = new Vector2(0, 0);
				child.offsetMax = new Vector2(0, 0);
				Image im = child.gameObject.AddComponent<Image>();
				im.color = im.color.Random();
				child.name = "Item " + (i + 1);
				LayoutElement le = child.gameObject.AddComponent<LayoutElement>();
				if (flex == -2)
				{
					le.flexibleHeight = (vertical ? 1f / count : 1);
					le.flexibleWidth = (vertical ? 1 : 1f / count);
				}
				else
				{
					le.flexibleHeight = flex;
					le.flexibleWidth = flex;
				}

				child.localScale = Vector3.one; 
			}

			container.name = (vertical ? "VerticalLayout" : "HorizontalLayout");
			return container;
		}

		public static RectTransform CreateGroup()
		{
#if UNITY_EDITOR
			if (Selection.activeGameObject == null)
			{
				Debug.Log("nothing selected");
				return null;
			}

			if (Selection.activeGameObject.transform.parent == null)
			{
				Debug.Log("no parent ");
				return null;
			}

			RectTransform rect = CreaatePanelChild();
			rect.anchorMin = Vector2.one / 2;
			rect.anchorMax = Vector2.one / 2;
			rect.SetParent(Selection.activeGameObject.transform.parent);

			// Debug.Log("pareting "+rect.name+" to "+)
			rect.SetSiblingIndex(Selection.activeGameObject.transform.GetSiblingIndex());
			float maxH = 10;
			float maxW = 10;
			float sumH = 0;
			int count = 0;
			for (int i = 0; i < Selection.objects.Length; i++)
			{
				GameObject g = Selection.objects[i] as GameObject;
				if (g != null)
				{
					RectTransform grect = g.GetComponent<RectTransform>();
					if (grect == null) continue;

					//                float h = grect.GetHeight();
					//              float w = grect.GetWidth();
					count++;
					float w = grect.sizeDelta.x;
					float h = grect.sizeDelta.y;
					sumH += h;

					if (h > maxH) maxH = h;
					if (w > maxW) maxW = w;
					LayoutElement le = grect.GetComponent<LayoutElement>();
					if (le == null)
					{
						le = grect.gameObject.AddComponent<LayoutElement>();
						le.preferredWidth = w;
						le.preferredHeight = h;
						le.flexibleWidth = 1;
					}

					grect.SetParent(rect);
				}
			}

			rect.SetSizeXY(maxW, sumH + (2 + count) * defaultSpacing);
			return rect;
#else
			return null;
#endif
		}

		private static RectTransform CreaatePanelChild()
		{
			GameObject go = new GameObject("Panel");
#if UNITY_EDITOR
			if (Selection.activeGameObject != null) go.transform.SetParent(Selection.activeGameObject.transform);
			else
#endif
				go.transform.SetParent(CreateCanvasIfNotPresent());
			go.transform.localPosition = Vector2.zero;
			RectTransform rect = go.AddComponent<RectTransform>();
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.sizeDelta = Vector2.zero;
			Image image = go.AddComponent<Image>();
			image.color = new Color(0, 0, 0, 0.2f);
#if UNITY_EDITOR
			image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
#endif
			image.type = Image.Type.Sliced;
			rect.localScale = Vector3.one;

#if UNITY_EDITOR
			Undo.RegisterCreatedObjectUndo(go, "Create object");
#endif

			return rect;
		}
	}
}
