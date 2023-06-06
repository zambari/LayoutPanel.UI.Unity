using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Z.LayoutPanel;

namespace Z.LayoutPanel
{
	public static class MoreLayoutExtensions
	{
		private const int defaultVerticalSpacing = 20;

		private const int defaultHorizontalSpacing = 10;

		public static void SetParams(this HorizontalLayoutGroup group, LayoutPanel panel)
		{
			group.SetChildControlFromPanel();

			group.SetPadding(panel);
			group.SetSpacing();
		}

		public static void SetParams(this VerticalLayoutGroup group, LayoutPanel panel)
		{
			group.SetChildControlFromPanel();
			group.SetPadding(panel);
			group.SetSpacing();
		}

		public static void SetPadding(this HorizontalLayoutGroup layoutGroup, LayoutPanel panel)
		{
			layoutGroup.padding = GetRectOffset(panel);
		}

		public static void SetPadding(this VerticalLayoutGroup layoutGroup, LayoutPanel panel)
		{
			layoutGroup.padding = GetRectOffset(panel);
		}

		public static void SetSpacing(this VerticalLayoutGroup group)
		{
			group.spacing = defaultVerticalSpacing;
		}

		public static void SetSpacing(this HorizontalLayoutGroup group)
		{
			group.spacing = defaultHorizontalSpacing;

			//group.spacing = LayoutSettings.groupSpacing;
		}

		// public static void SetMins(this LayoutElement le)
		// {
		// 	le.minHeight = topHeight;
		// 	le.minWidth = minWidth;
		// }

		public static List<GameObject> SplitToLayout(
			this GameObject container,
			bool horizontal,
			int count,
			float flex = -2)
		{
			return container.GetComponent<RectTransform>().SplitToLayout(horizontal, count, flex);
		}

		public static Color Randomize(
			this Color c,
			float howMuchHue = 0.15f,
			float howMuchSat = 0.3f,
			float howMuchL = 0.2f)
		{
			float H,
				  S,
				  L;
			Color.RGBToHSV(c, out H, out S, out L);
			H = H.Randomize(howMuchHue);
			H = H % 1;
			S = S.RandomizeClamped(howMuchSat);
			L = L.RandomizeClamped(howMuchL);
			Color newCol = Color.HSVToRGB(H, S, L);
			newCol.a = c.a;
			return newCol;
		}

		public static float RandomizeClamped(this float f, float howMuch)
		{
			float n = f.Randomize(howMuch);
			if (n < 0) n = 0;
			if (n > 1) n = 1;
			return n;
		}

		public static void SetChildControlFromPanel(this HorizontalLayoutGroup layout, float spacing = 0)

		{
			if (layout == null) return;

			layout.childForceExpandHeight = true;
			layout.childForceExpandWidth = false;
			layout.childControlHeight = true;
			layout.childControlWidth = true;
			layout.spacing = spacing;
		}

		// public static void SetParams(this HorizontalLayoutGroup group, LayoutPanel panel)
		// {
		// 	group.SetChildControlFromPanel();

		// 	group.SetPadding(panel);
		// 	group.SetSpacing();
		// }
		// public static void SetParams(this VerticalLayoutGroup group, LayoutPanel panel)
		// {
		// 	group.SetChildControlFromPanel();
		// 	group.SetPadding(panel);
		// 	group.SetSpacing();
		// }
		public static void SetChildControlFromPanel(this VerticalLayoutGroup layout, float spacing = 0)

		{
			if (layout == null) return;

			layout.childForceExpandHeight = false; // ?
			layout.childForceExpandWidth = true;
			layout.childControlHeight = true;
			layout.childControlWidth = true;
			layout.spacing = spacing;
		}

		// public static int groupPaddigNoPanel { get { return 0; } }
		//         public static int groupPaddigPanel { get { return borderSize + internalGroupPadding; } }
		static RectOffset GetRectOffset(LayoutPanel panel)
		{
			RectOffset padding = new RectOffset();
			int size = (panel == null) ? 0 : 44;
			padding.top = size;
			padding.left = size;
			padding.bottom = size;
			padding.right = size;
			return padding;
		}

		public static List<GameObject> SplitToLayout(
			this RectTransform container,
			bool horizontal,
			int count,
			float flex = -2)
		{
			if (!horizontal)
			{
				var oldgroup = container.gameObject.GetComponent<HorizontalLayoutGroup>();
				if (oldgroup != null) GameObject.DestroyImmediate(oldgroup);
				var group = container.gameObject.AddOrGetComponent<VerticalLayoutGroup>();
				group.SetChildControlFromPanel();
				group.SetSpacing();
			}
			else
			{
				var oldgroup = container.gameObject.GetComponent<VerticalLayoutGroup>();
				if (oldgroup != null) GameObject.DestroyImmediate(oldgroup);
				var group = container.gameObject.AddOrGetComponent<HorizontalLayoutGroup>();
				group.SetChildControlFromPanel();
				group.SetSpacing();
			}

			List<GameObject> cretedObjects = new List<GameObject>();
			for (int i = 0; i < count; i++)
			{
				RectTransform child = container.AddChild();

#if UNITY_EDITOR

				UnityEditor.Undo.RegisterCreatedObjectUndo(child.gameObject, "layout");
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
					le.flexibleHeight = (!horizontal ? 1f / count : 1);
					le.flexibleWidth = (!horizontal ? 1 : 1f / count);
				}
				else
				{
					le.flexibleHeight = flex;
					le.flexibleWidth = flex;
				}

				child.localScale = Vector3.one; //why do we need this
			}

			container.name = (horizontal ? "HorizontalLayout" : "VerticalLayout");
			return cretedObjects;
		}

		public static RectTransform SetColor(this RectTransform rectTransform, Color color, bool addImage)
		{
			var image = rectTransform.GetComponent<Image>();
			if (image == null && addImage) image = rectTransform.gameObject.AddComponent<Image>();
			image.color = color;
			return rectTransform;
		}
	}
}
