using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Z.LayoutPanel
{
	using System;

	using UnityEditor;

	[CreateAssetMenu]
	public class LayoutSetup : ScriptableObject
	{
		[Range(0, 30)]
		public int panelSpacing = 15;

		public Action onChanged;

		//	[Range(0, 30)]
		public int borderSize
		{
			get { return borderSetup.borderSize; }
			set { borderSetup.borderSize = value; }
		}

		[Header("borders")]
		public BorderSetup borderSetup = new BorderSetup();

		[Range(-10, 10)]
		public int panelSpacingOffserHoriz = 0;

		[Space]
		public int topHeight = 15;

		public LayoutGroupSettings mainhorizontalSettings = new LayoutGroupSettings();

		public LayoutGroupSettings columnGroupSettings = new LayoutGroupSettings();

		[HideInInspector]
		public LayoutGroupSettings panelGroupSettings = new LayoutGroupSettings();

		public void OnValidate()
		{
			borderSetup.OnValidate();
			mainhorizontalSettings.spacing = panelSpacing + panelSpacingOffserHoriz;
			columnGroupSettings.spacing = panelSpacing;
#if UNITY_EDITOR
			EditorApplication.delayCall += () => onChanged?.Invoke();
#endif

			//	LayoutBorderControl.BroadcastSetup(this);
		}
	}
}
