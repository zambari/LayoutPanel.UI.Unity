namespace zUI.LayoutPanelTools
{
	using UnityEngine;

	public interface IDropTarget
	{
		int targetDropIndex { get; }

		Transform dropTarget { get; }

		Transform transform { get; }

		GameObject gameObject { get; }

		bool isHorizontal { get; }

		string name { get; }
	}
}
