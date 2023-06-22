namespace Z.LayoutPanel.EditorCreators
{
	using System.Collections;
	using System.Collections.Generic;

	using UnityEditor;

	using UnityEngine;

	using Z.LayoutPanel;

	[CustomEditor(typeof(LayoutItemCreator))]
	public class LayoutItemCreatorEditor : LayoutEditorBase
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			var creator = target as LayoutItemCreator;
			AddButton(nameof(creator.Add_Child_And_ConvertToPanel), creator.Add_Child_And_ConvertToPanel);
			AddButton(nameof(creator.Convert_This_To_LayoutPanel), creator.Convert_This_To_LayoutPanel);
			AddButton(nameof(creator.Sub_Divde_Layout), creator.Sub_Divde_Layout);
			AddButton(nameof(creator.DisassemblePanel), creator.DisassemblePanel);
		}
	}
}
