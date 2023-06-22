namespace Z.LayoutPanel
{
	using UnityEngine;

	public abstract class LayoutCreatorBase : MonoBehaviourWithBg
	{
		protected virtual void Reset()
		{
			var layoutSteup = GetComponentInParent<LayoutSetupProvider>();
			if (layoutSteup == null)
			{
				var parent = transform;
				while (parent.parent != null) parent = parent.parent;
				parent.gameObject.AddComponent<LayoutSetupProvider>();
			}

			// var bc = GameObject.FindObjectOfType<LayoutSetupProvider>();
			// if (bc == null)
			// {
			// 	var where = transform;
			// 	while (where.parent != null) where = where.parent;
			// 	where.gameObject.AddComponent<LayoutSetupProvider>();
			// 	Debug.Log("Added LayoutBorderControl");
			// }
		}
	}
}
