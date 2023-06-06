using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// removed leaking cross mode
// update 

[ExecuteInEditMode]
public class RectGizmo : MonoBehaviour {

    [Header ("Z offset")]
    [Range (-3, 3)]
    public float depthdistance = 6;

    [Range (0, 20)]
    public float edgeMargin = 0;
    [Range (20, .2f)]
    public float density = 5;
    RectTransform[] rects;

    public int fadeSteps = 5;

    [Range (0, 10)]
    public float maxdistance = 10;
    void OnEnable () {
        if (fadeColor == mainColor) {
            fadeColor.a = 0;
            mainColor.a = 0.5f;
        }
    }
    GameObject lastSelected;
    List<Vector3> points = new List<Vector3> ();
    Vector3[] corners = new Vector3[4];
    public Color mainColor = Color.yellow;
    public Color fadeColor = Color.yellow;

    //  List<Ray> rays = new List<Ray>();
    // List<Ray> rays2 = new List<Ray>();

    [Range (0, 10)]
    public float depthStep = .5f;

    public bool drawSquare = true;
    [Range (0.15f, 1f)]
    public float rayRatio = 1;
    void OnValidate () {
        lastSelected = null;
    }
    void OnDrawGizmos () {
#if UNITY_EDITOR
        GameObject selectedObject = UnityEditor.Selection.activeGameObject;

        if (lastSelected != selectedObject) {
            lastSelected = selectedObject;
            GetPoints (selectedObject);
        }
        if (selectedObject == null) return;
        Vector3 offset = Vector3.zero;

        if (drawSquare) {
            for (int i = 0; i < fadeSteps * 3; i++) {
                Gizmos.color = Color.Lerp (mainColor, fadeColor, i * 1f / fadeSteps * 3);
                offset += Vector3.forward * depthdistance * depthdistance * depthdistance;
                //Gizmos.DrawWireCube(selectedRect.)
                zGizmos.DrawPath (corners, offset, true);
                zGizmos.DrawPath (corners, -offset, true);
            }
        } else {
            /*   offset = -Vector3.forward * depthdistance / fadeSteps;
               Gizmos.color = mainColor;
               for (int i = 0; i < fadeSteps; i++)
               {
                   Gizmos.color = Color.Lerp(mainColor, fadeColor, (i * 1f / fadeSteps));
                   zGizmos.DrawRays(rays, offset, Mathf.FloorToInt(i * 1f * rays.Count / fadeSteps), Mathf.FloorToInt((i + 1f) / fadeSteps * rays.Count), maxdistance / fadeSteps);

               }

               Gizmos.color = fadeColor;
               zGizmos.DrawRays(rays2, offset, maxdistance);
   */

        }
#endif

    }
    RectTransform thisRect;

    Vector3 Quantize (Vector3 v) {
        return v;
        //return (new Vector3(Mathf.Round(v.x * quantize), Mathf.Round(v.y * quantize))) / quantize;
    }
    void GetPoints (GameObject selectedObject) {
        // if (selectedObject == null) 
        // {
        points.Clear ();
        //   rays.Clear();
        if (selectedObject == null) return;
        // }
        thisRect = selectedObject.GetComponent<RectTransform> ();
        if (thisRect == null) return;
        thisRect.GetWorldCorners (corners);
        Vector3 l = Vector3.left * edgeMargin * edgeMargin;
        Vector3 u = Vector3.up * edgeMargin * edgeMargin;
        corners[0] += l - u;
        corners[1] += l + u;
        corners[2] += -l + u;
        corners[3] += -l - u;
        float step = density;

        float startx = corners[1].x;
        float endx = corners[2].x;
        float starty = corners[0].y;
        float endy = corners[1].y;
        // rays.Clear();

        float thisx = startx;
        Vector3 starpoint = corners[0];
        Vector3 endPoint = corners[1];; //s corners[1] + tl;
        Vector3 offset = Vector3.up * density;
        // while (starpoint.y < endPoint.y && a)
        // {
        //     rays.Add(new Ray(starpoint, tl));
        //     if (c) rays.Add(new Ray(starpoint, bl));
        //     starpoint += offset;
        //     //  endPoint += offset;
        // }
        float stepx = density / thisRect.rect.width;
        float stepy = density / thisRect.rect.height;
        // var tl = (Vector3.up + Vector3.left);
        // var tr = (Vector3.up + Vector3.right);
        // var bl = (Vector3.down + Vector3.left);
        // var br = (Vector3.down + Vector3.right);
        /*
        if (a) for (float i = -extraDistance4; i < 1 + extraDistance5; i += stepx)
            {
                // rays.Add(new Ray(Vector3.LerpUnclamped(starpoint, endPoint, i), new Vector3(-1,-1,0)));
                Vector3 point = Vector3.LerpUnclamped(Quantize(corners[2]), Quantize(corners[1]), i);
                // (i > 0) rays.Add(new Ray(point, new Vector3(1, 1, 0))); //new Vector3(1,-1,0)

                Vector3 thispoint = point;
                // if (i > 1) thispoint = Vector3.Lerp(thispoint, thispoint + dir, 1 - i);
                rays.Add(new Ray(Quantize(thispoint), new Vector3(-1, 1, 0))); //new Vector3(1,-1,0)

                // rays.Add(new Ray(Vector3.LerpUnclamped(starpoint+Vector3.down*disty, endPoint+Vector3.down*disty, i), bl));
            }
        if (b) for (float i = -extraDistance4; i < 1 + extraDistance5; i += stepx)
            {
                Vector3 point = Vector3.LerpUnclamped(Quantize(corners[0]), Quantize(corners[3]), i);
                Vector3 thispoint = point;
                rays.Add(new Ray(Quantize(thispoint), new Vector3(+1, -1, 0))); //new Vector3(1,-1,0)

            }
        if (c) for (float i = -extraDistance4; i < 1 + extraDistance5; i += stepy)
            {
                Vector3 point = Vector3.LerpUnclamped(Quantize(corners[0]), Quantize(corners[1]), i);
                Vector3 thispoint = point;
                rays.Add(new Ray(Quantize(thispoint), new Vector3(-1, 1, 0))); //new Vector3(1,-1,0)

            }
        if (d) for (float i = -extraDistance4; i < 1 + extraDistance5; i += stepy)
            {
                Vector3 point = Vector3.LerpUnclamped(Quantize(corners[2]), Quantize(corners[3]), i);
                // if (i > 0) 
                Vector3 thispoint = point;
                rays.Add(new Ray(Quantize(thispoint), new Vector3(1, -1, 0))); //new Vector3(1,-1,0)

            }



        if (e) for (float i = -extraDistance2; i < 1; i += stepx)
            {
                // rays.Add(new Ray(Vector3.LerpUnclamped(starpoint, endPoint, i), new Vector3(-1,-1,0)));
                Vector3 point = Vector3.LerpUnclamped(Quantize(corners[1]), Quantize(corners[2]), i);
                Vector3 thispoint = point;
                rays.Add(new Ray(Quantize(thispoint), new Vector3(1, 1, 0))); //new Vector3(1,-1,0)

                // rays.Add(new Ray(Vector3.LerpUnclamped(starpoint+Vector3.down*disty, endPoint+Vector3.down*disty, i), bl));
            }
        if (f) for (float i = -extraDistance2; i < 1; i += stepx)
            {
                Vector3 point = Vector3.LerpUnclamped(Quantize(corners[0]), Quantize(corners[3]), i);
                Vector3 thispoint = point;
                rays.Add(new Ray(Quantize(thispoint), new Vector3(-1, -1, 0))); //new Vector3(1,-1,0)
                                                                                // if (i < 1) rays.Add(new Ray(point, new Vector3(+1, -1, 0))); //new Vector3(1,-1,0)

            }
        if (g) for (float i = -extraDistance2; i < 1; i += stepy)
            {
                Vector3 point = Vector3.LerpUnclamped(Quantize(corners[0]), Quantize(corners[1]), i);
                Vector3 thispoint = point;
                rays.Add(new Ray(Quantize(thispoint), new Vector3(-1, -1, 0))); //new Vector3(1,-1,0)
                                                                                // if (i < 1) rays.Add(new Ray(point, new Vector3(-1, 1, 0))); //new Vector3(1,-1,0)

            }
        if (h) for (float i = -extraDistance2; i < 1; i += stepy)
            {
                Vector3 point = Vector3.LerpUnclamped(Quantize(corners[2]), Quantize(corners[3]), i);
                Vector3 thispoint = point;
                rays.Add(new Ray(point, new Vector3(1, 1, 0))); //new Vector3(1,-1,0)
                                                                //  if (i < 1) rays.Add(new Ray(point, new Vector3(1, -1, 0))); //new Vector3(1,-1,0)

            }
        int oneCycleCount = rays.Count;
        var rayst2 = new List<Ray>(rays);

        for (int j = 0; j < fadeSteps; j++)
        {
            Vector3 o = -Vector3.forward * depthStep * depthStep * j;
            for (int i = 0; i < oneCycleCount; i++)
            {
                var thisray = rayst2[i];
                thisray.origin += thisray.direction * maxdistance * rayRatio * j + o;
                rays.Add(thisray);
            }
        }
        // }


        // while (thispoint.x < endPoint.x && b)
        // {
        //     rays.Add(new Ray(Quantize( thispoint), endPoint));
        //     // rays.Add(new Ray(starpoint2, endPoint));
        //     thispoint += offset;
        //     // starpoint2 += offset;
        //     //endPoint2+=off
        //     //  endPoint += offset;
        // }
        // while (starpoint.x < endPoint.x && c)
        // {
        //     // rays.Add(new Ray(starpoint, endPoint));
        //     rays.Add(new Ray(starpoint2, endPoint));
        //     starpoint += offset;
        //     // starpoint2 += offset;
        //     //endPoint2+=off
        //     //  endPoint += offset;
        // }
        */
    }
    void Update () {
        if (lastSelected != null) {
            if (lastSelected.transform.hasChanged) {
                lastSelected.transform.hasChanged = false;
                GetPoints (lastSelected);
            }
        }
    }
}