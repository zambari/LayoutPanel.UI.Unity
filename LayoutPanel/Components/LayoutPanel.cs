namespace zUI.LayoutPanelTools
{

	using UnityEngine;
	using UnityEngine.UI;

	using zUI;

	[SelectionBase]
	public class LayoutPanel : MonoBehaviour
	{
		public static System.Action onBorderSizeChange;

		public const string spacerName = "---";

		// defaults END
		public DrawInspectorBg draw;

		[HideInInspector]
		public bool hasSiblingTop;

		[HideInInspector]
		public bool hasSiblingBottom;

		[HideInInspector]
		public bool isAlignedBottom;

		public Text labelText;

		public LayoutElement resizableElement;

		public bool freeMode;

		// defaults
#if UNITY_ANDROID && !UNITY_EDITOR
        static int __borderSpacing = 2;
#else
		static int __borderSpacing = 1;
#endif
		static int _verticalSpacing = 1;

		static int _topHeight = 25;
#if UNITY_ANDROID && !UNITY_EDITOR
        static int _borderSize = 8;
#else
		static int _borderSize = 7;
#endif
		[SerializeField]
		private string _panelName;

		public static int borderSize
		{
			get { return _borderSize; }
			set
			{
				_borderSize = value;
				if (onBorderSizeChange != null) onBorderSizeChange();
			}
		}

		public string panelName
		{
			get { return _panelName; }
			set
			{
				value = value.RemoveTag();
				if (labelText == null)
				{
					var top = GetComponentInChildren<LayoutTopControl>();
					if (top != null)
					{
						var childtop = top.GetComponentInChildren<Text>();
						if (childtop.transform.parent.GetComponent<Button>() != null)
						{
							labelText = childtop;
							labelText = top.GetComponentInChildren<Text>();
							labelText.SetText(value);
							if (labelText == null) Debug.Log("no label", gameObject);
						}
					}
					else Debug.Log("no top control?", gameObject);
				}

				if (labelText != null) labelText.SetText(value);
				_panelName = value;
				name = value;
			}
		}

		public static int borderSpacing
		{
			get { return __borderSpacing; }
			set
			{
				__borderSpacing = value;
				if (onBorderSizeChange != null) onBorderSizeChange();
			}
		}

		public static int verticalSpacing
		{
			get { return _verticalSpacing; }
			set
			{
				_verticalSpacing = value;
				if (onBorderSizeChange != null) onBorderSizeChange();
			}
		}

		public static int topHeight
		{
			get { return _topHeight; }
			set
			{
				_topHeight = value;
				if (onBorderSizeChange != null) onBorderSizeChange.Invoke();
			}
		}

		public void PlaceDropTarget(Transform target, int newSiblingIndex = -1)
		{
			if (target == null)
			{
				Debug.Log("no target");
				return;
			}

			if (LayoutDropTarget.currentTarget != null && LayoutDropTarget.currentTarget.isHorizontal) { }

			transform.SetParent(target);
			if (newSiblingIndex != -1) transform.SetSiblingIndex(newSiblingIndex);
			else transform.SetAsLastSibling();
		}

		private void Start()
		{
			OnValidate();
			if (GetComponentInParent<LayoutColumn>() == null)
			{
				//   Debug.Log("Set freemode "+name);
				freeMode = true;
			}
			else
			{
				//            Debug.Log("set nonfree");
			}
		}

		private void OnValidate()
		{
			//      gameObject.AddOrGetComponent<PanelSaverHelper>();
			if (string.IsNullOrEmpty(_panelName)) _panelName = name;
			panelName = _panelName;
			if (resizableElement == null)
			{
				var les = this.GetComponentsInDirectChildren<LayoutElement>();
				for (int i = 0; i < les.Length; i++)
				{
					if (!les[i].ignoreLayout && les[i].flexibleHeight > 0) resizableElement = les[i];
				}
			}
		}

		private void SetGroupPadding()
		{
			var group = gameObject.AddOrGetComponent<VerticalLayoutGroup>();
			if (group != null)
			{
				var padding = group.padding;
				if (padding.top < LayoutPanel.topHeight)
				{
					padding.top = LayoutPanel.topHeight;
					group.padding = padding;
				}
			}
		}

		public Transform GetTargetTransformForSide(Side side)
		{
			if (transform.parent == null) return transform;

			return transform.parent;
		}

		public LayoutElement GetTargetElementForSide(Side side)
		{
			// if (side == LayoutBorderDragger.Side.Left || side == LayoutBorderDragger.Side.Right)
			//     return transform.parent.GetComponent<LayoutElement>();
			// else
			return resizableElement;
		}

		private void Reset()
		{
			_panelName = name;
		}

		private void OnTransformParentChanged()
		{
			var c = transform.GetComponentInParent<LayoutColumn>();
			if (c != null) c.NotifyOfChange(this);
		}
	}
}
