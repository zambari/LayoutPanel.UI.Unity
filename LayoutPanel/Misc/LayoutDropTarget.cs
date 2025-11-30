namespace zUI.LayoutPanelTools
{
	using UnityEngine;

	public static class LayoutDropTarget
	{
		public static GameObject currentTargetObject;

		public static IDropTarget currentTarget
		{
			get
			{
				if (currentTargetObject == null) return null;
				 return currentTargetObject.GetComponent<IDropTarget>();
			}
		}
	}
}
