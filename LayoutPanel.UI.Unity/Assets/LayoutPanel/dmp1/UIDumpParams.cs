using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
#if UNITY_EDITOR
using UnityEditor.Overlays;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  more conditions
/// </summary>
public class UIDumpParams : MonoBehaviour
{
	public Transform sourceObject;

	[TextArea(5, 30)]
	public string result;

	private StringBuilder sb;

	private List<string> usedNames;

	private int currentObject;

	public string writePath;

	public int limitObjects = -1;

	public bool writeToFile;

	private Dictionary<string, string> nameDict;

	private Dictionary<Transform, string> nameDict2;

	private Dictionary<Color, string> colorValueDict;

	private Dictionary<Color, string> colorNameDict;

	public static string RemoveSpecialCharacters(string str)
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < str.Length; i++)
		{
			if ((str[i] >= '0' && str[i] <= '9') ||
				(str[i] >= 'A' && str[i] <= 'z'))
			{
				sb.Append(str[i]);
			}
		}

		return sb.ToString();
	}

	// [ExposeMethodInEditor]
	// void Restore()
	// {
	// 	if (writeToFile)
	// 	{
	// 	
	// 		File.WriteAllText(writePath, filePart1 + filePart2);
	// 		UnityEditor.AssetDatabase.Refresh();
	// 	}
	// }

	//
	// [ExposeMethodInEditor]
	// void GetSliderFields()
	// {
	// 	// var info = typeof(Slider).GetMembers(BindingFlags.Instance);
	// 	// foreach (var thisInfo in info)
	// 	// {
	// 	// 	Debug.Log($"this info {thisInfo.Name} {thisInfo.ToString()}  {thisInfo.ReflectedType}");
	// 	// }
	// 	//
	// 	// var props = typeof(Slider).GetProperties();
	// 	// foreach (var thisProp in props)
	// 	// {
	// 	// 	Debug.Log($"this prop {thisProp.Name} {thisProp.ToString()}  {thisProp.ReflectedType}");
	// 	// }
	//
	// 	var fields = typeof(Slider).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
	// 	foreach (var fieldInfo in fields)
	// 	{
	// 		Debug.Log($"thisFIELD  info {fieldInfo.Name} {fieldInfo.ToString()}  {fieldInfo.ReflectedType}");
	// 	}
	// }

	// [ExposeMethodInEditor]
	void CreateImportClass()
	{
		if (string.IsNullOrEmpty(writePath)) writePath = Path.Combine(Application.dataPath, "ObjCreateTest.cs");

		colorIndex = 0;
		nameDict = new Dictionary<string, string>();
		nameDict2 = new Dictionary<Transform, string>();
		colorValueDict = new Dictionary<Color, string>();
		colorNameDict = new Dictionary<Color, string>();
		currentObject = 0;
		sb = new StringBuilder();
		usedNames = new List<string>();
		ScanReferences(sourceObject);
		ImportTransform(sourceObject, null);

		// sb.Append("\n\n ")
		result = sb.ToString();
		if (writeToFile)
		{
			string rep = template.Replace("//#HERE", result);
			File.WriteAllText(writePath, rep);

			// File.WriteAllText(writePath, filePart1 + result + filePart2);
			UnityEditor.AssetDatabase.Refresh();
		}
	}

	// [ExposeMethodInEditor]
	void RestoreCreateClassForComipleError()
	{
		File.WriteAllText(writePath, template);
		UnityEditor.AssetDatabase.Refresh();
	}

	string SanitizeName(string src)
	{
		return CheckIfNameUnique(SanitizeNameInternal(src));
	}

	string SanitizeNameInternal(string src)
	{
		src = RemoveSpecialCharacters(src);
		src = src.Replace(" ", "");
		src = src.Replace("(", "");
		src = src.Replace(")", "");
		return src.ToLower();
	}

	string VectorToString(Vector2 vector)
	{
		return String.Format("new Vector2({0}f,{1}f)", vector.x, vector.y);
	}

	string Vector3ToString(Vector3 vector)
	{
		return String.Format("new Vector2({0}f,{1}f,{2}f)", vector.x, vector.y, vector.z);
	}

	string ColorToString(Color color)
	{
		return String.Format(
			"new Color({0}f,{1}f,{2}f,{3}f)",
			color.r,
			color.g,
			color.b,
			color.a);
	}

	void GetRectDescription(RectTransform rect, string objName)
	{
		if (rect == null) return;

		string thisObjName = CheckIfNameUnique("rectTransform_" + objName);

		sb.Append("\n // RectTransform \n");
		sb.Append(String.Format(" var {0} = {1}.GetComponent<RectTransform>(); \n ", thisObjName, objName));
		sb.Append(String.Format("  {0}.anchorMin = {1};\n ", thisObjName, VectorToString(rect.anchorMin)));
		sb.Append(String.Format("  {0}.anchorMax = {1};\n ", thisObjName, VectorToString(rect.anchorMax)));
		sb.Append(String.Format("  {0}.offsetMin = {1};\n ", thisObjName, VectorToString(rect.offsetMin)));
		sb.Append(String.Format("  {0}.offsetMax = {1};\n ", thisObjName, VectorToString(rect.offsetMax)));
		sb.Append(
			String.Format("  {0}.anchoredPosition = {1};\n ", thisObjName, VectorToString(rect.anchoredPosition)));
		if (rect.pivot != Vector2.one / 2)
			sb.Append(String.Format("  {0}.pivot = {1};  \n ", thisObjName, VectorToString(rect.pivot)));
	}

	void GetButtonDescription(Button button, string objName)
	{
		if (button == null) return;

		string thisObjName = CheckIfNameUnique(objName + "_button");

		sb.Append("\n // Button \n");
		sb.Append(String.Format(" var {0} = {1}.GetComponent<Button>(); \n ", thisObjName, objName));
		sb.Append(String.Format("  //{0}.targetGraphic = {1};\n ", thisObjName, button.targetGraphic.name));
		sb.Append(
			String.Format("  //{0}.targetGraphic = {1};\n ", thisObjName, nameDict2[button.targetGraphic.transform]));
	}

	void GetScrollRectDescription(ScrollRect scrollRect, string objName)
	{
		if (scrollRect == null) return;

		string thisObjName = CheckIfNameUnique(objName + "_scrollRect");
		sb.Append(String.Format(" var {0} = {1}.GetComponent<ScrollRect>(); \n ", thisObjName, objName));
	}

	void GetScrollBarDescription(Scrollbar scrollbar, string objName)
	{
		if (scrollbar == null) return;

		string thisObjName = CheckIfNameUnique("scrollRect_" + objName);
		sb.Append("\n // Scrollbar \n");
		sb.Append(String.Format(" var {0} = {1}.GetComponent<Scrollbar>(); \n ", thisObjName, objName));
	}

	void GetSliderDescription(Slider slider, string objName)
	{
		if (slider == null) return;

		string thisObjName = CheckIfNameUnique("slider_" + objName);

		sb.Append("\n // Slider \n");
		sb.Append(String.Format(" var {0} = {1}.GetComponent<Slider>(); \n ", thisObjName, objName));
		sb.Append(String.Format("  //{0}.targetGraphic = {1}\n; ", thisObjName, slider.targetGraphic.name));
	}

	void GetTextDescription(Text text, string objName)
	{
		if (text == null) return;

		string thisObjName = CheckIfNameUnique("text_" + objName);

		sb.Append("\n // Text \n");
		sb.Append(String.Format(" var {0} = {1}.GetComponent<Text>(); \n ", thisObjName, objName));
		sb.Append(String.Format("     {0}.text = \"{1}\";\n ", thisObjName, text.text));
		sb.Append(String.Format("     {0}.fontSize = {1};\n ", thisObjName, text.fontSize));
		sb.Append(String.Format("     {0}.color = {1}; \n ", thisObjName, ColorToString(text.color)));
		sb.Append(String.Format("     {0}.alignment = TextAnchor.{1};\n ", thisObjName, text.alignment));
		if (!text.raycastTarget)
			sb.Append(String.Format("     {0}.raycastTarget = {1};\n ", thisObjName, text.raycastTarget));
		if (!text.font.ToString().Contains("LegacyRuntime"))
			sb.Append(String.Format("    // {0}.font = {1};\n ", thisObjName, text.font));

		// sb.Append(String.Format("     {0}.alignment = TextAnchor. {1}\n ", thisObjName,text.));
	}

	string BoolToString(bool b)
	{
		return b ? "true" : "false";
	}

	void GetLayoutDescription(LayoutElement layout, string objName)
	{
		if (layout == null) return;

		sb.Append("\n // LayoutElement \n");
		string thisObjName = CheckIfNameUnique(objName + "_LayoutElement");
		sb.Append(String.Format(" var {0} = {1}.GetComponent<LayoutElement>(); \n ", thisObjName, objName));
		if (layout.minHeight != -1) sb.Append(String.Format(" {0}.minHeight={1}f;\n ", thisObjName, layout.minHeight));
		if (layout.minWidth != -1) sb.Append(String.Format(" {0}.minWidth={1}f;\n ", thisObjName, layout.minWidth));
		if (layout.preferredWidth != -1)
			sb.Append(String.Format(" {0}.preferredWidth={1}f;\n ", thisObjName, layout.preferredWidth));
		if (layout.preferredHeight != -1)
			sb.Append(String.Format(" {0}.preferredHeight={1}f;\n ", thisObjName, layout.preferredHeight));
		if (layout.flexibleWidth != -1)
			sb.Append(String.Format(" {0}.flexibleWidth={1}f;\n ", thisObjName, layout.flexibleWidth));
		if (layout.flexibleHeight != -1)
			sb.Append(String.Format(" {0}.flexibleHeight={1}f;\n ", thisObjName, layout.flexibleHeight));
	}

	void GetImageDescription(Image image, string objName)
	{
		if (image == null) return;

		sb.Append("\n // Image \n");
		string thisObjName = CheckIfNameUnique(objName + "_image");
		sb.Append(String.Format(" var {0} = {1}.GetComponent<Image>(); \n ", thisObjName, objName));
		sb.Append(String.Format(" {0}.color={1};\n ", thisObjName, ColorToString(image.color)));
		if (!image.enabled)
			sb.Append(String.Format("  {0}.enabled={1}; \n ", thisObjName, BoolToString(image.enabled)));
		if (!image.raycastTarget)
			sb.Append(String.Format("  {0}.raycastTarget={1}; \n ", thisObjName, BoolToString(image.raycastTarget)));
		sb.Append(String.Format("  {0}.type= Image.Type.{1}; \n ", thisObjName, image.type));
	}

	string CheckIfNameUnique(string src)
	{
		while (usedNames.Contains(src))
		{
			src += "a";
		}

		usedNames.Add(src);
		return src;
	}

	string GetComponents(Transform t)
	{
		var components = t.GetComponents<Component>();

		string componentNames = "";
		foreach (var thisComponent in components) componentNames += $"typeof({thisComponent.GetType()}),";

		componentNames = componentNames.Substring(0, componentNames.Length - 1);

		// componentNames = "null";
		return componentNames;
	}

	string AddDescription(Transform t, string parentName)
	{
		currentObject++;
		Debug.Log($"describing {t.name} ", t);

		var objName = nameDict2[t]; //  CheckIfNameUnique(SanitizeName(t.name));

		// nameDict.Add(t.name, objName);

		var componentNames = GetComponents(t);
		sb.Append(
			String.Format(
				"var {0} = new GameObject(\"{1}\",{2}); \n ",
				objName,
				t.name,
				componentNames));
		if (!string.IsNullOrEmpty(parentName))
		{
			sb.Append(string.Format("\n {0}.transform.SetParent({1}.transform) ;\n", objName, parentName));
		}

		GetImageDescription(t.GetComponent<Image>(), objName);
		GetRectDescription(t.GetComponent<RectTransform>(), objName);
		GetTextDescription(t.GetComponent<Text>(), objName);
		GetLayoutDescription(t.GetComponent<LayoutElement>(), objName);
		GetButtonDescription(t.GetComponent<Button>(), objName);
		GetSliderDescription(t.GetComponent<Slider>(), objName);
		GetScrollRectDescription(t.GetComponent<ScrollRect>(), objName);
		GetScrollBarDescription(t.GetComponent<Scrollbar>(), objName);
		sb.Append("\n");

		return objName;
	}

	void AddToNameDict(Component t)
	{
		if (t == null) return;

		if (!nameDict2.ContainsKey(t.transform)) nameDict2.Add(t.transform, SanitizeName(t.name));
	}

	private int colorIndex = 0;

	void AddColorDict(Color c)
	{
		if (!colorValueDict.ContainsKey(c))
		{
			colorValueDict.Add(c, ColorToString(c));
			colorNameDict.Add(c, $"color_{colorIndex++}");
		}
		else
		{
			Debug.Log("Duplcatecololr");
		}
	}

	void ScanRefs(Transform t)
	{
		AddToNameDict(t);
		var componentS = t.gameObject.GetComponents<Component>();
		foreach (var thisComponent in componentS)
		{
			if (thisComponent is Image image)
			{
				AddColorDict(image.color);
			}

			if (thisComponent is Text text)
			{
				AddColorDict(text.color);
			}

			if (thisComponent is Slider slider)
			{
				AddToNameDict(slider.handleRect);
				AddToNameDict(slider.fillRect);
				AddToNameDict(slider.targetGraphic);
			}

			if (thisComponent is Scrollbar scrollbar)
			{
				AddToNameDict(scrollbar.handleRect);
				AddToNameDict(scrollbar.targetGraphic);
			}

			if (thisComponent is ScrollRect scrollRect)
			{
				AddToNameDict(scrollRect.content);
				AddToNameDict(scrollRect.viewport);
				AddToNameDict(scrollRect.horizontalScrollbar);
				AddToNameDict(scrollRect.verticalScrollbar);
			}
		}

		// foreach (var kvp in nameDict2)
		// {
		// 	Debug.Log($"nameDict2  {kvp.Key} = {kvp.Value}");
		// }
		//
		// foreach (var kvp in colorNameDict)
		// {
		// 	sb.Append("\n //Colors \n\n");
		// 	sb.Append(String.Format("  {0} ccc={1} \n\n", kvp.Value, ColorToString(kvp.Key)));
		// }
	}

	void ScanReferences(Transform t)
	{
		ScanRefs(t);

		for (int i = 0;
			 i < t.childCount;
			 i++)
		{
			ScanReferences(t.GetChild(i));
		}
	}

	void ImportTransform(Transform t, string thisName)
	{
		// string thisName = nameDict2[t];
		if (limitObjects > 0 && currentObject >= limitObjects) return;

		sb.Append($" \n  // GAMEOBJECT  [{currentObject}: {t.name}] \n\n");
		var parentName = AddDescription(t, thisName);
		if (currentObject == 1) sb.Append(String.Format(" {0}.transform.SetParent(target);", parentName));
		for (int i = 0;
			 i < t.childCount;
			 i++)
		{
			ImportTransform(t.GetChild(i), parentName);
		}
	}

	private string template = @"using UnityEngine;
using UnityEngine.UI;

public class ObjCreateTest : MonoBehaviour
{
	public RectTransform target;
	[ExposeMethodInEditor]
	void TestCreate() {
		//#HERE
	 }
}
";
}
#endif
