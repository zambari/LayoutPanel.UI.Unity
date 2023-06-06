using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using Z.LayoutPanel;

public class SimpleFoldController : FoldControlSomething
{
	protected LayoutElement layoutElement
	{
		get
		{
			if (_layoutElement == null) _layoutElement = GetComponent<LayoutElement>();
			return _layoutElement;
		}
	}

	private LayoutElement _layoutElement;

	protected bool DisableObjectCondition(GameObject g)
	{
		return g.GetComponent<LayoutTopControl>() == null &&
			   g.GetComponent<LayoutBorderDragger>() == null &&
			   !g.name.Contains(LayoutBorderDragger.baseName);
	}

	// public override void SetFold(bool newFold)
	// {
	// 	//        Debug.Log("fold " + newFold);
	// 	/*  if (!isActiveAndEnabled)
	// 	   {
	// 		   Debug.Log("inactive");
	// 		   return;
	// 	   } */
	// 	if (!gameObject.activeInHierarchy)
	// 	{
	// 		NaiveFold(newFold);
	// 		SetFoldLabel();
	//
	// 		// Debug.Log("folding inactive");
	// 		return;
	// 	}
	//
	// 	//     if (newFold == isFolded) { Debug.Log("has this satae"); return; }
	// 	// if (!newFold && !isFolded) { Debug.Log("has this satae"); return; }
	//
	// 	if (CanStartCoroutine()) StartCoroutine(FoldRoutine(newFold));
	// 	else Debug.Log("cannot start", gameObject);
	//
	// 	// if (isFolded)
	// 	//    foldLabelText.SetText(isLeftSide ? labelFoldedAlt : labelFolded); //▲ ▶ ◀ ▼
	// 	// else
	// 	//    foldLabelText.SetText(labelUnfolded);
	// }

	public override async Task SetFoldAsync(bool shouldHide)
	{
		if (isLocked)
		{
			Debug.Log("animating already " + name, gameObject);
			await Task.CompletedTask;
		}
		isLocked = true;
		GetObjectsToToggle();
		int perFrame = objectsToToggle.Count / 4;
		if (perFrame < 1) perFrame = 1;
		int currentobj = 0;
		int donePerFrame = 0;
		while (currentobj < objectsToToggle.Count)
		{
			if (!shouldHide) objectsToToggle[currentobj].SetActive(true);
			else objectsToToggle[currentobj].SetActive(false);
			currentobj++;
			donePerFrame++;
			if (donePerFrame >= perFrame)
			{
				donePerFrame = 0;
				await Task.Yield();
			}
		}

		isLocked = false;
		// isFolded = shouldHide;
		SetFoldLabel();
	}
}
