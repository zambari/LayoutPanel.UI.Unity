namespace Z.LayoutPanel
{
	using UnityEngine;

	public interface IDropTarget
		{
			int targetDropIndex { get; }
			Transform dropTarget { get; }
			Transform transform { get; }
			GameObject gameObject { get; }
			bool isHorizontalBar { get; }
			bool isPanelHorizontal { get; }
			string name { get; }
		}

}
