namespace zUI
{
	using zUI.LayoutPanelTools;

	using UnityEngine;
	using UnityEngine.UI;

	[ExecuteInEditMode]
	[RequireComponent(typeof(HorizontalLayoutGroup))]
	public class LayoutHorizontalController : MonoBehaviour
	{
		public DrawInspectorBg draw;

		HorizontalLayoutGroup group;

		private void OnEnable()
		{
			LayoutPanel.onBorderSizeChange += UpdateBorder;
		}

		private void OnDisable()
		{
			LayoutPanel.onBorderSizeChange -= UpdateBorder;
		}

		private void UpdateBorder()
		{
			if (group == null) group = GetComponent<HorizontalLayoutGroup>();
			group.spacing = LayoutPanel.borderSize * 3 + 2 * LayoutPanel.borderSpacing;
			var padding = group.padding;
			padding.top = (int)group.spacing;
			padding.bottom = (int)group.spacing;
			group.padding = padding;
		}
	}
}
