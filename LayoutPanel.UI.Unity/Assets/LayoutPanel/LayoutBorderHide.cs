using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
#if UNITY_EDITOR

using UnityEditor;
#endif
using System.Linq;
namespace Z.LayoutPanel
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class LayoutBorderHide : MonoBehaviour
    {

        public enum BorderHideMode { Ignore, Visible, Hidden, TopOnly, HideAll, Clear }

        [ClickableEnum]
        [SerializeField]
        BorderHideMode _borderHideMode;
        [SerializeField]
        [HideInInspector]
        BorderHideMode _lastHideMode;
        // public bool applyToAllChildren;
        // [SerializeField] bool _editColors;
        [SerializeField] bool _alsoHideSCrollBars=true;
        // public bool editColors
        // {
        //     get { return _editColors; }
        //     set { _editColors = value; OnValidate(); }
        // }
        // public Color borderColor;

        int hiddenCount = 0;

      
        public BorderHideMode borderHideMode
        {
            get { return _borderHideMode; }
            set
            {

                _borderHideMode = value;

                hiddenCount = 0;
                hiddenCount = SetBasedOnGetComponents();
                RepaintHierarchy();
            }
        }

        [Header("In case of invisible objects")]
        public bool unhideAllObjectsInScene;
        int SetRecursive(Transform transform, HideFlags hideFlags, int startValue = 0)
        {

            for (int i = 0; i < transform.childCount; i++)
                startValue += SetRecursive(transform.GetChild(i), hideFlags);
            transform.hideFlags = hideFlags;
            return startValue + 1;
        }

        int SetBasedOnGetComponents()
        {
            var allchildren = transform.GetAllChildren();
            var alllBorders = this.GetComponentsInChildrenIncludingInactive<LayoutBorderDragger>(allchildren);
            var allScrols = this.GetComponentsInChildrenIncludingInactive<Scrollbar>(allchildren);
            var allTops = this.GetComponentsInChildrenIncludingInactive<LayoutTopControl>(allchildren);
            // Debug.Log($"found {alllBorders.Length}");
            //            Debug.Log("warning, setting visibility based on getcomponentinchildren: this only works on active objects", gameObject);
            var panels = gameObject.GetComponentsInChildren<LayoutPanel>();
            List<LayoutBorderDragger> borders = new List<LayoutBorderDragger>();
            List<Scrollbar> scrollbars = new List<Scrollbar>();
            List<LayoutTopControl> tops = new List<LayoutTopControl>(); // = gameObject.GetComponentsInChildren<LayoutTopControl>();
            scrollbars.AddRange(GetComponentsInChildren<Scrollbar>());
            foreach (var p in panels)
            {
                for (int i = 0; i < p.transform.childCount; i++)
                {
                    var thischild = p.transform.GetChild(i);
                    var thisborder = thischild.GetComponent<LayoutBorderDragger>();
                    if (thisborder != null) borders.Add(thisborder);
                    var thistop = thischild.GetComponent<LayoutTopControl>();
                    if (thistop != null) tops.Add(thistop);
                    //var scroll = thischild.GetComponent<Scrollbar>();

                }
            }

            //var bs = gameObject.GetComponentsInChildren<LayoutBorderDragger>();
            //var tops = gameObject.GetComponentsInChildren<LayoutTopControl>();
            int hiddenCount = 0;
            // unhi
            foreach (var s in scrollbars)
                s.gameObject.hideFlags = (_alsoHideSCrollBars && _borderHideMode == BorderHideMode.Hidden) ? HideFlags.HideInHierarchy : HideFlags.None;
            switch (borderHideMode)
            {
                case BorderHideMode.Ignore:

                case BorderHideMode.Visible:
                    foreach (var t in tops)
                        t.gameObject.hideFlags = HideFlags.None;
                    foreach (var b in borders)
                        b.gameObject.hideFlags = HideFlags.None;
                    break;
                case BorderHideMode.HideAll:
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        var thischild = transform.GetChild(i);
                        if (thischild.hideFlags != HideFlags.HideInHierarchy)
                            thischild.hideFlags = HideFlags.HideInHierarchy;
                    }

                    break;
                case BorderHideMode.Clear:
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        var thischild = transform.GetChild(i);
                        if (thischild.hideFlags != HideFlags.None)
                            thischild.hideFlags = HideFlags.None;
                    }

                    break;
                case BorderHideMode.Hidden:
                    foreach (var t in tops)
                    {

                        t.gameObject.hideFlags = HideFlags.HideInHierarchy;
                        hiddenCount++;
                    }
                    foreach (var b in borders)
                    {
                        b.gameObject.hideFlags = HideFlags.HideInHierarchy;
                        hiddenCount++;
                    }
                    break;
                case BorderHideMode.TopOnly:
                    foreach (var t in tops)
                    {
                        hiddenCount++;
                        t.gameObject.hideFlags = HideFlags.HideInHierarchy;
                    }
                    foreach (var b in borders)
                    {
                        b.gameObject.hideFlags = HideFlags.HideInHierarchy;
                        hiddenCount++;
                    }
                    break;
            }

            return hiddenCount;
        }

        // int SetBasedOnNewExtensions()
        // {
        //     var allchildren = transform.GetAllChildren();
        //     var alllBorders = this.GetComponentsInChildrenIncludingInactive<LayoutBorderDragger>(allchildren);
        //     var allScrols = this.GetComponentsInChildrenIncludingInactive<Scrollbar>(allchildren);
        //     var allTops = this.GetComponentsInChildrenIncludingInactive<LayoutTopControl>(allchildren);

        //     int hiddenCount = 0;
        //     if (_borderHideMode == BorderHideMode.Hidden)
        //         foreach (var t in allTops)
        //         {
        //             hiddenCount++;
        //             t.gameObject.hideFlags = HideFlags.HideInHierarchy;
        //         }
        //     if (_borderHideMode == BorderHideMode.Hidden || _borderHideMode == BorderHideMode.TopOnly)
        //     {
        //         foreach (var b in alllBorders)
        //         {
        //             b.gameObject.hideFlags = HideFlags.HideInHierarchy;
        //             hiddenCount++;
        //         }
        //     }
        //     else
        //     {
        //         foreach (var t in allTops)
        //             t.gameObject.hideFlags = HideFlags.None;
        //         foreach (var b in alllBorders)
        //             b.gameObject.hideFlags = HideFlags.None;
        //     }
        //     foreach (var s in allScrols)
        //         s.gameObject.hideFlags = (_alsoHideSCrollBars && _borderHideMode == BorderHideMode.Hidden) ? HideFlags.HideInHierarchy : HideFlags.None;
        //     return hiddenCount;
        // }

        // int SetBasedOnRoot(Transform source)
        // {
        //     int hiddenCount = 0;
        //     for (int i = 0; i < source.childCount; i++)
        //     {
        //         var thisGame = source.GetChild(i).gameObject;
        //         if (thisGame.GetComponent<LayoutBorderDragger>() != null)
        //         {
        //             if (_borderHideMode == BorderHideMode.Visible)
        //                 thisGame.hideFlags = HideFlags.None;
        //             else
        //             {
        //                 hiddenCount++;
        //                 thisGame.hideFlags = HideFlags.HideInHierarchy;
        //             }
        //         }
        //         else
        //         {
        //             if (thisGame.GetComponent<LayoutTopControl>())
        //                 if (_borderHideMode == BorderHideMode.Hidden)
        //                 {
        //                     hiddenCount++;
        //                     thisGame.hideFlags = HideFlags.HideInHierarchy;
        //                 }
        //             else
        //             {
        //                 thisGame.hideFlags = HideFlags.None;
        //             }

        //         }
        //     }
        //     return hiddenCount;
        // }
        void SelectionRepaint()
        {
            // BRUTEFORCE BEGIN
#if UNITY_EDITOR
            var sel = Selection.activeGameObject;
            if (sel == gameObject)
            {
                if (transform.parent != null)
                {
                    Selection.activeGameObject = transform.parent.gameObject;
                    EditorApplication.delayCall += () => Selection.activeGameObject = sel;
                }
            }
            // BRUTEFORCE END
#endif
        }
        public static void RepaintHierarchy()
        {
#if UNITY_EDITOR
            try
            {
                EditorApplication.RepaintHierarchyWindow();
                EditorApplication.DirtyHierarchyWindowSorting();
            }
            catch {}
#endif
        }

#if UNITY_EDITOR
        void Reset()
        {
            // for (int i = 0; i < 41; i++)                 
            // UnityEditorInternal.ComponentUtility.MoveComponentUp(this); //shit do not do this, messes up nested prefabs
            borderHideMode = BorderHideMode.Hidden;

            var b = GetComponentInChildren<LayoutBorderDragger>();
            // if (b != null) borderColor = b.GetComponent<Image>().color;
            // else
            //     borderColor = Color.gray * 0.6f + (new Color(Random.value, Random.value, Random.value)) * 0.3f;

        }
#endif
        void UnhideAll()
        {
            unhideAllObjectsInScene = false;
#if UNITY_EDITOR
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty)
            {
                Debug.Log("please save your scene first, this can be danerous");
                return;
            }

            var all = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            int modified = 0;
            //    Undo.RegisterUndo("hideflags");
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].hideFlags == HideFlags.HideInHierarchy)
                {
                    Debug.Log("found flag " + all[i].hideFlags, all[i]);
                    modified++;
                    //  Undo.RegisterUndo(all[i], "hideflags");
                    all[i].hideFlags = HideFlags.None;
                }
            }
            Debug.Log("Modified " + modified + " objects out of total of " + all.Length);
#endif
        }
        void OnEnable()
        {
            _lastHideMode=BorderHideMode.Ignore;//
            OnValidate();
        }
        void OnValidate()
        {
            var list = transform.GetAllChildren();
            if (_borderHideMode == _lastHideMode)
                 return;
            borderHideMode = _borderHideMode;
            _lastHideMode = borderHideMode;

            if (unhideAllObjectsInScene) UnhideAll();
            // borderHideMode = BorderHideMode.Visible;
            // if (editColors)
            // {
            //     if (applyToAllChildren)
            //     {
            //         var bs = gameObject.GetComponentsInChildren<LayoutBorderDragger>();
            //         foreach (var b in bs)
            //             b.GetComponent<Image>().color = borderColor;
            //     }
            //     else
            //     {
            //         var bs = gameObject.GetComponentsInDirectChildren<LayoutBorderDragger>();
            //         foreach (var b in bs)
            //             b.GetComponent<Image>().color = borderColor;
            //     }
            // }
            // SetObjectName();
        }
        void OnDestroy()
        {
            borderHideMode = BorderHideMode.Visible;
        }

        #region concept_that_wasnt_so_great

#if NOT
        [ExposeMethodInEditor]
        void SelectBorders()
        {
            borderHideMode = BorderHideMode.Visible;
            var borders = this.GetComponentsInDirectChildren<LayoutBorderDragger>();
            UnityEditor.Selection.objects = borders.ToList().Select(x => x.gameObject) as GameObject[];
        }
        void OnEnable()
        {
            Selection.selectionChanged -= OnSelectionChange;
            Selection.selectionChanged += OnSelectionChange;

        }
        void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChange;

        }

        static void OnSelectionChange()
        {
            if (Selection.activeGameObject != null) // && Selection.activeGameObject.hideFlags == HideFlags.HideInHierarchy)
            {
                var borderHide = Selection.activeGameObject.GetComponentInParent<LayoutBorderHide>();
                var border = Selection.activeGameObject.GetComponent<LayoutBorderDragger>();
                var top = Selection.activeGameObject.GetComponent<LayoutTopControl>();
                if (border != null)
                {
                    borderHide.gameObject.PingInEditor("Selected vorder is hidden, try unhiding first");
                }
                if (top != null)
                {
                    borderHide.gameObject.PingInEditor("Selected header is hidden, try unhiding first");
                }
            }
        }

        [ExposeMethodInEditor]
        void SelectFrames()
        {
            var draggers = transform.parent.GetComponentsInChildren<LayoutBorderDragger>();
            UnityEditor.Selection.objects = draggers.ToList().Select(x => { return x.gameObject; }) as GameObject[];

        }
#endif
        #endregion
    }
}