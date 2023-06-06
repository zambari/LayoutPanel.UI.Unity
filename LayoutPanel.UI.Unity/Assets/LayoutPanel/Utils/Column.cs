using System.Collections;
using System.Collections.Generic;

namespace Z.LayoutPanel
{
	// using FrameWork.UIBuilder;

	using UnityEngine;
	using UnityEngine.Serialization;
	using UnityEngine.UI;

	public class Column : MonoBehaviour, IBorderControlListener //, IBorderDragReceiverHorizontal
	{
		private LayoutElement layoutElement;

		private const float minWidth = 100;

		public bool isRightOfSpacer;

		public int index;

		public GroupPick group;

		bool hasVertical;

		bool hasHorizontal;

		/// <inheritdoc />
		public void Resize(float amt)
		{
			if (layoutElement == null) layoutElement = GetComponent<LayoutElement>();
			var currentWidth = layoutElement.preferredWidth;
			currentWidth += amt;
			if (currentWidth > minWidth)
			{
				layoutElement.preferredWidth = currentWidth;
			}
		}

		public enum GroupPick
		{
			none,

			mainHoriz,

			column,

			panel
		}

		[SerializeField]
		VerticalLayoutGroup _verticalLayoutGroup;

		[SerializeField]
		HorizontalLayoutGroup _horizontalLayoutGroup;

		[SerializeField]
		LayoutSetup _setup;

		public LayoutElement targetLayoutElement;

		public bool applySettingsToVerticalLayout = true;

		public bool applySettingsToHorizontalLayout = true;

		public Transform _content;

		public Transform content
		{
			get
			{
				if (group == GroupPick.column)
					if (_content == null)
					{
						if (targetLayoutElement != null) content = targetLayoutElement.transform;

						//var ver = GetComponent<VerticalLayoutGroup>();
						//if (ver != null) content = ver.transform as RectTransform;
					}

				return _content;
			}
			set { _content = value; }
		}

		private void Reset()
		{
			if (targetLayoutElement == null) targetLayoutElement = GetComponentInChildren<LayoutElement>();
		}

		private void OnValidate()
		{
			if (targetLayoutElement == null) targetLayoutElement = GetComponentInChildren<LayoutElement>();
		}

		LayoutSetup setup
		{
			get
			{
				if (_setup == null)
				{
					//if (Lay)
					_setup = LayoutSetupProvider.setup;
				}

				return _setup;
			}
		}

		private void OnEnable()
		{
			if (!gameObject.PrefabModeIsActive())
			{
				_verticalLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();
				_horizontalLayoutGroup = GetComponentInChildren<HorizontalLayoutGroup>();
				LayoutSetupProvider.instance.onChanged -= UpdateLayoutSetupObject;
				LayoutSetupProvider.instance.onChanged += UpdateLayoutSetupObject; //

				//
				LayoutSetupProvider.Subscribe(this);
			}
		}

		private void OnDisable()
		{
			if (!gameObject.PrefabModeIsActive())
			{
				LayoutSetupProvider.instance.onChanged -= UpdateLayoutSetupObject;

				// LayoutBorderControl.UnsSubscribe(this);
			}
		}

		public void UpdateLayoutSetupObject(LayoutSetup setup)
		{
			LayoutGroupSettings thisGroupSettings = null;
			if (group == GroupPick.none) return;
			if (gameObject.PrefabModeIsActive()) return;

			if (group == GroupPick.mainHoriz) thisGroupSettings = setup.mainhorizontalSettings;
			if (group == GroupPick.panel) thisGroupSettings = setup.panelGroupSettings;
			if (group == GroupPick.column) thisGroupSettings = setup.columnGroupSettings;

			// Debug.Log
			if (applySettingsToVerticalLayout && _verticalLayoutGroup != null)
			{
				thisGroupSettings.ApplyTo(_verticalLayoutGroup, setup);
			}

			if (applySettingsToHorizontalLayout && _horizontalLayoutGroup != null)
			{
				thisGroupSettings.ApplyTo(_horizontalLayoutGroup, setup);
			}
		}
	}
}
