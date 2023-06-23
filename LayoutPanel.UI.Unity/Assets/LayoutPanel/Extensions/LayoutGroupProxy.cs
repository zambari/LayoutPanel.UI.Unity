namespace Z.LayoutPanel
{
	using UnityEngine.UI;

	public class LayoutGroupProxy
	{
		private HorizontalLayoutGroup hor;

		private VerticalLayoutGroup ver;

		public LayoutGroupProxy(HorizontalLayoutGroup horizontalLayoutGroup)
		{
			hor = horizontalLayoutGroup;
		}

		public LayoutGroupProxy(VerticalLayoutGroup verticalLayoutGroup)
		{
			ver = verticalLayoutGroup;
		}
	}
}
