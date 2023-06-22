namespace Z.LayoutPanel.EditorCreators
{
	using System.Collections;
	using System.Collections.Generic;

	using UnityEditor;

	using UnityEngine;

	[CustomEditor(typeof(LayoutCreatorAdvanced))]
	public class LayoutCreatorAdvancedEditor : LayoutEditorBase
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			var creator = target as LayoutCreatorAdvanced;
			AddButton(nameof(creator.BackToCreator), creator.BackToCreator);
			AddButton(nameof(creator.CreateHorizontalLayoutSlpit), creator.CreateHorizontalLayoutSlpit);
			AddButton(nameof(creator.CreateVerticalLayoutSlpit), creator.CreateVerticalLayoutSlpit);
			AddButton(nameof(creator.RemoveAllCreators), creator.RemoveAllCreators);
			AddButton(nameof(creator.RemoveAllChildren), creator.RemoveAllChildren);
			AddButton(nameof(creator.LaunchItemCreators), creator.LaunchItemCreators);
			AddButton(nameof(creator.SelectAllCreators), creator.SelectAllCreators);
			
		}
	}
}
