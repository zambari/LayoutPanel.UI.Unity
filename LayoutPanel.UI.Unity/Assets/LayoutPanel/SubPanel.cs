using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

// using zUI;


namespace Z.LayoutPanel
{
	[ExecuteInEditMode]
	public class SubPanel : UIBase //, IProvideTopColor, IContextMenuBuilder
	{
		// [SerializeField]
		// private string _label;

		// protected Text text { get { if (_text == null) _text = GetComponentInChildren<Text>(); return _text; } }

		// [Space]
		// public Text _text;

		public IFoldController foldControl
		{
			get { return _foldControl; }
			set { _foldControl = value; }
		}

		IFoldController _foldControl;

		protected VerticalLayoutGroup layoutGroup
		{
			get
			{
				if (_layoutGroup == null) _layoutGroup = GetComponent<VerticalLayoutGroup>();
				return _layoutGroup;
			}
		}

		VerticalLayoutGroup _layoutGroup;

		Image _topImage;

		public SubPanel parentSubPanel;


		// public Image image { get { if (_imageForColors == null) _imageForColors = GetComponentInChildren<Image>(); return _imageForColors; } }

		// [SerializeField]
		// Image _imageForColors;

		public Color topColor
		{
			get
			{
				if (topImage == null) return default(Color);

				return topImage.color;
			}
			set
			{
				if (topImage != null) topImage.color = value;
			}
		}

		public int indentation
		{
			get
			{
				if (layoutGroup == null) return 0;

				return layoutGroup.padding.left;
			}
			set
			{
				if (layoutGroup == null) return;

				if (layoutGroup != null)
				{
					RectOffset padding = layoutGroup.padding;
					padding.left = value;
					layoutGroup.padding = padding;
				}
			}
		}

		Image topImage
		{
			get
			{
				if (_topImage == null)
					if (foldControl != null)
						_topImage = foldControl.gameObject.GetComponent<Image>();
				return _topImage;
			}
		}

		[LPExposeMethodInEditor]
		void SetPadding()
		{
			RectOffset padding = layoutGroup.padding;
			var left = padding.left;
			Debug.Log("padding " + padding.left);
			padding.left++;
			layoutGroup.padding = padding;
		}

		protected virtual void OnEnable()
		{
			GetParentPanel();
		}

		// [ExposeMethodInEditor]
		void GetParentPanel()
		{
			if (transform.parent != null) parentSubPanel = transform.parent.GetComponentInChildren<SubPanel>();
		}

		protected override void Reset()
		{
			base.Reset();
			GetParentPanel();
			foldControl = GetComponentInChildren<IFoldController>();
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			GetParentPanel();
			if (foldControl == null) foldControl = GetComponentInChildren<IFoldController>();
		}

		// public virtual void BuildContextMenu(PrefabProvider prefabs, Transform target)
		// {
		// 	if (image != null)
		// 	{
		// 		prefabs.GetButton(target, name+" RandomizeColor").AddCallback(() =>
		// 		{
		// 			color = color.Randomize();
		// 		});
		// 	}
		// }
	}
}
