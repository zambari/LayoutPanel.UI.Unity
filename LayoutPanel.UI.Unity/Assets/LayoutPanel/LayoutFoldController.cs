using System.Collections;
using System.Collections.Generic;
using LayoutPanelDependencies;
using UnityEngine;
using UnityEngine.UI;
using zUI;
//  [RequireComponent(typeof(VerticalLayoutGroup))]
//   [RequireComponent(typeof(LayoutElement))]
public class LayoutFoldController : MonoBehaviour
{
    public DrawInspectorBg draw;
    public Button foldButton;
    [SerializeField] [HideInInspector] float savedPreferredHeight = -1;
    [SerializeField] [HideInInspector] float savedFlexibleHeight = -1;
    [SerializeField]
    List<GameObject> objectsToKeepDisabled = new List<GameObject>();
    public bool isFolded { get { return _isFolded; } protected set { _isFolded = value; } }
    bool _isFolded;
    LayoutElement layoutElement;
    RectTransform _rect;
    public bool isLeftSide;
    public bool foldChildren;

    public static string labelUnfolded { get { return "▼"; } }
    public static string labelFolded { get { return "◀"; } }
    public static string labelFoldedAlt { get { return "▶"; } }
    RectTransform rect { get { if (_rect == null) _rect = GetComponent<RectTransform>(); return _rect; } }
    bool isAnimating;
    public System.Action<bool> onFold;
    public bool ignoreSavedKeepDisabledList = true;

    [SerializeField] Text _foldLabelText;

    public Text foldLabelText
    {
        get { return _foldLabelText; }
        set
        {
            _foldLabelText = value;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () => _foldLabelText.SetText(GetFoldString());
#else
            _foldLabelText.SetText(GetFoldString());
#endif
        }
    }
    [ExposeMethodInEditor]
    public void ToggleFold()
    {
        if (foldChildren || Input.GetKey(KeyCode.LeftAlt))
        {
            var otherTops = transform.parent.GetComponentsInChildren<LayoutFoldController>();
            for (int i = 0; i < otherTops.Length; i++)
                otherTops[i].Fold(!isFolded);
        }
        else
            Fold(!isFolded);
    }

    public string GetFoldString()
    {
        if (isFolded)
            return isLeftSide ? labelFoldedAlt : labelFolded; //▲ ▶ ◀ ▼
        else
            return labelUnfolded;
    }
    void OnValidate()
    {
        if (foldLabelText != null) foldLabelText.text = GetFoldString();
        UpdateSize();
    }
    void UpdateSize()
    {
        if (layoutElement == null)
            layoutElement = GetComponent<LayoutElement>();
        if (layoutElement != null)
        {
            layoutElement.flexibleHeight = -1;
            layoutElement.minHeight = LayoutSettings.topHeight;
        }
    }
    void OnEnable()
    {
        LayoutSettings.onBorderSizeChange += UpdateSize;
    }
    void OnDisable()
    {
        LayoutSettings.onBorderSizeChange -= UpdateSize;
    }
    void Start()
    {
        if (foldButton != null) foldButton.onClick.AddListener(ToggleFold);
        layoutElement = GetComponent<LayoutElement>();
    }

    public void Fold(bool newFold)
    {

        /*  if (!isActiveAndEnabled)
           {
               Debug.Log("inactive");
               return;
           } */
        if (newFold && isFolded) return;
        if (!newFold && !isFolded) return;
        if (!gameObject.activeInHierarchy) return;
        if (!isFolded)
            StartCoroutine(Fold());
        else
            StartCoroutine(UnFold());
        foldLabelText.SetText(GetFoldString());
        // if (isFolded)
        //    foldLabelText.SetText(isLeftSide ? labelFoldedAlt : labelFolded); //▲ ▶ ◀ ▼
        // else
        //    foldLabelText.SetText(labelUnfolded);
    }

    bool CanStartCoroutine()
    {
        if (!isActiveAndEnabled) return false;
        return (Application.isPlaying);
    }

    bool DisableObjectCondition(GameObject g)
    {
        return g.GetComponent<LayoutTopControl>() == null && g.GetComponent<LayoutBorderDragger>() == null && !g.name.Contains(LayoutBorderDragger.baseName);
    }
    IEnumerator StoreActiveObjects()
    {
        objectsToKeepDisabled = new List<GameObject>();
        int objectsPerFrame = GetWaitAfterNObjects();
        for (int i = 0; i < transform.childCount; i++)
        {
            var thisChild = transform.GetChild(i).gameObject;
            if (DisableObjectCondition(thisChild))
            {
                if (!thisChild.activeSelf)
                    objectsToKeepDisabled.Add(thisChild);

                thisChild.SetActive(false);
                if (Application.isPlaying && i > 0 && i % objectsPerFrame == 0)
                {
                    yield return null;
                }
            }
        }
        yield break;
    }
    int GetWaitAfterNObjects()
    {
        int objectsPerFrame = transform.childCount / 5;
        if (objectsPerFrame < 1)
            objectsPerFrame = 1;
        return objectsPerFrame;
    }

    IEnumerator RestoreActiveObject()
    {
        if (objectsToKeepDisabled == null) objectsToKeepDisabled = new List<GameObject>(); //activeDict = new Dictionary<GameObject, bool>();
        int objectsPerFrame = GetWaitAfterNObjects();

        for (int i = 0; i < transform.childCount; i++)
        {

            var thisChild = transform.GetChild(i).gameObject;
            if (DisableObjectCondition(thisChild))
            {
                if (ignoreSavedKeepDisabledList || !objectsToKeepDisabled.Contains(thisChild))

                    thisChild.SetActive(true);
                if (Application.isPlaying && i > 0 && i % objectsPerFrame == 0)
                {
                    yield return null;
                }
                else
                {
                    Debug.Log("going forward ");

                }
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
