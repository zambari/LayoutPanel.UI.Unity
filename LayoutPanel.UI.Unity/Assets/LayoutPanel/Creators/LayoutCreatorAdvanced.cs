using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

// using zUI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Z.LayoutPanel
{
	[DisallowMultipleComponent]
	public class LayoutCreatorAdvanced : LayoutCreatorBase
	{
		public Color startColor = new Color(0, 0.408f, 0.945f, 0.559f);

		public Color endColor = new Color(0.000f, 0.851f, 0.302f, 0.578f);

		[HideInInspector]
		public Color textColor = Color.white * 0.8f;

		[HideInInspector]
		public Color topColor = Color.white * 0.4f;

		public bool startVertical;

		public int startWithsteps = 3;

		[Header("Visible borders")]
		public bool hideCreatedBordersInHierarchy = true;

		public bool bordersPlacedInside = false;

		// float flexHeight = 0.2f;
		// [Header("something,something\n\nehruie")]
		// public bool tagObjectNamesWithDepth;

		LayoutNameHelper nameHelper;

		int contName;

		private List<Image> createdImages;

		public bool onlyCreateBorders = false;

		public bool checkForChildren = true;

		private void OnValidate()
		{
			if (startWithsteps < 1) startWithsteps = 1;
			if (startWithsteps > 4) startWithsteps = 4;

			// if (lastTag != tagObjectNamesWithDepth)
			// {
			//     lastTag = tagObjectNamesWithDepth;
			//     var itemcreators = GetComponentsInChildren<LayoutNameHelper>();
			//     foreach (var i in itemcreators)
			//         i.NameTagHandling(tagObjectNamesWithDepth);
			// }
		}

		private void AddPanelBorders(Transform src, bool fullPanel)
		{
			var creator = src.gameObject.AddComponent<LayoutItemCreator>();
			if (fullPanel) creator.ConvertToLayoutPanel();
			else creator.AddBorders();
			if (creator != null) DestroyImmediate(creator);
		}

		public void CreateSetupNewExperimentalPart1()
		{
			if (GetComponentInParent<Canvas>() == null)
			{
				Debug.Log("Please add to a panel");
				return;
			}

			bool currentDirHorizontal = !startVertical;
			int thisStepCount = startWithsteps;
			RectTransform thisRect = Prepare();

			Vector2 prefferedSize = new Vector2(thisRect.rect.width, thisRect.rect.height);
			Debug.Log("current preffered " + prefferedSize);

			// AddGroupComponents(thisRect, currentDirHorizontal);

			//    prefferedSize /= thisStepCount;
			var thisLevel = new List<Transform>();
			thisLevel.Add(thisRect.transform);
			int leveel = 0;
			while (thisStepCount >= 0 && leveel < 4)
			{
				leveel++;
				prefferedSize /= thisStepCount;
				Debug.Log(
					"level is " + leveel + " current preffered " + prefferedSize + " created =" + createdImages.Count);
				currentDirHorizontal = !currentDirHorizontal;
				thisLevel = HandleStep(thisLevel, currentDirHorizontal, thisStepCount, prefferedSize);
				thisStepCount--;
			}

			for (int i = 0; i < createdImages.Count; i++)
			{
				createdImages[i].color = Color.Lerp(startColor, endColor, 1f * i / createdImages.Count);
				LayoutItemCreator creator = createdImages[i].AddOrGetComponent<LayoutItemCreator>();
			}
		}

		public void CreateSetupNewExperimentalPart2()
		{
			Debug.Log("we have " + createdImages.Count + "images ");
			for (int i = 0; i < createdImages.Count; i++) { }
		}

		private List<Transform> HandleStep(
			List<Transform> transforms,
			bool stephorizontal,
			int thisStepCount,
			Vector2 prefferedSize)
		{
			List<Transform> list = new List<Transform>();
			if (stephorizontal) prefferedSize.x = -1;
			else prefferedSize.y = -1;
			for (int i = 0; i < transforms.Count; i++)
			{
				transforms[i].AddGroupComponents(stephorizontal);
				var panels = FillWithPanels(transforms[i], thisStepCount, prefferedSize);
				list.AddRange(panels);
			}

			return list;
		}

		List<Transform> FillWithPanels(Transform src, int count, Vector2 preferredSize)
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < count; i++)
			{
				GameObject newChild = new GameObject("item " + contName, typeof(Image));
				createdImages.Add(newChild.GetComponent<Image>());
				newChild.AddLayoutElement(preferredSize);
				newChild.transform.SetParent(src);
				list.Add(newChild.transform);
				contName++;
			}

			return list;
		}

		private RectTransform Prepare()
		{
			createdImages = new List<Image>();
			contName = 1;
			var image = GetComponent<Image>();
			if (image != null) image.enabled = false;

			var hg = GetComponent<HorizontalLayoutGroup>();
			var vg = GetComponent<VerticalLayoutGroup>();
			if (hg != null) DestroyImmediate(hg);
			if (vg != null) DestroyImmediate(vg);

			var thisRect = GetComponent<RectTransform>().AddImageChild(0.8f).GetComponent<RectTransform>();
			thisRect.anchorMax = Vector2.one;
			thisRect.anchorMin = Vector2.zero;

			while (thisRect.rect.width < 100) thisRect.sizeDelta = thisRect.sizeDelta + new Vector2(10, 0);
			while (thisRect.rect.height < 100) thisRect.sizeDelta = thisRect.sizeDelta + new Vector2(0, 10);
			return thisRect;
		}

		private void ChangeToObject(GameObject obj)
		{
#if UNITY_EDITOR
			Debug.Log("chaned");
			DestroyImmediate(this);
			Selection.activeGameObject = obj;
			obj.AddComponent<LayoutCreator>();
#endif
		}

		protected override void Reset()
		{
			base.Reset();
#if UNITY_EDITOR
			Canvas canvas = GetComponentInParent<Canvas>();
			if (canvas == null)
			{
				Debug.Log(
					"Please add me to a panel to your canvas (start with UI image/panel), currently no canvas is found in parent");
				DestroyImmediate(this);
				return;
			}

			if (canvas.transform == transform)
			{
				var newObj = canvas.GetComponent<RectTransform>().AddImageChild();
				EditorApplication.delayCall += () => ChangeToObject(newObj.gameObject);
				return;
			}

			Image image = GetComponent<Image>();
			if (image.color == Color.white)
			{
				Undo.RegisterCompleteObjectUndo(image, "color");
				image.color = (Color.black + Color.white * 0.2f).Randomize();
			}

			RectTransform rect = GetComponent<RectTransform>();
			if (rect.rect.width == 100 && rect.rect.height == 100) rect.sizeDelta = new Vector2(300, 400);
#endif
		}

		private void UpdateBorders()
		{
			var borders = gameObject.GetComponentsInChildren<LayoutBorderDragger>();
			foreach (var b in borders) b.side = b.side;
		}

		private void AddRandomChildren(GameObject parent, int count, ref List<LayoutPanel> panels)
		{
			for (int j = 0; j < count; j++)
			{
				var panel = parent.CreatePanel("Item [" + (panels.Count + 1) + " ] ");

				//                panel.detachedMode = false;
				panels.Add(panel);
			}
		}

		public static LayoutElement CreateSpacer(GameObject thisChild, float flexWidth = 0.2f, float flexHeight = 0.2f)
		{
			var img = thisChild.AddChildRectTransform();
			var le = img.gameObject.AddComponent<LayoutElement>();
			le.flexibleHeight = flexHeight;
			le.flexibleWidth = flexWidth;
			le.name = LayoutPanel.spacerName;
			return le;
		}

		private void UpdateNames()
		{
			var nameH = GetComponentsInChildren<LayoutNameHelper>();
			foreach (var h in nameH) h.UpdateName();
		}

		[LPExposeMethodInEditor]
		private void BackToCreator()
		{
			if (gameObject.GetComponent<LayoutCreator>() == null) gameObject.AddComponent<LayoutCreator>();
			GameObject.DestroyImmediate(this);
		}

		[LPExposeMethodInEditor]
		public void CreateHorizontalLayoutSlpit()
		{
			gameObject.CreateHorizontalLayoutCreators(3);
		}

		[LPExposeMethodInEditor]
		public void CreateVerticalLayoutSlpit()
		{
			gameObject.CreateVerticalLayoutCreators(3);
		}

		[LPExposeMethodInEditor]
		private void RemoveAllCreators()
		{
#if UNITY_EDITOR
			var cr = GetComponentsInChildren<LayoutItemCreator>();
			foreach (var c in cr) Undo.DestroyObjectImmediate(c.gameObject);
			EditorApplication.delayCall += () => Undo.DestroyObjectImmediate(this);
#endif
		}

		[LPExposeMethodInEditor]
		public void RemoveAllChildren()
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
#if UNITY_EDITOR
				Undo.DestroyObjectImmediate(transform.GetChild(i).gameObject);
#else
            DestroyImmediate(transform.GetChild(i).gameObject);
#endif
		}

		[LPExposeMethodInEditor]
		public void LaunchItemCreators()
		{
			gameObject.LaunchItemCreators(checkForChildren, onlyCreateBorders);
		}
#if UNITY_EDITOR
		[LPExposeMethodInEditor]
		public void SelectAllCreators()
		{
			List<GameObject> games = new List<GameObject>();
			var itemcr = GetComponentsInChildren<LayoutItemCreator>();
			foreach (var i in itemcr) games.Add(i.gameObject);
			Selection.objects = itemcr;
		}
#endif
	}
}
