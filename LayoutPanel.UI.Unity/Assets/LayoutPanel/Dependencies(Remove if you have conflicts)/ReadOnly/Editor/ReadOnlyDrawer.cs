// THIS IS A CLONE OF SHOWONLY
// renamed so I could packake it with zTools - i use it a lot

// I am ont an author of this

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        string valueStr;

        switch (prop.propertyType)
        {
            case SerializedPropertyType.Integer:
                valueStr = prop.intValue.ToString();
                break;
            case SerializedPropertyType.Boolean:
                valueStr = prop.boolValue.ToString();
                break;
            case SerializedPropertyType.Float:
                valueStr = prop.floatValue.ToString("0.00000");
                break;
            case SerializedPropertyType.String:
                valueStr = prop.stringValue;
                break;
           case SerializedPropertyType.Vector3:
                valueStr = prop.stringValue;
                break;
             case SerializedPropertyType.Vector2:
                valueStr = prop.stringValue;
                break;
            default:
                valueStr = "(not supported)";
                break;
        }
        string labelSplit=label.text;
        if (labelSplit[0]=='_') labelSplit=labelSplit.Substring(1);
        if (labelSplit[labelSplit.Length-1]=='_') labelSplit=labelSplit.Substring(0,labelSplit.Length-1);
        EditorGUI.LabelField(position, labelSplit, valueStr);
    }
}