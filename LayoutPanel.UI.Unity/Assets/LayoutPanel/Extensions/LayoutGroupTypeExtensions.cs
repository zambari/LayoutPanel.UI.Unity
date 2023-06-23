namespace Z.LayoutPanel
{
	using UnityEngine;
	using UnityEngine.UI;

	public static class LayoutGroupTypeExtensions
	{
		public static LayoutGroupType GetLayoutGroupType(this GameObject gameObject)
		{
			if (gameObject.GetComponent<VerticalLayoutGroup>()) return LayoutGroupType.VerticalLayout;
			if (gameObject.GetComponent<HorizontalLayoutGroup>()) return LayoutGroupType.HorizontalLayout;

			return LayoutGroupType.None;
		}
	}
}
