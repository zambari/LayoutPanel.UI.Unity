namespace Z.LayoutPanel
{
	using System.Collections.Generic;

	using UnityEngine;
	using UnityEngine.UI;

	public static class LayoutPanelCreatorExtensions
	{
		public static List<LayoutItemCreator> CreateHorizontalLayoutCreators(this GameObject where, int thisSplitCount)
		{
			var vg = where.GetComponent<VerticalLayoutGroup>();
			if (vg != null) GameObject.DestroyImmediate(vg);
			var list = LayoutPanelExtensions.AddComponentsToAllGameObjects<LayoutItemCreator>(
				where.GetComponent<RectTransform>().SplitToLayout(true, thisSplitCount));
			LayoutPanelExtensions.AmendName(where);
			return list;
		}

		public static List<LayoutItemCreator> CreateVerticalLayoutCreators(this GameObject where, int thisSplitCount)
		{
			var hg = where.GetComponent<HorizontalLayoutGroup>();
			if (hg != null) GameObject.DestroyImmediate(hg);
			var list = LayoutPanelExtensions.AddComponentsToAllGameObjects<LayoutItemCreator>(
				where.GetComponent<RectTransform>().SplitToLayout(false, thisSplitCount));

			LayoutPanelExtensions.AmendName(where);
			return list;
		}

		public static List<LayoutSplitCreator> CreateHorizontalLayoutSplits(this GameObject where, int thisSplitCount)
		{
			var vg = where.GetComponent<VerticalLayoutGroup>();
			if (vg != null) GameObject.DestroyImmediate(vg);
			var list = LayoutPanelExtensions.AddComponentsToAllGameObjects<LayoutSplitCreator>(
				where.GetComponent<RectTransform>().SplitToLayout(true, thisSplitCount));
			foreach (var item in list)
			{
				item.splitType = LayoutSplitCreator.SplitType.Vertical;
			}

			LayoutPanelExtensions.AmendName(where);
			return list;
		}

		public static List<LayoutSplitCreator> CreateVerticalLayoutSplits(this GameObject where, int thisSplitCount)
		{
			var hg = where.GetComponent<HorizontalLayoutGroup>();
			if (hg != null) GameObject.DestroyImmediate(hg);
			var list = LayoutPanelExtensions.AddComponentsToAllGameObjects<LayoutSplitCreator>(
				where.GetComponent<RectTransform>().SplitToLayout(false, thisSplitCount));
			LayoutPanelExtensions.AmendName(where);
			return list;
		}

		private static bool CheckCreateCondition(LayoutItemCreator c, bool checkForChildren)
		{
			if (checkForChildren) return c.transform.childCount == 0;

			return
				c.transform.parent.GetComponent<LayoutItemCreator>() == null;
		}

		private static List<LayoutItemCreator> AddItemCreatorsToChildren(List<GameObject> target)
		{
			return target.AddComponentsToAllGameObjects<LayoutItemCreator>();
		}

		public static void LaunchItemCreators(this GameObject src, bool checkForChildren, bool onlyCreateBorders)
		{
			var itemcr = src.GetComponentsInChildren<LayoutItemCreator>();
			int processed = 0;
			int notprocssed = 0;
			foreach (var i in itemcr)
			{
				if (CheckCreateCondition(i, checkForChildren))
				{
					if (onlyCreateBorders)
					{
						i.AddBorders();
					}
					else
					{
						i.ConvertToLayoutPanel();
					}

					processed++;
				}
				else notprocssed++;
			}

			// Debug.Log("Processed " + processed + " not processed " + notprocssed);
		}
	}
}
