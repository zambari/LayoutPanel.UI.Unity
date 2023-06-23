namespace Z.LayoutPanel.Editor
{
	using System;

	using UnityEngine.UIElements;

	public static class EditorUIExtension
	{
		public static Label Larger(this Label label)
		{
			return label.SetTextSize(15);
		}

		public static Label SetTextSize(this Label label, int fontSize)
		{
			label.style.fontSize = fontSize;
			return label;
		}

		public static Button AddButton(this VisualElement content, string label, Action onClick = null)
		{
			var button = new Button(onClick) { text = label };
			content.Add(button);
			return button;
		}
	}
}
