using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Z.LayoutPanel
{
	using System.Linq;

	[DisallowMultipleComponent]
	public class LayoutSplitCreator : LayoutCreatorBase
	{
		public enum SplitType
		{
			Horizontal,

			Vertical
		}

		public bool addMoreSplittersToCreatedPanels;

		public bool runItemCreatorsAfterSplit = false;

		private bool checkForChildren;

		private bool onlyCreateBorders;

		[ClickableEnum]
		public SplitType splitType;

		[Range(1, 6)]
		public int splitCount =
			2;

		private bool addRandomTexstOnCreate = true;

		protected override void Reset()
		{
			base.Reset();
			splitCount += Random.Range(0, 3);
		}

		[LPExposeMethodInEditor]
		public void Split()
		{
			bool thisHorizontal = splitType == SplitType.Horizontal;
			var items = new List<LayoutSplitCreator>();

			if (thisHorizontal)
			{
				items = gameObject.CreateHorizontalLayoutSplits(splitCount);
			}
			else
			{
				gameObject.AddOrGetComponent<Column>();
				items = gameObject.CreateVerticalLayoutSplits(splitCount);
			}

			var listForSelection = new List<Object>();
			listForSelection.AddRange(items.Select(x => x.gameObject));

			for (int i = 0; i < splitCount; i++)
			{
				thisHorizontal = !thisHorizontal;
				var thisItems = new List<LayoutItemCreator>();
				for (int j = 0; j < items.Count; j++)
				{
					var thisItem = items[j];
					thisItem.addRandomTexstOnCreate = addRandomTexstOnCreate;
				}

				// items = new List<LayoutSplitCreator>(thisItems);
			}

			var image = GetComponent<Image>();
			if (image != null) image.enabled = false;

			if (runItemCreatorsAfterSplit) gameObject.LaunchItemCreators(checkForChildren, onlyCreateBorders);

#if UNITY_EDITOR
			UnityEditor.Selection.objects = listForSelection.ToArray();
#endif
		}

		[LPExposeMethodInEditor]
		private void AddItemCreator()
		{
			var component = gameObject.AddOrGetComponent<LayoutItemCreator>();
#if UNITY_EDITOR
			Undo.RegisterCreatedObjectUndo(component, "");
			Undo.DestroyObjectImmediate(this);
#endif
		}

		[LPExposeMethodInEditor]
		public void RemoveAllChildren()
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
#if UNITY_EDITOR
				Undo.DestroyObjectImmediate(transform.GetChild(i).gameObject);
#else
            DestroyImmediate (transform.GetChild (i).gameObject);
#endif
			name = "Empty";
		}

		[LPExposeMethodInEditor]
		public void RemoveCreators()
		{
			var creators = GetComponentsInChildren<LayoutCreatorBase>();
			foreach (var thisCreator in creators)
			{
#if UNITY_EDITOR
				Undo.DestroyObjectImmediate(thisCreator);
#endif
			}
		}

		[LPExposeMethodInEditor]
		private void BackToCreator()
		{
			if (gameObject.GetComponent<LayoutCreator>() == null) gameObject.AddComponent<LayoutCreator>();
			GameObject.DestroyImmediate(this);
		}
	}
}
