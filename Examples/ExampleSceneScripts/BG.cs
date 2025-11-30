namespace zUI.LayoutPanelTools.Examples
{
	using UnityEngine;
	using UnityEngine.UI;
#if UNITY_EDITOR
	using UnityEditor;

	[RequireComponent(typeof(Image))]
	public class BG : MonoBehaviour
	{
		void Reset()
		{
			Image image = GetComponent<Image>();
			if (image != null)
			{
				Undo.RegisterCompleteObjectUndo(gameObject, "BG");
				name = "BG";
				transform.SetAsFirstSibling();
				image.color = new Color(Random.value * 0.1f, Random.value * 0.1f, Random.value * 0.1f);
				image.raycastTarget = false;
				RectTransform rect = GetComponent<RectTransform>();
				rect.anchorMin = Vector2.zero;
				rect.anchorMax = Vector2.one;
				rect.offsetMin = Vector2.zero;
				rect.offsetMax = Vector2.zero;
			}

			EditorApplication.delayCall += () => DestroyImmediate(this);
		}
	}
#endif
}
