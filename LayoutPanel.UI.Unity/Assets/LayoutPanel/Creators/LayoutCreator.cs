using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Z.LayoutPanel
{
	using System;

	[DisallowMultipleComponent]
	public class LayoutCreator : LayoutCreatorBase, IProvideLayoutNameHelperSettings, IProvideLayoutGroupSettings
	{
		[Header("Control object naming here:")]
		public LayoutNameHelperSettings namingSettings = new LayoutNameHelperSettings();

		public LayoutGroupSettings groupSettings = new LayoutGroupSettings();

		
		public void AddItemCreator()
		{
			gameObject.AddOrGetComponent<LayoutItemCreator>();
			GameObject.DestroyImmediate(this);
		}
		public void AddSimpleLayoutSpliter()
		{
			gameObject.AddOrGetComponent<LayoutSplitCreator>();
			GameObject.DestroyImmediate(this);
		}

		public void AddEasyAutoLayoutCreator()
		{
			gameObject.AddOrGetComponent<LayoutCreatorEasyAuto>();
			GameObject.DestroyImmediate(this);
		}

		public void AddAdvancedLayoutCreator()
		{
			gameObject.AddOrGetComponent<LayoutCreatorAdvanced>();
			var easy = gameObject.GetComponent<LayoutCreatorEasyAuto>();
			if (easy != null) DestroyImmediate(easy);
			GameObject.DestroyImmediate(this);
		}

		public LayoutNameHelperSettings GetSettings()
		{
			return namingSettings;
		}

		LayoutGroupSettings IProvideLayoutGroupSettings.GetGroupSettings()
		{
			return groupSettings;
		}
	}
}
