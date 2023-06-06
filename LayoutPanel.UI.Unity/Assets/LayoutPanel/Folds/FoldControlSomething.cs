using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Z;
//
// Old version
// Levaing it in for now just in case

namespace Z.LayoutPanel
{
	using System.Threading.Tasks;

	public class FoldControlSomething : FoldControlBase
	{
		public List<GameObject> objectsToIgnore = new List<GameObject>();

		public List<GameObject> objectsToToggle;

		// [ExposeMethodInEditor]
		// public override void ToggleFold()
		// {
		// 	bool newFold = !isFolded;
		// 	if (transform.parent == null) return;
		//
		// 	if (foldChildren || Input.GetKey(KeyCode.LeftAlt))
		// 	{
		// 		var otherFolds = foldRoot.GetComponentsInChildrenIncludingInactive<IFoldController>();
		// 		otherFolds.Add(this);
		// 		StartCoroutine(FoldAll(otherFolds, newFold, newFold));
		//
		// 		// Debug.Log($"allunfold {otherFolds.Count}");
		// 		//SetFold(newFold);
		// 	}
		// 	else SetFold(newFold);
		// }


		protected void NaiveFold(bool shouldHide)
		{
			GetObjectsToToggle();
			if (objectsToToggle.Count == 0)
			{
				Debug.Log("no objects", gameObject);
			}

			foreach (var o in objectsToToggle)
			{
				o.SetActive(!shouldHide);
			}

			isFolded = shouldHide;
		}

		protected void GetObjectsToToggle()
		{
			objectsToToggle = new List<GameObject>();
			if (transform.parent == null) return;

			if (foldRoot == null) FindRoot();
			for (int i = 0; i < foldRoot.childCount; i++)
			{
				var thischild = foldRoot.GetChild(i);
				if (thischild == transform) continue;

				var le = thischild.GetComponent<LayoutElement>();
				if (le != null && le.ignoreLayout) continue;

				objectsToToggle.Add(thischild.gameObject);
			}
		}

		protected void GetIgnoredObjects()
		{
			if (!objectsToIgnore.Contains(gameObject)) objectsToIgnore.Add(gameObject);

			// if (foldButton != null)
			// {
			//     var parent = foldButton.transform.parent;
			//     while (parent != transform && parent != null)
			//     {
			//         if (objectsToIgnore.Contains(parent.gameObject)) break;
			//         objectsToIgnore.Add(parent.gameObject);
			//         parent = parent.parent;
			//     }
			//        Debug.Log($"added {objectsToIgnore.Count}",gameObject);
			// }
			//   else Debug.Log("no button",gameObject);
		}

		protected override void Start()
		{
			base.Start();
			if (transform.parent == null) return;

			if (foldButton != null) foldButton.onClick.AddListener(ToggleFold);
			if (foldRoot == null) FindRoot();
			GetIgnoredObjects();
			if (startFolded) Fold();
			SetFoldLabel();
		}

		protected virtual void Reset()
		{
			if (foldRoot == null) foldRoot = transform.parent;

			if (foldButton == null && transform.childCount > 0)
				foldButton = transform.GetChild(0).GetComponentInChildren<Button>();
			if (_foldLabelText == null && foldButton != null)
				_foldLabelText = foldButton.GetComponentInChildren<Text>();
			if (_foldLabelText != null) _foldLabelText.text = GetFoldString();
			GetIgnoredObjects();
		}

		protected bool CanStartCoroutine()
		{
			if (!isActiveAndEnabled) return false;

			return (Application.isPlaying);
		}

		protected override void FindRoot()
		{
			//if (foldParent) 
			if (foldRoot != null) return;

			if (transform.parent.name.Contains("Frame")) foldRoot = transform.parent.parent;
			else foldRoot = transform.parent;
			;

			//return transform;
		}

		public override Task SetFoldAsync(bool newFold)
		{
			if (!gameObject.activeInHierarchy) return Task.CompletedTask;

			NaiveFold(newFold);
			SetFoldLabel();
			return Task.CompletedTask;
		}
	}
}
