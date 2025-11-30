namespace zUI.LayoutPanelTools.Editor
{
	using UnityEditor;

	using UnityEngine;

	/// <summary>
	/// This is a very hacky (but still working) way to draw a tiled texture as a background for the inspector
	/// </summary>
	[CustomPropertyDrawer(typeof(DrawInspectorBg))]
	public class InspectorBgDrawer : PropertyDrawer
	{
		static Texture2D _bgTexture;

		public static void DrawBG()
		{
			Rect rect = EditorGUILayout.GetControlRect();
			DrawBG(rect);
		}

		public static void DrawBG(Rect rect)
		{
			Texture2D texture = bgTexture;
			if (texture != null)
			{
				for (int x = 0; x < rect.width; x += texture.width)
					for (int y = 0; y < rect.height; y += texture.height)
						GUI.DrawTextureWithTexCoords(
							new Rect(rect.x + x, rect.y + y, texture.width, texture.height),
							texture,
							new Rect(0f, 0f, 1f, 1f));
			}
		}

		public static Texture2D bgTexture
		{
			get
			{
				if (_bgTexture == null) _bgTexture = ResourceLoader.InspectorBGTexture;
				return _bgTexture;
			}
		}

		public override void OnGUI(Rect _, SerializedProperty property, GUIContent label)
		{
			Rect rect = EditorGUILayout.GetControlRect();
			rect.width += 8;

			// Sorry, unity kept changing the interaction between rects, I am not sure I pinpoint the exact version here, 
			// but it should be ok at least on some versions.
#if UNITY_6000_1_OR_NEWER
			rect.y = rect.y - 28;
			rect.x = rect.x - 19;
#elif UNITY_2022_3_OR_NEWER
			rect.y = rect.y - 28;
			rect.x = rect.x - 18;
#else
			rect.y = rect.y - 10;
			rect.x = rect.x - 15;
#endif
			DrawBG(rect);
			DrawBG(rect);

			//  GUI.Label(rect, id.stringValue, EditorStyles.miniLabel);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 0;
		}
	}
}
