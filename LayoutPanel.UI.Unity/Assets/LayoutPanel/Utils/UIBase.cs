namespace Z.LayoutPanel
{
	using System.Collections;
	using System.Collections.Generic;

	using UnityEngine;
	using UnityEngine.UI;

	public abstract class UIBase : MonoBehaviour //, IHasContent
	{
#if LAYOUT_PANEL
		public DrawInspectorBg draw;
#endif

		[SerializeField]
		private string _label;

		public UIObjectReferences objectReferences = new UIObjectReferences();

		[SerializeField]
		[HideInInspector]
		private string _lastlabel;

		public string label
		{
			get
			{
				if (string.IsNullOrEmpty(_label)) return name;

				return _label;
			}
			set
			{
				if (_lastlabel != value)
				{
					_label = value;
					_lastlabel = value;
					if (objectReferences.autoSetText) text.SetText(value);
					if (!gameObject.PrefabModeIsActive())
					{
						if (objectReferences.autoName)
							if (name != value)
								name = value;
					}
				}
			}
		}

		public Button button
		{
			get
			{
				if (objectReferences._button == null) objectReferences._button = GetComponentInChildren<Button>();
				return objectReferences._button;
			}
			set { objectReferences._button = value; }
		}

		public Text text
		{
			get
			{
				if (objectReferences._text == null) objectReferences._text = GetComponentInChildren<Text>();
				return objectReferences._text;
			}
			set { objectReferences._text = value; }
		}

		public RectTransform rectTransform
		{
			get
			{
				if (objectReferences._rectTransform == null)
					objectReferences._rectTransform = GetComponent<RectTransform>();
				return objectReferences._rectTransform;
			}
		}

		public Image image
		{
			get
			{
				if (objectReferences._image == null) objectReferences._image = GetComponent<Image>();
				return objectReferences._image;
			}
		}

		public virtual Color color
		{
			get { return image.color; }
			set { image.color = color; }
		}

		public Transform content
		{
			get { return objectReferences._content; }
			set { objectReferences._content = value; }
		}

		public RectTransform contentRect
		{
			get { return objectReferences._content as RectTransform; }
		}

		protected virtual void Reset()
		{
			Debug.Log("reset called");
			objectReferences.AutoFill(this);
		}

		protected virtual void OnValidate()
		{
			if (objectReferences.autoFill) objectReferences.AutoFill(this);
			label = label;
		}
	}
}
