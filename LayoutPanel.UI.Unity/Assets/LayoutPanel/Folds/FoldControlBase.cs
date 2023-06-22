namespace Z.LayoutPanel
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using UnityEngine;
	using UnityEngine.UI;

	public abstract class FoldControlBase : MonoBehaviour, IFoldController
	{
		[Header("will use parent if null")]
		public Transform foldRoot;

		public Button foldButton;

		public Text _foldLabelText;

		public bool isLocked;

		protected int foldTimeMs = 100;

		public static string labelUnfolded
		{
			get { return "▼"; }
		}

		public static string labelFolded
		{
			get { return "◀"; }
		}

		public static string labelFoldedAlt
		{
			get { return "▶"; }
		}

		public bool _isFolded;

		public bool startFolded;

		public bool foldChildren;

		bool isLeftSide = false;

		public Action<bool> onFold { get; set; }

		public string GetFoldString()
		{
			if (isFolded) return isLeftSide ? labelFoldedAlt : labelFolded; //▲ ▶ ◀ ▼
			else return labelUnfolded;
		}

		protected virtual void FindRoot()
		{
			if (transform.parent != null) foldRoot = transform.parent.parent;
		}

		private RectTransform _rect;

		public RectTransform rect
		{
			get
			{
				if (_rect == null) _rect = GetComponent<RectTransform>();
				return _rect;
			}
		}

		public bool isFolded
		{
			get { return _isFolded; }
			set
			{
				_isFolded = value;

				// SetFold(value);

				if (onFold != null)
				{
					Debug.Log("event listener");
				}

				SetFoldLabel();
			}
		}

		protected void SetFoldLabel()
		{
			_foldLabelText.SetText(GetFoldString());
		}

		private List<IFoldController> grabbedList;

		// [LPExposeMethodInEditor]
		public virtual async void ToggleFold()
		{
			if (isLocked)
			{
				Debug.Log("folding is currently locked");
				await Task.CompletedTask;
			}

			bool newFold = !isFolded;
			if (foldRoot == null) FindRoot();

			// if (foldChildren || Input.GetKey(KeyCode.LeftAlt))
			if (foldChildren)
			{
				isLocked = true;
				grabbedList = foldRoot.GetComponentsInChildrenIncludingInactive<IFoldController>();
				await FoldAll(grabbedList, newFold, newFold);
				isLocked = false;
			}
			else
			{
				await SetFoldAsync(newFold);
			}

			onFold?.Invoke(newFold);
		}

		async Task FoldAll(List<IFoldController> list, bool newFold, bool reverse)
		{
			if (isLocked) return;

			int stepDelay = foldTimeMs * 2 / foldRoot.childCount;
			isLocked = true;
			if (reverse)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (list[i] == (object)this) isLocked = false;
					await list[i].SetFoldAsync(newFold);
					await Task.Delay(stepDelay);
				}
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] == (object)this) isLocked = false;
					await list[i].SetFoldAsync(newFold);
					await Task.Delay(stepDelay);
				}
			}

			isLocked = false;
		}

		protected virtual void Start()
		{
			if (foldButton == null) foldButton = GetComponentInChildren<Button>();
			if (foldButton != null) foldButton.onClick.AddListener(ToggleFold);

			if (_foldLabelText == null)
			{
				var texts = GetComponentsInChildren<Text>();
				if (texts.Length > 0) _foldLabelText = texts[texts.Length - 1];
			}

			if (startFolded) Fold();
			SetFoldLabel();
		}

		public virtual void SetFold(bool newFold) { }

		/// <inheritdoc />
		public virtual Task SetFoldAsync(bool newFold)
		{
			SetFold(newFold);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public void Fold()
		{
			SetFold(true);
		}

		public void UnFold()
		{
			SetFold(false);
		}
	}
}
