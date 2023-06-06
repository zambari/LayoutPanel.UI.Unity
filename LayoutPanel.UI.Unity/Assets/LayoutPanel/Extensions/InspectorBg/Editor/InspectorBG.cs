
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// small layout change for 2019

namespace Z.LayoutPanel
{
    using UnityEditor;
    namespace zUI
    {
        [CustomPropertyDrawer(typeof(DrawInspectorBg))]
        public class DrawInspectorBgDDrawer : PropertyDrawer
        {
            static Texture2D _bgTexture;
            public static void DrawBG()
            {
                Rect rect = EditorGUILayout.GetControlRect();
                DrawBG(rect);
            }
            public static void DrawBG(Rect rect)
            {
#if UNITY_2019_3_OR_NEWER
    rect.y = rect.y - 28;
            rect.x = rect.x - 18;
#else

            rect.y = rect.y - 18;
            rect.x = rect.x - 15;
#endif
                Texture2D texture = bgTexture;
                if (texture != null)
                {
                    for (int x = 0; x < rect.width; x += texture.width)
                        for (int y = 0; y < rect.height; y += texture.height)
                            GUI.DrawTextureWithTexCoords(new Rect(rect.x + x, rect.y + y, texture.width, texture.height), texture, new Rect(0f, 0f, 1f, 1f));
                }
            }
            public static Texture2D bgTexture
            {
                get
                {
                    if (_bgTexture == null)
                        _bgTexture = Resources.Load("stripebg") as Texture2D;
                    return _bgTexture;
                }
            }

            public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
            {
                Rect rect1 = EditorGUILayout.GetControlRect();
                rect1.y -= 20;
                rect1.width += 8;
                DrawBG(rect1);

                //  GUI.Label(rect, id.stringValue, EditorStyles.miniLabel);
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return 0;
            }
        }
    }
}

#endif