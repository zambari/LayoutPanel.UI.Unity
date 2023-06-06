using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using zUI;

namespace Z.LayoutPanel
{
	public class LayoutFoldController : SimpleFoldController
	{
		[SerializeField]
		public bool ignoreSavedKeepDisabledList = true;

		public Text foldLabelText
		{
			get { return _foldLabelText; }
			set
			{
				_foldLabelText = value;
#if UNITY_EDITOR
				UnityEditor.EditorApplication.delayCall += () => _foldLabelText.SetText(GetFoldString());
#else
          SetFoldLabel();
#endif
			}
		}

		int GetWaitAfterNObjects()
		{
			int objectsPerFrame = transform.childCount / 5;
			if (objectsPerFrame < 1) objectsPerFrame = 1;
			return objectsPerFrame;
		}

		IEnumerator RestoreActiveObject()
		{
			if (objectsToIgnore == null)
				objectsToIgnore = new List<GameObject>(); //activeDict = new Dictionary<GameObject, bool>();
			int objectsPerFrame = GetWaitAfterNObjects();

			for (int i = 0; i < transform.childCount; i++)
			{
				var thisChild = transform.GetChild(i).gameObject;
				if (DisableObjectCondition(thisChild))
				{
					if (ignoreSavedKeepDisabledList || !objectsToIgnore.Contains(thisChild)) thisChild.SetActive(true);
					if (Application.isPlaying && i > 0 && i % objectsPerFrame == 0)
					{
						yield return null;
					}
					else
					{
						// Debug.Log("going forward ");
					}
				}
			}

			yield break;
		}
	}
}
