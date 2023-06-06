using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
// using UnityEditor.Experimental.SceneManagement;

#endif
// namespace Z
// {
// some wrappers around standard Gizmos
// v.02 label
// v.03 linefade
// v.04 normal line and color wrappers
// v.05 prefabstaege
// v.07 cross
// v.07 debugcross
// drawfromray
public static class zGizmos
{

    public static void DrawCross2D(Vector3 position, float size = 0.05f)
    {
        Gizmos.DrawLine(position + new Vector3(-size, size, 0), position + new Vector3(size, -size, 0));
        Gizmos.DrawLine(position + new Vector3(size, size, 0), position + new Vector3(-size, -size, 0));
    }

    [System.Obsolete("Please use DrawPlus")]
    public static void DrawCross(Vector3 position, float size = 0.05f)
    {
        DrawPlus(position, size);
    }
    public static void DrawPlus(Vector3 position, float size = 0.05f)
    {
        Gizmos.DrawLine(position + new Vector3(-size, 0, 0), position + new Vector3(size, 0, 0));
        Gizmos.DrawLine(position + new Vector3(0, -size, 0), position + new Vector3(0, size, 0));
        Gizmos.DrawLine(position + new Vector3(0, 0, -size), position + new Vector3(0, 0, size));
    }
    public static void DrawPlusDebug(Vector3 position, float size = 0.05f)
    {
        Debug.DrawLine(position + new Vector3(-size, 0, 0), position + new Vector3(size, 0, 0));
        Debug.DrawLine(position + new Vector3(0, -size, 0), position + new Vector3(0, size, 0));
        Debug.DrawLine(position + new Vector3(0, 0, -size), position + new Vector3(0, 0, size));
    }
    public static void DrawPlus2D(Vector3 position, float size = 0.05f)
    {
        Gizmos.DrawLine(position + new Vector3(-size, 0, 0), position + new Vector3(size, 0, 0));
        Gizmos.DrawLine(position + new Vector3(0, -size, 0), position + new Vector3(0, size, 0));
    }
    public static void DrawStar(Vector3 position, float size = 0.05f)
    {
        Gizmos.DrawLine(position + new Vector3(0, -size, 0), position + new Vector3(0, size, 0)); //vertical
        size *= 0.7071f;
        Gizmos.DrawLine(position + new Vector3(-size, -size, -size), position + new Vector3(size, size, size));
        Gizmos.DrawLine(position + new Vector3(size, -size, -size), position + new Vector3(-size, size, size));
        Gizmos.DrawLine(position + new Vector3(-size, size, -size), position + new Vector3(size, -size, size));
        Gizmos.DrawLine(position + new Vector3(size, size, -size), position + new Vector3(-size, -size, size));
    }

    public static bool PrefabModeIsActive(GameObject gameObject) //https://stackoverflow.com/questions/56155148/how-to-avoid-the-onvalidate-method-from-being-called-in-prefab-mode
    {
#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
        UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
        if (prefabStage != null)
            return true;
        if (UnityEditor.EditorUtility.IsPersistent(gameObject))
            return true;
#endif
        return false;

    }
    //     public static bool IsPrefabInstance(GameObject obj) //https://answers.unity.com/questions/1599065/how-can-i-use-onvalidate-with-prefabs.html
    //     {
    //         bool is_prefab_instance = false;
    // #if UNITY_EDITOR
    //         is_prefab_instance = UnityEditor.PrefabUtility.IsPartOfAnyPrefab(obj) &&
    //             !UnityEditor.PrefabUtility.IsPartOfPrefabAsset(obj) &&
    //             UnityEditor.PrefabUtility.IsPartOfNonAssetPrefabInstance(obj);
    // #endif
    //         return is_prefab_instance;
    //     }
    //     // In project or prefab editor.
    //     public static bool IsPrefabAsset(GameObject obj) //https://answers.unity.com/questions/1599065/how-can-i-use-onvalidate-with-prefabs.html
    //     {
    //         bool is_prefab_asset = false;
    // #if UNITY_EDITOR
    //         var stage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
    //         is_prefab_asset = (stage != null &&
    //                 stage.scene == obj.scene) ||
    //             (UnityEditor.PrefabUtility.IsPartOfAnyPrefab(obj) &&
    //                 UnityEditor.PrefabUtility.IsPartOfPrefabAsset(obj) &&
    //                 !UnityEditor.PrefabUtility.IsPartOfNonAssetPrefabInstance(obj));
    // #endif
    //         return is_prefab_asset;
    //     }
    public static void Label(Vector3 position, string text, float offset = -0.01f)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.Label(position + Vector3.up * offset, text);
#endif
    }
    public static void DrawLineDashed(Vector3 position, Vector3 position2, float size = 0.1f)
    {
        float distance = (position - position2).magnitude;
        float step = size / distance;
        for (float i = 0; i < 1; i += step)
        {
            Gizmos.DrawLine(Vector3.Lerp(position, position2, i), Vector3.Lerp(position, position2, i + step));
            i += step;
        }

    }
    public static Color color { get { return Gizmos.color; } set { Gizmos.color = value; } }
    public static void DrawLine(Vector3 from, Vector3 to)
    {

        Gizmos.DrawLine(from, to);
    }
    static Vector3 rayDir = Vector3.left;
    public static void SetRayDir(Vector3 r)
    {
        rayDir = r;
    }
    public static void DrawFromRay(Vector3 origin)
    {
        DrawLine(origin, origin + rayDir);
    }

    public static void DrawFromRay(Vector3 origin, Vector3 dst, Color colorA, Color colorB, float size = 0.1f)
    {

        float distance = (origin - dst).magnitude;
        if (distance == 0) return;
        float step = size / distance;
        for (float i = 0; i < 1; i += step)
        {
            Gizmos.color = Color.Lerp(colorA, colorB, i);
            DrawFromRay(Vector3.Lerp(origin, dst, i));
        }
    }
    public static void DrawFromRay(Vector3 origin, Vector3 dst, float size = 0.1f)
    {

        float distance = (origin - dst).magnitude;
        if (distance == 0) return;
        float step = size / distance;
        for (float i = 0; i < 1; i += step)
        {
            DrawFromRay(Vector3.Lerp(origin, dst, i));
        }
    }
    public static void DrawLineDashed(Vector3 position, Vector3 position2, Color A, Color B, float size = 0.1f)
    {
        float distance = (position - position2).magnitude;
        if (distance == 0) return;
        float step = size / distance;
        for (float i = 0; i < 1; i += step)
        {
            Gizmos.color = Color.Lerp(A, B, i);
            Gizmos.DrawLine(Vector3.Lerp(position, position2, i), Vector3.Lerp(position, position2, i + step));
            i += step;
        }

    }
    public static void DrawLineFade(Vector3 position, Vector3 position2, Color A, Color B, float size = 0.1f)
    {
        float distance = (position - position2).magnitude;
        float step = size / distance;
        for (float i = 0; i < 1; i += step)
        {
            Gizmos.color = Color.Lerp(A, B, i);
            Gizmos.DrawLine(Vector3.Lerp(position, position2, i), Vector3.Lerp(position, position2, i + step));
            // i += step;
        }
    }
    public static void DrawPath(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length - 1; i++)
            Gizmos.DrawLine(positions[i], positions[i + 1]);
    }

    public static void DrawPath(List<Vector3> positions)
    {
        for (int i = 0; i < positions.Count - 1; i++)
            Gizmos.DrawLine(positions[i], positions[i + 1]);
    }
    public static void DrawPath(Vector3[] positions, Vector3 offset)
    {
        for (int i = 0; i < positions.Length - 1; i++)
            Gizmos.DrawLine(positions[i] + offset, positions[i + 1] + offset);
    }
    public static void DrawPath(List<Vector3> positions, Vector3 offset)
    {
        for (int i = 0; i < positions.Count - 1; i++)
            Gizmos.DrawLine(positions[i] + offset, positions[i + 1] + offset);
    }
    public static void DrawRays(List<Ray> rays, Vector3 offset, float length = 400)
    {
        for (int i = 0; i < rays.Count; i++)
            Gizmos.DrawRay(rays[i].origin + offset, rays[i].direction * length); //rays[i].origin +
    }
    public static void DrawRays(List<Ray> rays, Vector3 offset, int from, int to, float length = 400)
    {
        for (int i = from; i < to; i++)
            Gizmos.DrawRay(rays[i].origin + offset, rays[i].direction * length); //rays[i].origin +
    }
    public static void DrawRays(List<Ray> rays, float length = 400)
    {
        for (int i = 0; i < rays.Count; i++)
            Gizmos.DrawRay(rays[i].origin, rays[i].direction * length); //rays[i].origin +
    }
    public static void DrawPath(Vector3[] positions, Vector3 offset, bool loop)
    {
        for (int i = 0; i < positions.Length - 1; i++)
            Gizmos.DrawLine(positions[i] + offset, positions[i + 1] + offset);
        if (loop)
            Gizmos.DrawLine(positions[positions.Length - 1] + offset, positions[0] + offset);
    }
    public static void DrawPath(Vector3[] positions, Transform referenceTransform)
    {
        DrawPath(positions, referenceTransform.position);
    }
    public static void DrawPath(List<Vector3> positions, Transform referenceTransform)
    {
        DrawPath(positions, referenceTransform.position);
    }
    public static void DrawPath(Vector3[] positions, Color A, Color B, bool pingpongColor = false)
    {
        if (HasLessThanTwoElements(positions)) return;
        if (pingpongColor)
        {
            DrawPathPingpong(positions, A, B);
            return;
        }
        float step = 1f / positions.Length;
        for (int i = 0; i < positions.Length - 1; i++)
        {
            Gizmos.color = Color.Lerp(A, B, i * step);
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
    }
    public static List<Vector3> GetTransformed(this List<Vector3> positions, Transform transform)
    {
        var list = new List<Vector3>();
        for (int i = 0; i < positions.Count; i++)
            list.Add(transform.TransformPoint(positions[i]));
        return list;
    }
    public static Vector3[] GetTransformed(this Vector3[] positions, Transform transform)
    {
        var list = new Vector3[positions.Length];
        for (int i = 0; i < positions.Length; i++)
            list[i] = transform.TransformPoint(positions[i]);
        return list;
    }

    public static void DrawPath(List<Vector3> positions, Color A, Color B, bool pingpongColor = false)
    {
        if (HasLessThanTwoElements(positions)) return;
        if (pingpongColor)
        {
            DrawPathPingpong(positions, A, B);
            return;
        }
        float step = 1f / positions.Count;
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Gizmos.color = Color.Lerp(A, B, i * step);
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
    }
    static void DrawPathPingpong(Vector3[] positions, Color A, Color B)
    {
        if (HasLessThanTwoElements(positions)) return;
        float step = 2f / positions.Length;
        int half = positions.Length / 2;
        for (int i = 0; i < half - 1; i++)
        {
            Gizmos.color = Color.Lerp(A, B, i * step);
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
        for (int i = half; i < positions.Length - 1; i++)
        {
            Gizmos.color = Color.Lerp(A, B, 1 - i * step);
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
    }
    static void DrawPathPingpong(List<Vector3> positions, Color A, Color B)
    {
        if (HasLessThanTwoElements(positions)) return;
        int last = positions.Count;
        float step = 2f / (last - 1);
        int half = last / 2;
        for (int i = 0; i < half; i++)
        {
            Gizmos.color = Color.Lerp(A, B, i * step);
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
        for (int i = half; i < last - 1; i++)
        {
            Gizmos.color = Color.Lerp(A, B, (last - i) * step);
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
    }
    static bool IsEmpty(Vector3[] pos)
    {
        if (pos == null) return true;
        if (pos.Length == 0) return true;
        return false;
    }
    static bool IsEmpty(List<Vector3> pos)
    {
        if (pos == null) return true;
        if (pos.Count == 0) return true;
        return false;
    }
    static bool HasLessThanTwoElements(Vector3[] pos)
    {
        if (pos == null) return true;
        if (pos.Length <= 2) return true;
        return false;
    }
    static bool HasLessThanTwoElements(List<Vector3> pos)
    {
        if (pos == null) return true;
        if (pos.Count <= 2) return true;
        return false;
    }
}
// }