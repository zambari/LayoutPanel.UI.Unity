using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

namespace Z.LayoutPanel
{
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public class LayoutItemCreator : MonoBehaviourWithBg
	{
		public LayoutBorderHide.BorderHideMode borderHideMode;

		private Color textColor = Color.white * 0.8f;

		private LayoutCreatorAdvanced layoutCreator;

		public bool removeMeWhenDone = false;

		public bool addNameMangler = false;

		[SerializeField]
		[HideInInspector]
		LayoutNameHelper nameHelper;

		[Range(2, 5)]
		public int subdivideCount = 2;

		public bool addRandomTexstOnCreate = true;

		static int randCount;

		private void Reset()
		{
			var layoutSteup = GetComponentInParent<LayoutSetupProvider>();
			if (layoutSteup == null)
			{
				var parent = transform;
				while (parent.parent != null) parent = parent.parent;
				parent.gameObject.AddComponent<LayoutSetupProvider>();
			}

			var allcoponents = gameObject.GetComponents<Component>();
			int thispos = 0;
			for (int i = 0; i < allcoponents.Length; i++)
			{
				if (allcoponents[i] == this) thispos = i;
			}

			this.MoveComponent(-thispos);

			RandomizeColor();
			subdivideCount = 2 + Random.Range(0, 2); //';//' + Random.value > 0.8f ? 1 : 0;
			UpdateNameHelper();
		}

		private void OnEnable()
		{
			UpdateNameHelper();
		}

		private void RandomizeColor()
		{
			Image image = GetComponent<Image>();
			if (image != null && image.color == Color.white)
			{
#if UNITY_EDITOR
				Undo.RegisterCompleteObjectUndo(image, "color");
#endif
				Color color =
					(Color.black +
					 new Color(0, Random.value * .2f, 0) +
					 Color.white * 0.4f); // Color.white * 0.2f).Randomize(1,1,.3f);
				color.a = 0.7f;
				image.color = color.Randomize(2, .4f, .3f);
			}
		}

		private void Awake()
		{
			layoutCreator = GetComponentInParent<LayoutCreatorAdvanced>();
			if (layoutCreator != null)
			{
				textColor = layoutCreator.textColor;
			}
		}

		private void UpdateNameHelper()
		{
			if (addNameMangler)
			{
				if (nameHelper == null) nameHelper = gameObject.AddOrGetComponent<LayoutNameHelper>();
				nameHelper.UpdateName();
			}
		}

		public List<LayoutItemCreator> SubDivdeLayout(int count)
		{
			Image image = GetComponent<Image>();
			if (image != null) image.enabled = false;
			var list = new List<LayoutItemCreator>();

			if (addNameMangler) gameObject.AddOrGetComponent<LayoutNameHelper>();
			var hg = transform.parent.GetComponent<HorizontalLayoutGroup>();
			var vg = transform.parent.GetComponent<VerticalLayoutGroup>();
			bool horizontal = vg != null;
			var subs = gameObject.SplitToLayout(horizontal, count);
			foreach (var g in subs)
			{
				list.Add(g.AddComponent<LayoutItemCreator>());
			}

#if UNITY_EDITOR

			Selection.objects = subs.ToArray();
			EditorApplication.delayCall += () =>
										   {
											   if (nameHelper != null) nameHelper.UpdateName();
										   };
#else
            UpdateNameHelper();
#endif

			RemoveMe();
			return list;
		}

		public void RemoveMe()
		{
			if (!removeMeWhenDone) return;
#if UNITY_EDITOR
			EditorApplication.delayCall += () =>
										   {
											   if (this != null)
											   {
												   if (nameHelper != null)
													   EditorApplication.delayCall += () =>
													   {
														   if (nameHelper != null) nameHelper.UpdateName();
													   };

												   Undo.DestroyObjectImmediate(this);
											   }
										   };
#endif
		}

		public void Add_Child_And_ConvertToPanel()
		{
			var child = GetComponent<RectTransform>().AddChild();
			child.name = "[LayoutPanel]";
#if UNITY_EDITOR
			Undo.RegisterCreatedObjectUndo(child.gameObject, "child");
#endif
			var panel = child.ConvertToLayoutPanel(true, textColor);
			if (addRandomTexstOnCreate)
			{
				panel.content.AddRandomTexts();
				var contentSize = child.gameObject.AddComponent<ContentSizeFitter>();
				contentSize.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			}
		}

		public void Convert_This_To_LayoutPanel()
		{
			gameObject.ConvertToLayoutPanel();
		}

		public void Sub_Divde_Layout()
		{
			SubDivdeLayout(subdivideCount);
		}
#if UNITY_EDITOR
		public void DisassemblePanel()
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				Undo.DestroyObjectImmediate(transform.GetChild(i).gameObject);
				var panel = GetComponent<LayoutPanel>();
				if (panel) Undo.DestroyObjectImmediate(panel);
			}
		}
#endif

		//
		//
		// [LPExposeMethodInEditor]
		// private void RemoveBorders()
		// {
		// 	var panel = GetComponent<LayoutPanel>();
		// 	if (panel != null)
		// 	{
		// 		if (panel.layoutElement != null) Undo.DestroyObjectImmediate(panel.layoutElement.gameObject);
		// 		Undo.DestroyObjectImmediate(panel);
		// 	}
		//
		// 	var bh = GetComponent<LayoutBorderHide>();
		//
		// 	if (bh != null)
		// 	{
		// 		bh.borderHideMode = LayoutBorderHide.BorderHideMode.Visible;
		// 		Undo.DestroyObjectImmediate(bh);
		// 	}
		//
		// 	for (int i = transform.childCount - 1; i >= 0; i--)
		// 	{
		// 		var thisChild = transform.GetChild(i);
		// 		if (thisChild.GetComponent<LayoutTopControl>() != null ||
		// 			thisChild.GetComponent<LayoutBorderDragger>() != null)
		// 		{
		// 			Undo.DestroyObjectImmediate(thisChild.gameObject);
		// 		}
		// 	}
		// }
	}
}
