using UnityEngine;
using UnityEngine.UI;

namespace Z.LayoutPanel
{
	using System.Collections.Generic;

	public class ColumnController : MonoBehaviour
	{
		private List<Column> columns = new List<Column>();

		private RectTransform _rect;

		private bool spacerAdded;

		private RectTransform rect
		{
			get
			{
				if (_rect == null) _rect = GetComponent<RectTransform>();
				return _rect;
			}
		}

		public RectTransform AddColumn()
		{
			var col = rect.AddColumn();

			var thisCol = col.gameObject.AddComponent<Column>();
			thisCol.isRightOfSpacer = spacerAdded;
			columns.Add(thisCol);
			col.gameObject.AddComponent<LayoutItemCreator>();
			thisCol.index = columns.Count;
			//var border = col.AddChildImage().gameObject.AddComponent<LayoutBorderDragger>();
			//border.side = Side.Right;
			return col;
		}

		public RectTransform AddColumnSpacer(string name = "Spacer")
		{
			var newRect = rect.AddFillingChild();
			newRect.name = name;
			newRect.anchorMin = new Vector2(0, 0);
			newRect.anchorMax = new Vector2(0, 1);
			newRect.sizeDelta = new Vector2(120, 10);
			newRect.anchoredPosition = Vector2.zero;
			newRect.localScale = Vector3.one;
			var le = newRect.gameObject.AddComponent<LayoutElement>();
			le.flexibleHeight = 1;
			le.flexibleWidth = 1;
			spacerAdded = true;
			return newRect;
		}
	}
}
