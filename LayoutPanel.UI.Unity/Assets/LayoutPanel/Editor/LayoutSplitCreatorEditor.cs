namespace Z.LayoutPanel.EditorCreators
{
	using System.Collections;
	using System.Collections.Generic;

	using UnityEditor;

	using UnityEngine;

	using Z.LayoutPanel;

	[CustomEditor(typeof(LayoutSplitCreator))]
	public class LayoutSplitCreatorEditor : LayoutEditorBase
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			var creator = target as LayoutSplitCreator;
			AddButton(nameof(creator.BackToCreator), creator.BackToCreator);
			AddButton(nameof(creator.Split), creator.Split);
			AddButton(nameof(creator.AddItemCreator), creator.AddItemCreator);
			AddButton(nameof(creator.RemoveAllChildren), creator.RemoveAllChildren);
			AddButton(nameof(creator.RemoveCreators), creator.RemoveCreators);
			AddButton(nameof(creator.BackToCreator), creator.BackToCreator);
		}
	}
}
