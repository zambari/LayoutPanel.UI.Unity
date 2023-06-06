namespace Z.LayoutPanel
{
	using System.Threading.Tasks;

	using UnityEngine;
	using UnityEngine.UI;

	public class SimpleFoldHelper : FoldControlBase
	{
		private LayoutPanel panel;

		protected override void FindRoot()
		{
			if (panel == null) panel = GetComponentInParent<LayoutPanel>();

			if (foldRoot != null) return;

			if (panel != null)
			{
				foldRoot = panel.transform;
			}
			else
			{
				foldRoot = transform.parent.parent;
			}
		}

		public override async Task SetFoldAsync(bool newFold)
		{
			if (isLocked)
			{
				Debug.Log("is locked");
				return;
			}

			isLocked = true;
			if (foldRoot == null) FindRoot();
			_isFolded = newFold;

			SetFoldLabel();
			int stepDelay = foldTimeMs / foldRoot.childCount;
			for (int i = 1; i < foldRoot.childCount; i++)
			{
				foldRoot.GetChild(i).gameObject.SetActive(!newFold);
				await Task.Delay(stepDelay);
			}

			isLocked = false;
			await Task.Yield();

			LayoutRebuilder.MarkLayoutForRebuild(panel.rectTransform);

			// LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
		}
	}
}
