//z2k17

namespace Z.LayoutPanel
{
	using UnityEngine;

	public static class LayoutResourceLoader
	{
		public static Sprite lineH
		{
			get
			{
				if (!_lineH) _lineH = Resources.Load<Sprite>("lineH");
				return _lineH;
			}
		}

		public static Sprite lineV
		{
			get
			{
				if (!_lineH) _lineH = Resources.Load<Sprite>("lineV");
				return _lineH;
			}
		}

		public static Texture2D horizontalCursor
		{
			get
			{
				if (!_horizontalCursor) _horizontalCursor = Resources.Load<Texture2D>("ResizeHorizontal");
				return _horizontalCursor;
			}
		}

		public static Texture2D vertialCursor
		{
			get
			{
				if (!_vertialCursor) _vertialCursor = Resources.Load<Texture2D>("ResizeVertical");
				return _vertialCursor;
			}
		}

		public static Texture2D panCursor
		{
			get
			{
				if (!_panCursor) _panCursor = Resources.Load<Texture2D>("PanView");
				return _panCursor;
			}
		}

		public static Sprite scrollbarBg
		{
			get
			{
				if (!_scrollBg) _scrollBg = Resources.Load<Sprite>("scrollbarBg");
				return _scrollBg;
			}
		}

		[SerializeField]
		public static Texture2D upLeftResizeCursor
		{
			get
			{
				if (!_upLeftResizeCursor) _upLeftResizeCursor = Resources.Load<Texture2D>("ResizeUpLeft");
				return _upLeftResizeCursor;
			}
		}

		[SerializeField]
		public static Texture2D upRightResizeCursor
		{
			get
			{
				if (!_upRightResizeCursor) _upRightResizeCursor = Resources.Load<Texture2D>("ResizeUpRight");
				return _upRightResizeCursor;
			}
		}

		private static Texture2D _horizontalCursor;

		private static Texture2D _vertialCursor;

		private static Texture2D _moveCursor;

		private static Texture2D _upLeftResizeCursor;

		private static Texture2D _upRightResizeCursor;

		private static Texture2D _panCursor;

		private static Sprite _scrollBg;

		private static Sprite _lineH;

		private static Sprite _lineV;
	}
}
