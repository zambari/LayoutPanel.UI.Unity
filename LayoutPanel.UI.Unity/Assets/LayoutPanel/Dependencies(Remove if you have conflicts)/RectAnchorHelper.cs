using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
//v.0.2. symmetrical mode

//v.0.3 deactivation on assembly reload
//v.0.4 margin control

namespace zUI
{
    [ExecuteInEditMode]
    public class RectAnchorHelper : MonoBehaviour
    {

        [SerializeField] bool edit;
        [SerializeField] bool symmetricalXMode;

        [SerializeField] bool symmetricalYMode;
        [SerializeField] RectTransform rect;
        [Range(0, 1)]
        [SerializeField] float xAnchorMin;
        [Range(0, 1)]
        [SerializeField] float xAnchorMax;
        //public bool showSymmetrical {get { return _showSymmetrical}}
        //[SerializeField] bool _showSymmetrical;
        [Range(0, 1)]
        [SerializeField] float yAnchorMin;
        [Range(0, 1)]
        [SerializeField] float yAnchorMax;

        [SerializeField][HideInInspector] Vector2 offsetMin;
        [SerializeField][HideInInspector]  Vector2 offsetMax;
        [Range(-1, 100)]
        [SerializeField] float margin = -1;
        void Reset()
        {
            GetValues();
        }
        void OnValidate()

        {
            if (Application.isPlaying) return;
            if (rect == null) rect = GetComponent<RectTransform>();
            if (symmetricalXMode) xAnchorMax = 1 - xAnchorMin;
            if (symmetricalYMode) yAnchorMax = 1 - yAnchorMin;
            if (edit)
            {
                SetValues();
            }
            else
                GetValues();
        }

        void SetValues()
        {
            rect.anchorMin = new Vector2(xAnchorMin, yAnchorMin);
            rect.anchorMax = new Vector2(xAnchorMax, yAnchorMax);
            if (margin != -1)
            {
                rect.offsetMin = new Vector2(margin, margin);
                rect.offsetMax = new Vector2(-margin, -margin);
            }
        }
        void GetValues()
        {
            if (rect == null) rect = GetComponent<RectTransform>();
            xAnchorMin = rect.anchorMin.x;
            xAnchorMax = rect.anchorMax.x;
            yAnchorMin = rect.anchorMin.y;
            yAnchorMax = rect.anchorMax.y;
            offsetMin = rect.offsetMin;
            offsetMax = rect.offsetMax;
        }

   
    }

}
#endif
