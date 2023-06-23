namespace Z.LayoutPanel
{
	using UnityEngine;
	using UnityEngine.UI;

	public static class LayoutExtensionsAdditional
	{
		public static void SetLayoutType(this GameObject gameObject, LayoutGroupType newGroupType, int spacing = 5)
		{
			if (gameObject == null) return;

			var ver = gameObject.GetComponent<VerticalLayoutGroup>();
			var hor = gameObject.GetComponent<HorizontalLayoutGroup>();
			if (newGroupType == LayoutGroupType.None || newGroupType == LayoutGroupType.HorizontalLayout)
			{
				if (ver) GameObject.DestroyImmediate(ver);
			}

			if (newGroupType == LayoutGroupType.None || newGroupType == LayoutGroupType.VerticalLayout)
			{
				if (hor) GameObject.DestroyImmediate(hor);
			}

			if (newGroupType == LayoutGroupType.HorizontalLayout)
			{
				if (hor == null)
				{
					hor = gameObject.AddComponent<HorizontalLayoutGroup>();
					hor.childControlWidth = true;
					hor.childControlHeight = true;
					hor.childForceExpandHeight = false;
					hor.childForceExpandWidth = false;
					hor.spacing = spacing;
				}
			}

			if (newGroupType == LayoutGroupType.VerticalLayout)
			{
				if (ver == null)
				{
					ver = gameObject.AddComponent<VerticalLayoutGroup>();
					ver.childControlWidth = true;
					ver.childControlHeight = true;
					ver.childForceExpandWidth = false;
					ver.childForceExpandHeight = false;
					ver.spacing = spacing;
				}
			}
		}
	}
}
