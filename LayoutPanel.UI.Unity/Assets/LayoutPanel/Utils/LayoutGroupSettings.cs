using UnityEngine;
using UnityEngine.UI;

namespace Z.LayoutPanel
{
	[System.Serializable]
	public class LayoutGroupSettings
	{
		public bool useSettings = true;

		public float spacing = 10;

		public bool sharedSpacing = true;

		public RectOffset padding;

		public int spacingTimesBorder = 1;

		// public LayoutGroupSettings()
		// {
		// 	padding = new RectOffset(10, 10, 10, 10);
		// }
		public void ApplyTo(VerticalLayoutGroup group, LayoutSetup setup)
		{
			if (!useSettings) return;

			if (group != null)
			{
				group.padding = padding;
				group.spacing = spacing + setup.borderSetup.GetSize(Side.Top) + setup.borderSetup.GetSize(Side.Bottom);
			}
		}

		public void ApplyTo(HorizontalLayoutGroup group, LayoutSetup setup)
		{
			if (!useSettings) return;

			if (group != null)
			{
				group.padding = padding;
				group.spacing = spacing + setup.borderSetup.borderSizeH * spacingTimesBorder;
			}
		}
	}
}
