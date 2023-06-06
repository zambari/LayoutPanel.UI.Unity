using UnityEngine;

namespace Z.LayoutPanel
{
	[System.Serializable]
	public class BorderSetup
	{
		public Color baseColor = Color.gray;

		[Range(0, 25)]
		public int borderSizeBottom = 5;

		public int borderSize
		{
			get { return borderSizeBottom; }
			set
			{
				if (borderSizeBottom == value) return;

				borderSizeBottom = value;
			}
		}

		[Range(0, 25)]
		public int borderSizeTop = 5;

		public bool linkTopAndSides;

		[Range(0, 25)]
		public int borderSizeH = 5;

		public bool linkTopAndBottom = true;

		public bool extendToFillCornersHorizontal;

		public bool sidesFillBottomCorners = true;

		public bool sidesFillTopCorners = true;

		[Header("dir")]
		public bool bordersPlacedInside;

		public bool topBorder = true;

		public bool bottomBorder = true;

		public Color normalColor
		{
			get
			{
				Color color = baseColor;
				color.a = normalAlpha;
				return color;
			}
		}

		public bool GetDragEnabled(Side side)

		{
			if (side == Side.Top) return topDragEnabled;

			return true;
		}

		public Color hoveredColor
		{
			get
			{
				Color color = baseColor;
				color.a = hoveredAlpha;
				return color;
			}
		}

		public float GetSize(Side side)
		{
			switch (side)
			{
				case Side.Left:
				case Side.Right: return borderSizeH;
				case Side.Top:
					if (!topBorder) return 0;

					return borderSizeTop;
				case Side.Bottom:
					if (!bottomBorder) return 0;

					return borderSizeBottom;
				default: return 0;
			}
		}

		[Range(0, 1)]
		public float normalAlpha = .5f;

		[Range(0, 1)]
		public float hoveredAlpha = .8f;

		public bool topDragEnabled;

		public Color dropTargetColor = new Color(0.2f, 1, 0.2f, 0.8f);

		public Color dropTargetColorWhenSplit = new Color(.7f, 0.2f, 0.7f, 0.8f);

		// public bool useOffsets;
		// public Vector4 offsetsT;
		// public Vector4 offsetsR;
		// public Vector4 offsetsL;
		// public Vector4 offsetsB;
		//	public bool moreofset=true;

		public void OnValidate()

		{
			baseColor.a = 1;
			if (linkTopAndBottom) borderSizeTop = borderSizeBottom;
			if (linkTopAndSides) borderSizeH = borderSizeBottom;
			if (!topBorder) borderSizeTop = 0;
		}

		public int verticalSizeOffset;

		[Range(0, 20)]
		public int offsetSide = 0;

		public void ApplyRectSettings(RectTransform target, Side side)
		{
			Vector3 newAnchoredPosition = Vector3.zero;
			target.anchorMin = side.GetAnchorMin();
			target.anchorMax = side.GetAnchorMax();
			target.pivot = side.GetPivot(bordersPlacedInside);
			target.anchoredPosition = Vector2.zero;
			target.sizeDelta = side.GetSizeDelta(GetSize(side));

			//doubleDistance
			//	if (moreofset)
			switch (side)
			{
				case Side.Top:
				case Side.Bottom:
				{
					if (extendToFillCornersHorizontal)
					{
						//
						//	toffsetsB
						//	target.offsetMin += new Vector2(0, -verticalSizeOffset);
						//	target.offsetMax += new Vector2(0, verticalSizeOffset);
						//	target.offsetMax += new Vector2(0,0);
					}
				}
					break;
				case Side.Left:
				case Side.Right:
				{
					if (sidesFillBottomCorners)
					{
						if (bordersPlacedInside) target.offsetMin += new Vector2(0, GetSize(Side.Bottom));
						else target.offsetMin += new Vector2(0, -GetSize(Side.Bottom));
						target.offsetMax += new Vector2(0, 0);
					}

					if (sidesFillTopCorners)
					{
						target.offsetMin += new Vector2(0, 0);
						if (bordersPlacedInside) target.offsetMax += new Vector2(0, -GetSize(Side.Top));
						else target.offsetMax += new Vector2(0, GetSize(Side.Top));
					}
				}
					break;
			}

			// if (useOffsets)
			// {
			// 	Vector4 ofsets = GetOffset(side);
			// 	target.offsetMin += new Vector2(ofsets.x, ofsets.y);
			// 	target.offsetMax += new Vector2(ofsets.z, ofsets.w);
			// }
			target.anchoredPosition += side.GetDirection() * offsetSide;

			//	target.offsetMin = new Vector2(horizontalOffsets.x, verticalOffsets.x);
			//	target.offsetMax = new Vector2(horizontalOffsets.y, verticalOffsets.y);

			// if (columnMode)
			// {
			//     if (_side == Side.Top)
			//         newAnchoredPosition += new Vector3(0, LayoutSettings.borderSizeColumnOffset);
			//     if (_side == Side.Bottom)
			//         newAnchoredPosition += new Vector3(0, -LayoutSettings.borderSizeColumnOffset);
			//     rect.anchoredPosition = newAnchoredPosition;

			// }
		}
	}
}
