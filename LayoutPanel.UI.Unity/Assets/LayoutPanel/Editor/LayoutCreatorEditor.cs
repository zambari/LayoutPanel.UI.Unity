namespace Z.LayoutPanel.EditorCreators
{
	using System.Collections;
	using System.Collections.Generic;

	using UnityEditor;

	using UnityEngine;

	using Z.LayoutPanel;

	[CustomEditor(typeof(LayoutCreator))]
	public class LayoutCreatorEditor : LayoutEditorBase
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			var creator = target as LayoutCreator;
			AddButton(nameof(creator.AddSimpleLayoutSpliter),creator.AddSimpleLayoutSpliter);
			AddButton(nameof(creator.AddEasyAutoLayoutCreator),creator.AddEasyAutoLayoutCreator);
			AddButton(nameof(creator.AddAdvancedLayoutCreator),creator.AddAdvancedLayoutCreator);
			// AddButton(nameof(creator.AddSimpleLayoutSpliter),creator.AddSimpleLayoutSpliter);
		}
	}
}
