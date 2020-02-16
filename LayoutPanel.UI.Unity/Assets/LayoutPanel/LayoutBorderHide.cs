

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LayoutPanelDependencies;
using Z;
#if UNITY_EDITOR

using UnityEditor;
#endif
using System.Linq;
namespace zUI
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class LayoutBorderHide : MonoBehaviour
    {
        public DrawInspectorBg draw;
        public enum BorderHideMode { Visible, Hidden, TopOnly }
        [ClickableEnum]
        [SerializeField]
        BorderHideMode _borderHideMode;

        public bool applyToAllChildren;
        [SerializeField] bool _editColors;
        public bool editColors
        {
            get { return _editColors; }
            set { _editColors = value; OnValidate(); }
        }
        public Color borderColor;

        public BorderHideMode borderHideMode
        {
            get { return _borderHideMode; }
            set
            {
                _borderHideMode = value;
                int hiddenCount = 0;
                if (applyToAllChildren)
                {
                    hiddenCount = SetBasedOnGetComponents();
                }
                else
                    hiddenCount = SetBasedOnRoot(transform);
                if (hiddenCount == 0) name = name.RemoveTag();
                else
                { if (hiddenCount == 2)
                    {
                        name = name.SetTag(" 	┌─┐"); //  
                    } else
                    if (hiddenCount == 4)
                    {
                        name = name.SetTag(" 	┌━┐"); // ┌─┐ 
                    }
                    else
                       if (hiddenCount == 5)
                    {
                        name = name.SetTag(" 	╒═╕");
                    }
                    else
                        name = name.SetTag(" 	┌" + hiddenCount + "┐");
                }
                RepaintHierarchy();
                //SelectionRepaint()
            }
        }

        [Header("In case of invisible objects")]
        public bool unhideAllObjectsInScene;

        int SetBasedOnGetComponents()
        {
            Debug.Log("warning, setting visibility based on getcomponentinchildren: this only works on active objects", gameObject);
            var bs = gameObject.GetComponentsInChildren<LayoutBorderDragger>();
            var tops = gameObject.GetComponentsInChildren<LayoutTopControl>();
            int hiddenCount = 0;
            if (_borderHideMode == BorderHideMode.Hidden)
                foreach (var t in tops)
                {
                    hiddenCount++;
                    t.gameObject.hideFlags = HideFlags.HideInHierarchy;
                }
            if (_borderHideMode == BorderHideMode.Hidden || _borderHideMode == BorderHideMode.TopOnly)
            {
                foreach (var b in bs)
                {
                    b.gameObject.hideFlags = HideFlags.HideInHierarchy;
                    hiddenCount++;
                }
            }
            else
            {
                foreach (var t in tops)
                    t.gameObject.hideFlags = HideFlags.None;
                foreach (var b in bs)
                    b.gameObject.hideFlags = HideFlags.None;
            }
            return hiddenCount;
        }
        int SetBasedOnRoot(Transform source)
        {
            int hiddenCount = 0;
            for (int i = 0; i < source.childCount; i++)
            {
                var thisGame = source.GetChild(i).gameObject;
                if (thisGame.GetComponent<LayoutBorderDragger>() != null)
                {
                    if (_borderHideMode == BorderHideMode.Visible)
                        thisGame.hideFlags = HideFlags.None;
                    else
                    {
                        hiddenCount++;
                        thisGame.hideFlags = HideFlags.HideInHierarchy;
                    }
                }
                else
                {
                    if (thisGame.GetComponent<LayoutTopControl>())
                        if (_borderHideMode == BorderHideMode.Hidden)
                        {
                            hiddenCount++;
                            thisGame.hideFlags = HideFlags.HideInHierarchy;
                        }
                        else
                        {
                            thisGame.hideFlags = HideFlags.None;
                        }

                }
            }
            return hiddenCount;
        }
        void SelectionRepaint()
        {
            // BRUTEFORCE BEGIN

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
        }
        public static void RepaintHierarchy()
        {
#if UNITY_EDITOR
            try
            {
                EditorApplication.RepaintHierarchyWindow();
                EditorApplication.DirtyHierarchyWindowSorting();
            }
            catch { }
#endif
        }


#if UNITY_EDITOR
        void Reset()
        {
            for (int i = 0; i < 41; i++)
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
            borderHideMode = BorderHideMode.Hidden;

            var b = GetComponentInChildren<LayoutBorderDragger>();
            if (b != null) borderColor = b.GetComponent<Image>().color;
            else
                borderColor = Color.gray * 0.6f + (new Color(Random.value, Random.value, Random.value)) * 0.3f;

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
        void OnValidate()
        {
            borderHideMode = _borderHideMode;
            if (unhideAllObjectsInScene) UnhideAll();

            // borderHideMode = BorderHideMode.Visible;
            if (editColors)
            {
                if (applyToAllChildren)
                {
                    var bs = gameObject.GetComponentsInChildren<LayoutBorderDragger>();
                    foreach (var b in bs)
                        b.GetComponent<Image>().color = borderColor;
                }
                else
                {
                    var bs = gameObject.GetComponentsInDirectChildren<LayoutBorderDragger>();
                    foreach (var b in bs)
                        b.GetComponent<Image>().color = borderColor;
                }
            }
        }
        void OnDestroy()
        {
            borderHideMode = BorderHideMode.Visible;
        }


#if UNITY_EDITOR_not
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
            var draggers= transform.parent.GetComponentsInChildren<LayoutBorderDragger>();
            UnityEditor.Selection.objects =draggers.ToList().Select(x => { return x.gameObject; }) as GameObject[];

        }
#endif
    }
}
