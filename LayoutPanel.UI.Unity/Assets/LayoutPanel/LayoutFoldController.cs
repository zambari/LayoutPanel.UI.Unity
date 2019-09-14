using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{
    [RequireComponent(typeof(VerticalLayoutGroup))]
    //   [RequireComponent(typeof(LayoutElement))]
    public class LayoutFoldController : MonoBehaviour
    {
        public DrawInspectorBg draw;
        public Button foldButton;
        [SerializeField] [HideInInspector] float savedPreferredHeight = -1;
        [SerializeField] [HideInInspector] float savedFlexibleHeight = -1;
        [SerializeField]
        List<GameObject> objectsToKeepDisabled = new List<GameObject>();
        [ReadOnly] public bool isFolded;
        LayoutElement layoutElement;
        RectTransform _rect;
        public static string labelUnfolded { get { return "▼"; } }
        public static string labelFolded { get { return "◀"; } }
        RectTransform rect { get { if (_rect == null) _rect = GetComponent<RectTransform>(); return _rect; } }
        bool isAnimating;
        public System.Action<bool> onFold;

        [ExposeMethodInEditor]
        public void ToggleFold()
        {
            if (!Input.GetKey(KeyCode.LeftAlt))
                Fold(!isFolded);
            else
            {
                var otherTops = transform.parent.GetComponentsInChildren<LayoutFoldController>();
                for (int i = 0; i < otherTops.Length; i++)
                    otherTops[i].Fold(!isFolded);
            }
        }

        void OnValidate()
        {

            if (layoutElement == null)
                layoutElement = GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                layoutElement.flexibleHeight = -1;
                layoutElement.minHeight = LayoutTopControl.topHeight;
            }
            gameObject.AddOrGetComponent<VerticalLayoutGroup>().SetChildControl();
        }

        void Start()
        {
            if (foldButton != null) foldButton.onClick.AddListener(ToggleFold);
            layoutElement = GetComponent<LayoutElement>();
        }

        public void Fold(bool newFold)
        {

            if (!isActiveAndEnabled)
            {
                Debug.Log("inactive");
                return;
            }
            if (!isFolded)
                StartCoroutine(Fold());
            else
                StartCoroutine(UnFold());
            if (isFolded)
                foldButton.GetComponentInChildren<Text>().SetText(labelFolded); //▲ ▶ ◀ ▼
            else
                foldButton.GetComponentInChildren<Text>().SetText(labelUnfolded);
        }
        bool CanStartCoroutine()
        {
            if (!isActiveAndEnabled) return false;
            return (Application.isPlaying);
        }

        bool DisableObjectCondition(GameObject g)
        {
            return g.GetComponent<LayoutTopControl>() == null && g.GetComponent<LayoutBorderDragger>() == null;
        }
        IEnumerator StoreActiveObjects()
        {
            objectsToKeepDisabled = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var thisChild = transform.GetChild(i).gameObject;
                if (DisableObjectCondition(thisChild))
                {
                    if (!thisChild.activeSelf)
                        objectsToKeepDisabled.Add(thisChild);

                    thisChild.SetActive(false);
                    if (Application.isPlaying) yield return null;
                }
            }
            yield break;
        }

        IEnumerator RestoreActiveObject()
        {
            if (objectsToKeepDisabled == null) yield break; //activeDict = new Dictionary<GameObject, bool>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var thisChild = transform.GetChild(i).gameObject;
                if (DisableObjectCondition(thisChild))
                {
                    if (!objectsToKeepDisabled.Contains(thisChild)) thisChild.SetActive(true);
                    if (Application.isPlaying) yield return null;
                }
            }
            yield break;
        }


        IEnumerator Fold()
        {
            if (isAnimating || isFolded) yield break;
            isAnimating = true;
            if (layoutElement != null)
            {
                savedPreferredHeight = layoutElement.preferredHeight;
                savedFlexibleHeight = layoutElement.flexibleHeight;
            }


            StartCoroutine(StoreActiveObjects());

            if (layoutElement != null)
            {
                layoutElement.preferredHeight = -1;
                layoutElement.flexibleHeight = -1;
            }
            isFolded = true;
            if (onFold != null) onFold(true);
            isAnimating = false;
        }

        IEnumerator UnFold()
        {
            if (isAnimating || !isFolded) yield break;
            isAnimating = true;
            StartCoroutine(RestoreActiveObject());
            if (layoutElement != null)
            {
                layoutElement.preferredHeight = savedPreferredHeight;
                layoutElement.flexibleHeight = savedFlexibleHeight;
            }
            isAnimating = false;
            if (onFold != null) onFold(false);
            isFolded = false;
        }
    }

}