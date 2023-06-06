using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Z.LayoutPanel
{
	[DisallowMultipleComponent]
	public class LayoutCreatorEasyAuto : LayoutCreatorBase
	{
		public enum StartType
		{
			Horizontal,

			Vertical
		}

		[ClickableEnum]
		public StartType startType;

		[Range(1, 4)]
		public int splitCountMIn = 2;

		[Range(1, 5)]
		public int splitCountMax = 3;

		[Range(1, 4)]
		public int splitDepth = 2;

		[Range(0, 1)]
		[SerializeField]
		float splitChance = 0.7f;

		public bool addRandomTexstOnCreate;

		[LPExposeMethodInEditor]
		private void BackToCreator()
		{
			if (gameObject.GetComponent<LayoutCreator>() == null) gameObject.AddComponent<LayoutCreator>();
			GameObject.DestroyImmediate(this);
		}

		[LPExposeMethodInEditor]
		public void CreateRandomLayout()
		{
			var creator = gameObject.AddOrGetComponent<LayoutCreatorAdvanced>();
			bool thisHorizontal = startType == StartType.Horizontal;
			var items = new List<LayoutItemCreator>();
			int thisSplitCount = Random.Range(splitCountMIn, splitCountMax);
			if (thisHorizontal)
			{
				items = gameObject.CreateHorizontalLayoutCreators( thisSplitCount);
			}
			else
			{
				items = gameObject.CreateVerticalLayoutCreators( thisSplitCount);
			}

			for (int i = 0; i < thisSplitCount; i++)
			{
				thisHorizontal = !thisHorizontal;
				var thisItems = new List<LayoutItemCreator>();
				for (int j = 0; j < items.Count; j++)
				{
					var thisItem = items[j];
					thisItem.addRandomTexstOnCreate = addRandomTexstOnCreate;
					if (Random.value < splitChance)
					{
						thisItems.AddRange(thisItem.SubDivdeLayout(Random.Range(splitCountMIn, splitCountMax)));
					}
				}

				items = new List<LayoutItemCreator>(thisItems);
			}

			creator.LaunchItemCreators();

			// DestroyImmediate(creator);
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
		}
	}
}
