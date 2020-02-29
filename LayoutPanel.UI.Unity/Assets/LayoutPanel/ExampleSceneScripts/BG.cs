using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LayoutPanelDependencies;
#if UNITY_EDITOR
using UnityEditor;
using Z;
[RequireComponent(typeof(Image))]
public class BG : MonoBehaviour
{
    void Reset()
    {
        Canvas canvas= GetComponent<Canvas>();
        if (canvas!=null)
        {
            var newRect=canvas.GetComponent<RectTransform>().AddImageChild();
            newRect.gameObject.AddComponent<BG>();
            Selection.activeGameObject=newRect.gameObject;
            EditorApplication.delayCall+=()=>DestroyImmediate(this);
            return;
        }
        Image image = GetComponent<Image>();
        if (image != null)
        {
            Undo.RegisterCompleteObjectUndo(gameObject, "BG");
            name = "BG";
            transform.SetAsFirstSibling();
            image.color = new Color(Random.value * 0.1f, Random.value * 0.1f, Random.value * 0.1f );
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