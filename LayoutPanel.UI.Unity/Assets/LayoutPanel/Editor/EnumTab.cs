namespace Z.LayoutPanel.EditorCreators
{
	using System;

	using UnityEditor.Graphs;

	using UnityEngine;
	using UnityEngine.UIElements;

	public class LayoutTab : EnumTab<LayoutGroupType>
	{
		/// <inheritdoc />
		public LayoutTab(string label, Action<LayoutGroupType> OnValueChanged = null) : base(label, OnValueChanged) { }

		/// <inheritdoc />
		public LayoutTab(string label, LayoutGroupType startValue, Action<LayoutGroupType> OnValueChanged = null) :
			base(label, startValue, OnValueChanged) { }
	}

	public class EnumTab<T> where T : Enum
	{
		private readonly string[] names;

		private int _value;

		private VisualElement[] highlightElements;

		public event Action<int> onValueChangedInt;

		public event Action<T> onValueChanged;

		public event Action<string> onValueChangedString;

		private string label;

		public int value
		{
			get { return _value; }
			set
			{
				_value = value;
				UpdateVisuals();
				onValueChangedString?.Invoke(names[value]);
				onValueChangedInt?.Invoke(value);
				onValueChanged?.Invoke(Enum.Parse<T>(names[value]));
			}
		}

		public EnumTab(string label, Action<T> OnValueChanged = null)
		{
			names = Enum.GetNames(typeof(T));
			this.label = label;
			if (OnValueChanged != null) onValueChanged += OnValueChanged;
		}

		public EnumTab(string label, T startValue, Action<T> OnValueChanged = null)
		{
			names = Enum.GetNames(typeof(T));
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i] == startValue.ToString()) value = i;
			}

			this.label = label;
			if (OnValueChanged != null) onValueChanged += OnValueChanged;
		}

		private VisualElement GetVisualElement()
		{
			VisualElement content = new VisualElement();

			content.style.flexDirection = FlexDirection.Row;
			content.style.flexGrow = 1;
			content.style.marginLeft = 30;
			highlightElements = new VisualElement[names.Length];
			content.Add(new Label(label));
			for (int i = 0; i < highlightElements.Length; i++)
			{
				int k = i;
				var thisButton = new Button(() => { value = k; }) { text = names[i] };
				thisButton.style.flexGrow = 1;
				content.Add(thisButton);

				var thisHighlight = new VisualElement();
				thisButton.Add(thisHighlight);

				thisHighlight.style.position = Position.Absolute;
				thisHighlight.style.height = 2;
				thisHighlight.style.left = 2;
				thisHighlight.style.right = 2;
				thisHighlight.style.bottom = 2;
				thisButton.style.height = 30;
				highlightElements[i] = thisHighlight;
			}

			UpdateVisuals();
			return content;
		}

		void UpdateVisuals()
		{
			if (highlightElements == null) return;

			for (int i = 0; i < highlightElements.Length; i++)
			{
				if (i == value)
					highlightElements[i].style.backgroundColor = new Color(0, 1, 0.5f); //Color.black * .4f);
				else highlightElements[i].style.backgroundColor = new Color(0, 0, 0, 0);
			}
		}

		public static implicit operator VisualElement(EnumTab<T> t) => t.GetVisualElement();
	}
}
