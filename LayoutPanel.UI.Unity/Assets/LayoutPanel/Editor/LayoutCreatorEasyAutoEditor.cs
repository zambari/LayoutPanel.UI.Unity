namespace Z.LayoutPanel.EditorCreators
{
	using System.Collections;
	using System.Collections.Generic;

	using UnityEditor;

	using UnityEngine;

	using Z.LayoutPanel;
	[CustomEditor(typeof(LayoutCreatorEasyAuto))]
	public class LayoutCreatorEasyAutoEditor : LayoutEditorBase
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			var creator = target as LayoutCreatorEasyAuto;
			AddButton(nameof(creator.BackToCreator),creator.BackToCreator);
			AddButton(nameof(creator.CreateRandomLayout),creator.CreateRandomLayout);
			AddButton(nameof(creator.RemoveAllChildren),creator.RemoveAllChildren);
			//AddButton(nameof(creator.BackToCreator),creator.BackToCreator);
			
		}
	}
}
