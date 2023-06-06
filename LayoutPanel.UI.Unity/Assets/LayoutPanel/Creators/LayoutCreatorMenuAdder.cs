#if UNITY_EDITOR

using UnityEditor;

namespace Z.LayoutPanel
{
	using UnityEngine;
	using UnityEngine.UI;

	public static class LayoutCreatorMenuAdder
	{
		[MenuItem("GameObject/UI/Add LayoutPanel Item")]
		static RectTransform CreaatePanelChildLayout()
		{
			if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<RectTransform>() == null)
			{
				Debug.Log("Please select a RectTransform first");
				return null;
			}

			RectTransform rect = CreaatePanelChild();
			if (rect != null) Selection.activeGameObject = rect.gameObject;
			Undo.RegisterCreatedObjectUndo(rect.gameObject, "Create object");
			rect.gameObject.AddComponent<LayoutItemCreator>().ConvertToLayoutPanel();
			rect.gameObject.AddComponent<LayoutItemCreator>().RemoveMe();
			return rect;
		}

		static RectTransform CreaatePanelChild()
		{
			GameObject go = new GameObject("Panel");
			if (Selection.activeGameObject != null) go.transform.SetParent(Selection.activeGameObject.transform);

			//  else
			//    go.transform.SetParent(CreateCanvasIfNotPresent());
			go.transform.localPosition = Vector2.zero;
			go.transform.localScale = Vector3.one;
			RectTransform rect = go.AddComponent<RectTransform>();
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
			rect.sizeDelta = Vector2.zero;
			Image image = go.AddComponent<Image>();
			image.color = new Color(0, 0, 0, 0.2f);
			image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			image.type = Image.Type.Sliced;
			rect.localScale = Vector3.one;

			//  Selection.activeGameObject = go;

			Undo.RegisterCreatedObjectUndo(go, "Create object");
			return rect;
		}
	}
}
#endif
