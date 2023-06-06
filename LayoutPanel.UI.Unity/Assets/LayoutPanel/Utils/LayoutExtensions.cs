namespace Z.LayoutPanel
{
	using System.Collections.Generic;

	using UnityEditor;

	using UnityEngine;
	using UnityEngine.UI;

	public static class LayoutExtensions
	{
		public static void SetPivot(this RectTransform rect, Vector2 pivot)
		{
			float deltaPivotx = rect.pivot.x - pivot.x;
			float deltaPivoty = rect.pivot.y - pivot.y;
			Vector2 temp = rect.localPosition;
			rect.pivot = pivot;
			rect.localPosition = temp -
								 new Vector2(
									 deltaPivotx * rect.rect.width * rect.localScale.x,
									 deltaPivoty * rect.rect.height * rect.localScale.y);
		}

		public static void SetText(this Text text, string s)
		{
			if (text != null) text.text = s;
		}

		public static RectTransform AddChildRectTransform(this GameObject parent)
		{
			RectTransform parentRect = parent.GetComponent<RectTransform>();
			return parentRect.AddChild();
		}

		public static RectTransform AddChild(this RectTransform parentRect)
		{
			GameObject go = new GameObject("Rect", typeof(RectTransform));
			RectTransform rect = go.GetComponent<RectTransform>();

			go.transform.SetParent(parentRect);

			rect.anchorMin = new Vector2(0, 0);
			rect.anchorMax = new Vector2(1, 1);
			rect.sizeDelta = new Vector2(30, 30);
			rect.offsetMin = new Vector2(5, 5);
			rect.offsetMax = new Vector2(-5, -5);
			rect.localPosition = Vector2.zero;
			rect.localScale = Vector3.one;

			return rect;
		}

		public static void SetRelativeSizeX(this RectTransform rect, RectTransform parentRect, float v)
		{
			float sizeX = parentRect.rect.width;
			if (!float.IsNaN(v)) rect.sizeDelta = new Vector2(sizeX * v, rect.sizeDelta.y);
			else Debug.Log("source is" + rect.name + " parent " + parentRect.name, rect.gameObject);
		}

		public static void SetRelativeSizeY(this RectTransform rect, RectTransform parentRect, float v)
		{
			float sizeY = parentRect.rect.height;
			if (!float.IsNaN(v)) rect.sizeDelta = new Vector2(rect.sizeDelta.x, sizeY * v);
			else Debug.Log("source is" + rect.name + " parent " + parentRect.name, rect.gameObject);
		}

		public static void SetRelativeLocalY(this RectTransform rect, RectTransform parentRect, float v)
		{
			float sizeY = parentRect.rect.height;
			if (!float.IsNaN(v)) rect.localPosition = new Vector2(rect.localPosition.x, sizeY * v);
			else Debug.Log("source is" + rect.name + " parent " + parentRect.name, rect.gameObject);
		}

		public static void SetSizeXY(this RectTransform rect, float x, float y)
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
		}

		public static ContentSizeFitter AddContentSizeFitter(this RectTransform rect)
		{
			ContentSizeFitter c = rect.gameObject.AddOrGetComponent<ContentSizeFitter>();
			c.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			c.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			return c;
		}

		public static void SetSizeX(this RectTransform rect, float v)
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, v);
		}

		public static void SetSizeY(this RectTransform rect, float v)
		{
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, v);
		}

		public static LayoutElement AddIgnoreLayoutElement(this RectTransform src)
		{
			var le = src.gameObject.AddOrGetComponent<LayoutElement>();
			le.ignoreLayout = true;
			return le;
		}

		public static bool PrefabModeIsActive(
			this GameObject gameObject) //https://stackoverflow.com/questions/56155148/how-to-avoid-the-onvalidate-method-from-being-called-in-prefab-mode
		{
#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
			UnityEditor.SceneManagement.PrefabStage prefabStage =
				UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
			if (prefabStage != null) return true;
			if (UnityEditor.EditorUtility.IsPersistent(gameObject)) return true;
#endif
			return false;
		}

		public static T AddOrGetComponent<T>(this GameObject gameObject) where T : UnityEngine.Component
		{
			T t = gameObject.GetComponent<T>();
			if (t == null)
			{
				t = gameObject.AddComponent<T>();
#if UNITY_EDITOR
				if (t != null) UnityEditor.Undo.RegisterCreatedObjectUndo(t, "Added component");
#endif
			}

			return t;
		}

		public static void SetMargin(this HorizontalLayoutGroup layout, int margin = 0)
		{
			if (layout == null) return;

			layout.padding = new RectOffset(margin, margin, margin, margin);
		}

		public static void SetMargin(this VerticalLayoutGroup layout, int margin = 0)

		{
			if (layout == null) return;

			layout.padding = new RectOffset(margin, margin, margin, margin);
		}

		public static void SetChildControl(this HorizontalLayoutGroup layout, float spacing = 0)

		{
			if (layout == null) return;

			layout.childForceExpandHeight = true;
			layout.childForceExpandWidth = false;
			layout.childControlHeight = true;
			layout.childControlWidth = true;
			layout.spacing = spacing;
		}

		public static VerticalLayoutGroup ToVeritical(this HorizontalLayoutGroup layout)
		{
			RectOffset padding = layout.padding;
			float spacing = layout.spacing;
			bool childForceExpandHeight = layout.childForceExpandHeight;
			bool childForceExpandWidth = layout.childForceExpandWidth;
			bool childControlHeight = layout.childControlHeight;
			bool childControlWidth = layout.childControlWidth;
			GameObject g = layout.gameObject;
			GameObject.DestroyImmediate(layout);
			VerticalLayoutGroup ng = g.AddComponent<VerticalLayoutGroup>();
			ng.childForceExpandHeight = false;
			ng.childForceExpandWidth = false;
			ng.childControlHeight = true;
			ng.childControlWidth = true;
			ng.spacing = spacing;
			ng.padding = padding;
			return ng;
		}

		public static T[] GetComponentsInChildrenIncludingInactive<T>(this Component k, List<Transform> childrenList)
		{
			List<T> list = new List<T>();
			foreach (var c in childrenList)
			{
				var thisComponent = c.GetComponent<T>();
				if (thisComponent != null) list.Add(thisComponent);
			}

			return list.ToArray();
		}

		public static Image AddChildImage(this RectTransform parentRect, string name = "unnamed")
		{
			GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
			RectTransform rect = go.GetComponent<RectTransform>();
			go.transform.SetParent(parentRect);

			rect.anchorMin = new Vector2(0, 0);
			rect.anchorMax = new Vector2(1, 1);

			rect.localPosition = Vector2.zero;
			rect.localScale = Vector3.one;
			return go.GetComponent<Image>();
		}

		public static RectTransform AddColumn(this RectTransform parent, string name = "Column")
		{
			var rect = parent.AddFillingChild();
			rect.name = name;
			rect.SetColor(Color.green * .2f, true);

			//	rect.sizeDelta = new Vector2(300, rect.sizeDelta.y);
			rect.anchorMin = new Vector2(0, 0);
			rect.anchorMax = new Vector2(0, 1);

			rect.sizeDelta = new Vector2(120, 10);
			rect.anchoredPosition = Vector2.zero;
			rect.localScale = Vector3.one;

			//rect.offsetMin = new Vector2(5, 5);
			var vertLayout = rect.gameObject.AddComponent<VerticalLayoutGroup>();
			vertLayout.SetSomeLayoutParameters();
			rect.AddColumnLayoutElement();
			return rect;
		}

		public static void SetSomeLayoutParameters(this HorizontalLayoutGroup vl)
		{
			vl.spacing = 2;
			vl.padding = new RectOffset(3, 3, 3, 3);
			vl.childControlWidth = true;
			vl.childControlHeight = true;
			vl.childForceExpandHeight = true;
			vl.childForceExpandWidth = false;
		}

		public static Color Random(this Color c)
		{
			c.a = UnityEngine.Random.value * 0.4f + 0.2f;
			return UnityEngine.Random.ColorHSV(0.4f, 0.8f, 0.3f, 0.6f);
		}

		public static void AddColumnLayoutElement(this RectTransform rect, float preferredWidth = 200)
		{
			var element = rect.gameObject.AddComponent<LayoutElement>();
			element.preferredWidth = preferredWidth;
			element.flexibleHeight = 1;
		}

		public static void SetSomeLayoutParameters(this VerticalLayoutGroup vl)
		{
			vl.spacing = 2;
			vl.padding = new RectOffset(3, 3, 3, 3);
			vl.childControlWidth = true;
			vl.childControlHeight = true;
			vl.childForceExpandHeight = true;
			vl.childForceExpandWidth = false;
		}

		public static void
			MoveComponent(this UnityEngine.Component component, int offset) // : where T:UnityEngine.Component
		{
#if UNITY_EDITOR
			if (component == null) return;

#if UNITY_2018_3_OR_NEWER
			var status = UnityEditor.PrefabUtility.GetPrefabInstanceStatus(component.gameObject);
			if (status == PrefabInstanceStatus.Connected)
			{
				Debug.Log("cannot move component on prefab, aborting. remove this debug");
				return;
			}
#endif
			if (offset < 0)
				for (int i = 0; i < -offset; i++)
					UnityEditorInternal.ComponentUtility.MoveComponentUp(component);
			else
				for (int i = 0; i < offset; i++)
					UnityEditorInternal.ComponentUtility.MoveComponentDown(component);
#endif
		}

		public static T AddOrGetComponent<T>(this MonoBehaviour mono) where T : UnityEngine.Component
		{
			T t = mono.gameObject.GetComponent<T>();
			if (t == null) t = mono.gameObject.AddComponent<T>();
			return t;
		}

		public static float
			Randomize(this float f, float howMuch) // warning this methos has chaned the parameter scaling
		{
			float n = f * UnityEngine.Random.Range(1 - howMuch, 1 + howMuch);
			return n;
		}

		public static List<T> GetComponentsInChildrenIncludingInactive<T>(this Component k)
		{
			List<T> list = new List<T>();
			if (k == null) return list;

			var first = k.GetComponent<T>();
			if (first != null) list.Add(first);
			var allchildren = k.transform.GetAllChildren();
			foreach (var c in allchildren)
			{
				var thisComponent = c.GetComponent<T>();
				if (thisComponent != null) list.Add(thisComponent);
			}

			return list; //.ToArray();
		}

		public static List<Transform> GetAllChildren(this Transform transform, List<Transform> list = null)
		{
			if (list == null) list = new List<Transform>();
			list.Add(transform);
			for (int i = 0; i < transform.childCount; i++)
			{
				var thisTransform = transform.GetChild(i);
				thisTransform.GetAllChildren(list);
			}

			return list;
		}

		public static HorizontalLayoutGroup ToHorizontal(this VerticalLayoutGroup layout)
		{
			RectOffset padding = layout.padding;
			float spacing = layout.spacing;
			bool childForceExpandHeight = layout.childForceExpandHeight;
			bool childForceExpandWidth = layout.childForceExpandWidth;
			bool childControlHeight = layout.childControlHeight;
			bool childControlWidth = layout.childControlWidth;
			GameObject g = layout.gameObject;
			GameObject.DestroyImmediate(layout);
			HorizontalLayoutGroup ng = g.AddComponent<HorizontalLayoutGroup>();
			ng.childForceExpandHeight = false;
			ng.childForceExpandWidth = false;
			ng.childControlHeight = true;
			ng.childControlWidth = true;
			ng.spacing = spacing;
			ng.padding = padding;
			return ng;
		}

		public static void SetChildControl(this VerticalLayoutGroup layout, float spacing = 0)
		{
			if (layout == null) return;

			layout.childForceExpandHeight = false;
			layout.childForceExpandWidth = true;
			layout.childControlHeight = true;
			layout.childControlWidth = true;
			layout.spacing = spacing;
		}

		public static Image AddImageChild(this RectTransform rect, float opacity = 0.3f)
		{
			return rect.gameObject.AddImageChild(opacity);
		}

		public static Image AddImageChild(this GameObject g, float opacity = 0.3f)
		{
			Image image = g.AddChildRectTransform().gameObject.AddComponent<Image>();
			image.color = new Color(
				UnityEngine.Random.value * 0.3f + 0.7f,
				UnityEngine.Random.value * 0.3f + 0.7f,
				UnityEngine.Random.value * 0.2f,
				opacity);
			image.sprite = Resources.Load("Background") as Sprite;
			image.name = "Image";
			return image;
		}
	}
}
