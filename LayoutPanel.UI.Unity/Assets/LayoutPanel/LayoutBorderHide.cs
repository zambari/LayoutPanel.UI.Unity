

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        public BorderHideMode borderHideMode
        {
            get { return _borderHideMode; }
            set
            {
                _borderHideMode = value;
                for (int i = 0; i < transform.childCount; i++)
                {
                    var thisGame = transform.GetChild(i).gameObject;
                    if (thisGame.name.Contains(LayoutBorderDragger.baseName))
                    {
                        if (_borderHideMode == BorderHideMode.TopOnly)
                        {
                            if (thisGame.name.Contains("Top"))
                                thisGame.hideFlags = (_borderHideMode == BorderHideMode.TopOnly ? HideFlags.None : HideFlags.HideInHierarchy);
                            else thisGame.hideFlags = HideFlags.HideInHierarchy;
                        }
                        else
                        {
                            thisGame.hideFlags = (_borderHideMode == BorderHideMode.Hidden ? HideFlags.HideInHierarchy : HideFlags.None);
                        }
                    }
                }

                zExt.RepaintHierarchy();

#if UNITY_EDITOR

                /*    var sel = Selection.activeGameObject;
                Selection.activeGameObject = null;
                EditorApplication.delayCall += () => Selection.activeGameObject = sel;*/
#endif
            }
        }
        [ClickableEnum]
        [SerializeField]
        BorderHideMode _borderHideMode;


        [SerializeField] bool _editColors;
        public bool editColors
        {
            get { return _editColors; }
            set { _editColors = value; OnValidate(); }
        }
        public Color borderColor;
#if UNITY_EDITOR
        void Reset()
        {
            for (int i = 0; i < 41; i++)
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);
            borderHideMode = BorderHideMode.Hidden;

            var b = GetComponentInChildren<LayoutBorderDragger>();
            if (b != null) borderColor = b.GetComponent<Image>().color;
            else borderColor = Color.gray * 0.5f;

        }
#endif
        void OnValidate()
        {
            borderHideMode = _borderHideMode;
            if (editColors)
            {
                var bs = GetComponentsInChildren<LayoutBorderDragger>();
                foreach (var b in bs)
                    b.GetComponent<Image>().color = borderColor;
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
