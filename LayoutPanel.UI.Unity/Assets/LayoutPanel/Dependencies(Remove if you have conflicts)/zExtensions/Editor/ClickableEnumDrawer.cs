

// The purpose for this script was to avoid a nasty bug in UnityEditor 
// no matter what your Run In Background setting - unity will stop rendering when any dropdown
// is opened - this includes top menu (and there isnt much we can do about it)
// but it also affects any enum dropdowns, and I like those a lot, so I wrote this 'one-liner'
// which adds [CliickableEnum] decorator, which will display your custom enum as a toolbar
// zambari 2017

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ClickableEnumAttribute))]
public class ClickableEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        if (prop.propertyType == SerializedPropertyType.Enum)
            prop.enumValueIndex = GUILayout.Toolbar(prop.enumValueIndex, prop.enumDisplayNames);
        else
            GUILayout.Label("Please this attribute on ENUMs");
    }
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
        return 0;
    }
}