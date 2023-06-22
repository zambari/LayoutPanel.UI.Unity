namespace Z.LayoutPanel.EditorCreators
{
	using UnityEditor;

	using UnityEngine;

	using System;

	public class LayoutEditorBase : Editor
	{
		protected void AddButton(string label, Action action)
		{
			if (GUILayout.Button(label)) action?.Invoke();
		}
		
		protected void AddButton( Action action,string label)
		{
			if (GUILayout.Button(label)) action?.Invoke();
		}
	}
}
