using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
// v0.02 favourite object added
// v0.03/02 UNDO
// v0.04  namespac
// v.05 unityeditor !
namespace Z
{


/// <summary>
/// 
/// 
///   This class binds ` (tile, the key below escape) to toggling active state of a selected objects.
/// 
/// 
/// </summary>


    public static class ObjectEnableToggle
    {
        /*  [MenuItem("Tools/Actions/Select Parent %`")] 
           static void selParent()
           {
               if (Selection.activeGameObject != null && Selection.activeGameObject.transform.parent!=null)
               {

                  Selection.activeGameObject=Selection.activeGameObject.transform.parent.gameObject;
               }
           }*/
        static GameObject selectedObject;
        [MenuItem("Tools/Actions/Toggle Enabled  _`")]
        static void ToggleEnabled()
        {
            if (Selection.activeGameObject != null)
            {
                bool newActiveStatus = !Selection.activeGameObject.activeSelf;
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                    ToggleActiveStatus(Selection.gameObjects[i], newActiveStatus);
            }
        }
        [MenuItem("Tools/Actions/Select favourite Togglable object  _%#`")]
        static void SelectTogglable()
        {
            if (Selection.activeGameObject != null)
            {
                selectedObject = Selection.activeGameObject;
                Debug.Log(selectedObject.name + " was marked for control tilde toggle");
            }
        }
        [MenuItem("Tools/Actions/Toggle favourite object  _#`")]
        static void SelectedTogglableToggle()
        {
            if (selectedObject != null)
            {
                ToggleActiveStatus(selectedObject, !selectedObject.activeSelf);
            }
            else Debug.Log("No favourite object, please mark it with ctrl+shift+` ");
        }
        static void ToggleActiveStatus(GameObject o, bool status)
        {
            Undo.RecordObject(o, "EnableToggle");
            o.SetActive(status);
            if (o.activeSelf
             && !o.activeInHierarchy)
            {
                Transform thisTransform = o.transform.parent;
                while (thisTransform != null)
                {
                    if (thisTransform.gameObject.activeInHierarchy == false)
                    {
                        Undo.RecordObject(thisTransform.gameObject, "EnableToggle");
                        thisTransform.gameObject.SetActive(true);
                    }

                    thisTransform = thisTransform.parent;
                }
            }
        }
    }
}
#endif