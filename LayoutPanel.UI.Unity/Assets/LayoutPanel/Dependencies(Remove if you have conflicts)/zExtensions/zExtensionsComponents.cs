

using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

// v .0.2. addorgetcomponent<monobehaviour source> added
// v. 0.3 nonrecursive components
// v. 0.4 nonrecursive transform, gameobject, mono overloads 

/// oeverrides zRectExtensions

public static class zExtensionsComponents // to useful to be in namespace1
{
    /// <summary>
    /// Never returns null. If component cannot be found, it is added and instance is returned
    /// </summary>
    public static T AddOrGetComponent<T>(this MonoBehaviour mono) where T : Component
    {
        T t = mono.gameObject.GetComponent<T>();
        if (t == null) t = mono.gameObject.AddComponent<T>();
        return t;
    }

    /// <summary>
    /// Never returns null. If component cannot be found, it is added and instance is returned
    /// </summary>

    public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t == null) t = gameObject.AddComponent<T>();
        return t;
    }
    /// <summary>
    /// Never returns null. If component cannot be found, it is added and instance is returned
    /// </summary>
    public static T AddOrGetComponent<T>(this Transform transform) where T : Component
    {
        T t = transform.GetComponent<T>();
        if (t == null) t = transform.gameObject.AddComponent<T>();
        return t;
    }

    /// <summary>
    /// Rearches one level of transform (doesn't go deep)
    /// </summary>
    public static T[] GetComponentsInDirectChildren<T>(this MonoBehaviour mono, bool includeDisabled = true) where T : Component
    {
        return zExtensionsComponents.GetComponentsInDirectChildren<T>(mono.transform, includeDisabled);
    }
    /// <summary>
    /// Rearches one level of transform (doesn't go deep)
    /// </summary>
    public static T[] GetComponentsInDirectChildren<T>(this GameObject game, bool includeDisabled = true) where T : Component
    {
        return zExtensionsComponents.GetComponentsInDirectChildren<T>(game.transform, includeDisabled);
    }
    /// <summary>
    /// Rearches one level of transform (doesn't go deep)
    /// </summary>
    /// 
    public static T[] GetComponentsInDirectChildren<T>(this Transform transform, bool includeDisabled = true) where T : Component
    {
        List<T> components = new List<T>();
        for (int i = 0; i < transform.childCount; i++)
        {
            T t = transform.GetChild(i).GetComponent<T>();
            if (t != null && (includeDisabled || t.gameObject.activeSelf)) components.Add(t);
        }
        return components.ToArray();
    }
}

namespace Z
{

    public static class zExtensionsComponentsZ
    {




        public static void RemoveAllComponentsExcluding(this GameObject obj, params Type[] types)
        {
            Component[] c = obj.GetComponents<Component>();
            for (int i = c.Length - 1; i > 1; i--)
                GameObject.DestroyImmediate(c[i]);

        }
        public static GameObject[] GetGameObjectsWithComponent<T>() where T : Component
        {
            T[] foundObjects = GameObject.FindObjectsOfType<T>();
            GameObject[] g = new GameObject[foundObjects.Length];
            for (int i = 0; i < foundObjects.Length; i++)
                g[i] = foundObjects[i].gameObject;
            return g;
        }


        public static string NameOrNull(this MonoBehaviour source)
        {
            return (source == null ? "null" : source.name);
        }

        public static GameObject[] GetChildrenGameObjects(this GameObject go)
        {
            return GetChildrenGameObjects(go.transform);
        }

        public static GameObject[] GetChildrenGameObjects(this Transform transform)
        {
            GameObject[] children = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i).gameObject;
            }
            return children;
        }


        public static void DestroySmart(this Component c)
        {

            if (Application.isPlaying)
            {
                MonoBehaviour.Destroy(c);
            }
            else
            {
#if UNITY_EDITOR
                EditorApplication.delayCall += () => MonoBehaviour.DestroyImmediate(c);
#endif
            }


        }



        public static void CollapseComponent(this MonoBehaviour mono, Component c, bool expanded = false)
        {
#if UNITY_EDITOR
            if (c != null)
                UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
        }
        public static void CollapseComponent(this MonoBehaviour mono, bool expanded = false)
        {

#if UNITY_EDITOR
            Component c = mono;
            if (c != null)
                UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
        }


        public static void CollapseComponent(this Component c, bool expanded = false)
        {
#if UNITY_EDITOR
            if (c != null)
                UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
        }

        public static Transform[] GetChildren(this Transform transform)
        {
            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }
            return children;
        }

        public static GameObject[] GetChildren(this GameObject thisGo, bool deep = false)
        {
            if (!deep)
            {
                Transform t = thisGo.transform;
                GameObject[] c = new GameObject[t.childCount];
                for (int i = 0; i < c.Length; i++)
                {
                    c[i] = t.GetChild(i).gameObject;
                }
                return c;
            }
            else
            {
                Transform[] transforms = thisGo.GetComponentsInChildren<Transform>(true);
                GameObject[] c = new GameObject[transforms.Length - 1];
                for (int i = 0; i < c.Length; i++)
                {
                    c[i] = transforms[i + 1].gameObject;
                }
                return c;
            }
        }

        /// <summary>
        /// Gets children for an array, useful for editor selections 
        /// </summary>

        public static GameObject[] GetChildrenArray(this GameObject[] thisGoArray, bool deep = false)
        {
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < thisGoArray.Length; i++)
                children.AddRange((thisGoArray[i]).GetChildren(deep));


            return children.ToArray();
        }


        public static GameObject[] GetAllChildrenCalled(this GameObject[] thisGoArray, string name)
        {
            GameObject[] children = thisGoArray.GetChildrenArray(true);
            List<GameObject> namedObjects = new List<GameObject>();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].name.Equals(name)) namedObjects.Add(children[i]);
            }
            return namedObjects.ToArray();


        }
    }
}