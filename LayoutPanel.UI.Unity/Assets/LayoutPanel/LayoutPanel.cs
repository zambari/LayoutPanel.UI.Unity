
using UnityEngine;
using UnityEngine.UI;

namespace Z.LayoutPanel
{

	[SelectionBase]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(LayoutElement))]
	public class LayoutPanel : SubPanel
	{

		[SerializeField]
		public bool freeMoveMode;

		public static string spacerName
		{
			get { return "---"; }
		}

		
		public LayoutElement layoutElement
		{
			get
			{
				if (_layoutElement == null) _layoutElement = GetComponent<LayoutElement>();
				return _layoutElement;
			}
		}

		private LayoutElement _layoutElement;

		public bool detachedMode
		{
			get { return freeMoveMode; }
		}

		[HideInInspector]
		[SerializeField]
		public bool isBeingDragged;

	
		public bool isInHorizontalGroup;

		public bool isInVerticalGroup;

		private VerticalLayoutGroup verticalLayout;
		
		// public bool autoSetGroupParams = true;

		public void PlaceDropTarget(Transform target, int newSiblingIndex = -1)
		{
			if (target == null)
			{
				Debug.Log("no target");
				return;
			}

			if (LayoutDropTarget.currentTarget != null)
			{
				LayoutPanel targetPanel = LayoutDropTarget.dropTargetPanel;
				bool isHorizontalbar = LayoutDropTarget.currentTarget.isHorizontalBar;
				bool isPanelHorizontal = LayoutDropTarget.currentTarget.isPanelHorizontal;
				bool isTargetHoritontal = targetPanel.isInHorizontalGroup;
				if (isTargetHoritontal != isHorizontalbar)
				{
					var cloned = LayoutEditorUtilities.ClonePanel(targetPanel);
					if (isTargetHoritontal)
					{
						// verticalLayout = cloned.AddOrGetComponent<VerticalLayoutGroup>();
						//    if (autoSetGroupParams) verticalLayout.SetParams(this);
					}
					else
					{
						//   var group = cloned.AddOrGetComponent<HorizontalLayoutGroup>();
						//  if (autoSetGroupParams) group.SetParams(this);
					}

					target = cloned;
					targetPanel.transform.SetParent(target);

					//       var nh = cloned.gameObject.AddOrGetComponent<LayoutNameHelper>();
					//nh.UpdateNameF();
					Debug.Log("Layout SPLIT  target =" + target);
				}
				else
				{
					Debug.Log(
						"LayoutDropTarget.isHorizontal=" +
						isPanelHorizontal +
						" isHorizontalbar=" +
						isHorizontalbar +
						" isPanelHorizontal= " +
						isPanelHorizontal +
						" this panel ishoriz=" +
						isInHorizontalGroup);
				}
			}

			Transform oldParent = transform.parent;
			transform.SetParent(target);
			if (newSiblingIndex != -1) transform.SetSiblingIndex(newSiblingIndex);
			else transform.SetAsLastSibling();

			if (oldParent.childCount == 0)
			{
				Debug.Log("removed orphan panel");
				DestroyImmediate(oldParent.gameObject);
			}
		}

		// public void SetGroupSettings(LayoutGroupSettings settings)
		// {
		//     groupSettings = settings;
		//     ApplyGroupSettings();
		// }
		public void PlaceTemporary(Transform target, int newSiblingIndex = -1)
		{
			transform.SetParent(target);
			if (newSiblingIndex != -1) transform.SetSiblingIndex(newSiblingIndex);
			else transform.SetAsLastSibling();
		}

		LayoutGroupSettings groupSettings = new LayoutGroupSettings();

		// protected override  void OnEnable()
		//     {
		//         GetGroupSettings();
		//     }
		// void GetGroupSettings()
		// {
		//     var groupProvider = GetComponentInParent<IProvideLayoutGroupSettings>();
		//     if (groupProvider != null) groupSettings = groupProvider.GetGroupSettings();
		// }
		// public void ApplyGroupSettings()
		// {
		//     //            Debug.Log("aplygroupsetings");
		//     if (groupSettings == null)
		//     {
		//         GetGroupSettings();
		//     }
		//     if (groupSettings != null)
		//     {
		//         if (verticalLayout == null) verticalLayout = GetComponent<VerticalLayoutGroup>();
		//         if (verticalLayout != null) groupSettings.ApplyTo(verticalLayout,setup);

		//     }
		// }

		public void CheckGroups()
		{
			if (transform.parent == null) return;

			isInHorizontalGroup = transform.parent.GetComponent<HorizontalLayoutGroup>() != null;
			isInVerticalGroup = transform.parent.GetComponent<VerticalLayoutGroup>() != null;
			if (!isInHorizontalGroup && !isInVerticalGroup)
			{
				//   Debug.Log("Set detachedMode "+name);
				freeMoveMode = true;
			}
			else
			{
				//            Debug.Log("set nonfree");
			}
		}

		void Start()
		{
			// var rt = GetComponent<RectTransform>();
			// float w = rt.rect.width;
			// float h = rt.rect.height;
			// Debug.Log(" w=" + w + " h=" + h);

			OnValidate();
			CheckGroups();
		}

		protected override void OnValidate()
		{
			if (gameObject == null) return;

			base.OnValidate();
			freeMoveMode = false; //?

			//  this.MoveComponentToPosition(1);
			// if (layoutElement == null || useOwnLE)
			// {
			// 	layoutElement = gameObject.AddOrGetComponent<LayoutElement>();
			// 	var rt = GetComponent<RectTransform>();
			// 	float w = rt.rect.width;
			// 	float h = rt.rect.height;
			// 	if (w > 0 && h > 0)
			// 	{
			// 		if (isInHorizontalGroup)
			// 		{
			// 			layoutElement.flexibleHeight = .1f;
			// 			layoutElement.preferredWidth = w;
			// 		}
			//
			// 		if (isInVerticalGroup)
			// 		{
			// 			layoutElement.flexibleWidth = .1f;
			// 			layoutElement.preferredHeight = h;
			// 		}
			//
			// 		//                Debug.Log("ser preferred  w=" + w + " h=" + h);
			// 	}
			// }

			if (layoutElement != null)
			{
				//   if (resizableElement.minHeight <= 0) resizableElement.minHeight = LayoutSettings.topHeight;
				//  if (resizableElement.minWidth <= 0) resizableElement.minWidth = LayoutSettings.minWidth;
			}

			// var le = GetComponent<LayoutElement>();
			// if (le != null)
			// {
			//     if (le.preferredHeight == 0 || le.preferredHeight == 0)
			//     {

			//         if (w > 0) le.preferredWidth = w;
			//         if (h > 0) le.preferredHeight = h;
			//         Debug.Log("ser preferred  w=" + w + " h=" + h);
			//     }
			// }

			//      gameObject.AddOrGetComponent<PanelSaverHelper>();
			// if (string.IsNullOrEmpty(_label)) _label = name.RemoveAllTags();
			// label = _label;
			// if (layoutElement == null)
			// {
			// 	var les = this.GetComponentsInDirectChildren<LayoutElement>();
			// 	for (int i = 0; i < les.Length; i++)
			// 	{
			// 		if (!les[i].ignoreLayout && les[i].flexibleHeight > 0) layoutElement = les[i];
			// 	}
			// }
#if UNITY_EDITOR
			var creator = GetComponent<LayoutItemCreator>();

			if (creator != null) UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(creator); };
#endif

			//       if (nameHelper == null) nameHelper = gameObject.AddOrGetComponent<LayoutNameHelper>();
			//   nameHelper.UpdateName();
		}

		// void SetGroupPadding()
		// {
		//     var group = gameObject.AddOrGetComponent<VerticalLayoutGroup>();
		//     if (group != null)
		//     {
		//         var padding = group.padding;
		//         if (padding.top < LayoutSettings.topHeight)
		//         {
		//             padding.top = LayoutSettings.topHeight;
		//             group.padding = padding;
		//         }
		//     }

		// }

		public Transform GetTargetTransformForSide(Side side)
		{
			if (transform.parent == null) return transform;

			return transform.parent;
		}

		public LayoutElement GetTargetElementForSide(Side side)
		{
			if ((side.isHorizontal() && isInVerticalGroup) || (!side.isHorizontal() && isInHorizontalGroup))
			{
				// Debug.Log("returngn parent", gameObject);
				return transform.parent.GetComponent<LayoutElement>();
			}
			else
			{
				// Debug.Log("returning resizabel", resizableElement.gameObject);
				return layoutElement;
			}
		}
	}
}
