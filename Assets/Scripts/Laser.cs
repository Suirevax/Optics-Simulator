using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;


//[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField] uint maxReflections = 100;
    [SerializeField] float defaultRayDistance = 100;
    [SerializeField] float laserWidth = 0.2f;
    [SerializeField] Material material = null;

    [SerializeField] GameObject PointIndicator;
    [SerializeField] bool drawIndicators;

    void Update()
    {
        List<Vector3> vertices = new List<Vector3>();
        int laserSegmentCount = 0;
        Vector2 startDirection = new Vector2 (Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));

        
        NextSegment(ref vertices, ref laserSegmentCount, transform.position, startDirection);

        Graphics.DrawMesh(createMesh(vertices, laserSegmentCount), new Vector3(0, 0, -3), Quaternion.identity, material, 0);

        //string verticesS = "";

        //foreach(var i in vertices)
        //{
        //    verticesS += "[" + i.x + "," + i.y + "] ";
        //}

        //Debug.Log(verticesS);

        foreach(var i in GameObject.FindGameObjectsWithTag("Indicator"))
        {
            Destroy(i);
        }



        if (drawIndicators)
        {
            foreach (var i in vertices)
            {
                var tmp = Instantiate(PointIndicator);
                tmp.transform.position = i;
            }
        }

        Debug.Log("End");

    }

    private void NextSegment(ref List<Vector3> vertices, ref int laserSegmentCount, in Vector2 startPosition, in Vector2 direction )
    {
        if (laserSegmentCount++ > maxReflections) return;

        LayerMask layerMask = LayerMask.GetMask("Laser");
        RaycastHit2D hitData = Physics2D.Raycast(startPosition + (direction * Mathf.Pow(10, -5)), direction, defaultRayDistance, layerMask);
        AddStartVertices(ref vertices, startPosition, direction);

        if (hitData)
        {
            vertices.Add(hitData.point + Vector2.Perpendicular(direction) * laserWidth);
            vertices.Add(hitData.point - Vector2.Perpendicular(direction) * laserWidth);

            if (hitData.collider.CompareTag("Mirror"))
            {
                NextSegment(ref vertices, ref laserSegmentCount, hitData.point, Vector2.Reflect(direction, hitData.normal));
            }
            else if (hitData.collider.CompareTag("Transparent"))
            {
                float pointCalcMultiplier = 1 * Mathf.Pow(10, -3); 

                var outerPoint = hitData.point - (hitData.normal * pointCalcMultiplier);
                var innerPoint = hitData.point + (hitData.normal * pointCalcMultiplier);

                LayerMask pointLayerMask = LayerMask.GetMask("LensType");
                var outerOverlap = Physics2D.OverlapPoint(outerPoint, pointLayerMask);
                var innerOverlap = Physics2D.OverlapPoint(innerPoint, pointLayerMask);

                float innerBreakIndex = 1;
                float outerBreakIndex = 1;

                if (outerOverlap != null) outerBreakIndex = 1.6f;
                if (innerOverlap != null) innerBreakIndex = 1.6f;

                var entryAngle = Vector2.SignedAngle(-direction, hitData.normal);
                var exitAngle = Mathf.Asin(Mathf.Sin(entryAngle * Mathf.Deg2Rad) / (outerBreakIndex / innerBreakIndex));
                var angle = Vector2.SignedAngle(Vector2.right, -hitData.normal) * Mathf.Deg2Rad - exitAngle;
                Vector2 exitVector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                NextSegment(ref vertices, ref laserSegmentCount, hitData.point, exitVector.normalized);
            }
            else
            {
                return;
            }
        }
        else
        {
            var laserEnd = startPosition + direction * defaultRayDistance;
            vertices.Add(laserEnd + Vector2.Perpendicular(direction) * laserWidth);
            vertices.Add(laserEnd - Vector2.Perpendicular(direction) * laserWidth);
        }
    }

    private void AddStartVertices(ref List<Vector3> vertices, in Vector2 startPoint, in Vector2 startDirection)
    {
        vertices.Add(startPoint + Vector2.Perpendicular(startDirection) * laserWidth);
        vertices.Add(startPoint - Vector2.Perpendicular(startDirection) * laserWidth);
    }

    private Mesh createMesh(in List<Vector3> vertices, in int laserSegmentCount)
    {
        Vector2[] uv = new Vector2[vertices.Count];
        int[] triangles = new int[laserSegmentCount * 6]; //2 triangles per segment means 6 data values per segment

        int counter = 0;

        for(int segment = 0; segment < laserSegmentCount; segment++)
        {
            var tmp = segment * 4;

            triangles[counter++] = tmp;
            triangles[counter++] = 3 + tmp;
            triangles[counter++] = 1 + tmp;

            triangles[counter++] = tmp;
            triangles[counter++] = 2 + tmp;
            triangles[counter++] = 3 + tmp;
        }

        Mesh laserMesh = new Mesh();
        laserMesh.vertices = vertices.ToArray();
        laserMesh.uv = uv;
        laserMesh.triangles = triangles;

        return laserMesh;
    }
}