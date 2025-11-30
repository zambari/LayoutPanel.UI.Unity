namespace zUI.LayoutPanelTools
{
	//z2k17

	using UnityEngine;

	//using UnityEngine.UI;
	//using UnityEngine.UI.Extensions;
	//using System.Collections;
	//using System.Collections.Generic;
	// v.02 pan cursor
	// v.03 moved from resources root to base path
	public class ResourceLoader : MonoBehaviour
	{
		private const string BasePath = "LayoutPanel/";

		private const string ResizeHorizontal = "ResizeHorizontal";

		private const string ResizeVertical = "ResizeVertical";

		private const string ResizeUpLeft = "ResizeUpLeft";

		private const string ResizeUpRight = "ResizeUpRight";

		private const string InspectorBG = "InspectorBG";

		// Open source font used ! https://github.com/googlefonts/opensans

		private const string FontName = "OpenSans-Light";

		private const string PanView = "PanView";

		private const string LineH = "lineH";

		private const string LineV = "lineV";

		private static Texture2D _horizontalCursor;

		private static Texture2D _vertialCursor;

		private static Texture2D _moveCursor;

		private static Texture2D _upLeftResizeCursor;

		private static Texture2D _upRightResizeCursor;

		private static Texture2D _panCursor;

		private static Texture2D _inspectorBG;

		private static bool loaded;

		private static Sprite _lineH;

		private static Sprite _lineV;

		public static Sprite lineH
		{
			get
			{
				if (!loaded) LoadResources();
				return _lineH;
			}
		}

		public static Sprite lineV
		{
			get
			{
				if (!loaded) LoadResources();
				return _lineV;
			}
		}

		public static Texture2D horizontalCursor
		{
			get
			{
				if (!loaded) LoadResources();
				return _horizontalCursor;
			}
		}

		public static Texture2D vertialCursor
		{
			get
			{
				if (!loaded) LoadResources();
				return _vertialCursor;
			}
		}

		public static Texture2D moveCursor
		{
			get
			{
				if (!loaded) LoadResources();
				return _moveCursor;
			}
		}

		public static Texture2D panCursor
		{
			get
			{
				if (!loaded) LoadResources();
				return _panCursor;
			}
		}

		public static Texture2D upLeftResizeCursor
		{
			get
			{
				if (!loaded) LoadResources();
				return _upLeftResizeCursor;
			}
		}

		public static Texture2D upRightResizeCursor
		{
			get
			{
				if (!loaded) LoadResources();
				return _upRightResizeCursor;
			}
		}

		private static Font _defaultFont;

		public static Font DefaultFont
		{
			get
			{
				if (_defaultFont == null) _defaultFont = Resources.Load<Font>(BasePath + FontName);
				return _defaultFont;
			}
		}

		public static Texture2D InspectorBGTexture
		{
			get
			{
				if (_inspectorBG == null) _inspectorBG = Resources.Load(BasePath + InspectorBG) as Texture2D;
				return _inspectorBG;
			}
		}

		private static void LoadResources()
		{
			loaded = true;
			if (_horizontalCursor == null) _horizontalCursor = Resources.Load(BasePath + ResizeHorizontal) as Texture2D;
			if (_vertialCursor == null) _vertialCursor = Resources.Load(BasePath + ResizeVertical) as Texture2D;
			if (_panCursor == null) _panCursor = Resources.Load(BasePath + PanView) as Texture2D;
			if (_horizontalCursor == null || _vertialCursor == null) { }
			else
			{
				if (_moveCursor == null) _moveCursor = Resources.Load(BasePath + PanView) as Texture2D;
				if (_upLeftResizeCursor == null)
					_upLeftResizeCursor = Resources.Load(BasePath + ResizeUpLeft) as Texture2D;
				;
				if (_upRightResizeCursor == null)
					_upRightResizeCursor = Resources.Load(BasePath + ResizeUpRight) as Texture2D;
			}

			if (_lineH == null) _lineH = Resources.Load<Sprite>(BasePath + LineH) as Sprite;
			if (_lineV == null) _lineV = Resources.Load<Sprite>(BasePath + LineV) as Sprite;
		}
	}
}
