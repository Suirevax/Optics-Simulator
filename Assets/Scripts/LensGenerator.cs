using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class LensGenerator : MonoBehaviour
{
    [SerializeField] public Vector2 lensSize = Vector2.one;

    [SerializeField] public Vector2 firstPoint = Vector2.zero;
    [SerializeField] public Vector2 secondPoint = Vector2.zero;
    [SerializeField] public Vector2 handlerFirstPoint = Vector2.zero;
    [SerializeField] public Vector2 handlerSecondPoint = Vector2.zero;
    
    
    [SerializeField] public Vector2 firstPoint2 = Vector2.zero;
    [SerializeField] public Vector2 secondPoint2 = Vector2.zero;
    [SerializeField] public Vector2 handlerFirstPoint2 = Vector2.zero;
    [SerializeField] public Vector2 handlerSecondPoint2 = Vector2.zero;
    [SerializeField] public uint pointsQuantity = 1;

    Vector2 lastFirstPoint = Vector2.negativeInfinity;
    Vector2 lastSecondPoint = Vector2.negativeInfinity;
    Vector2 lastHandlerFirstPoint = Vector2.negativeInfinity;
    Vector2 lastHandlerSecondPoint = Vector2.negativeInfinity;
    
    Vector2 lastFirstPoint2 = Vector2.negativeInfinity;
    Vector2 lastSecondPoint2 = Vector2.negativeInfinity;
    Vector2 lastHandlerFirstPoint2 = Vector2.negativeInfinity;
    Vector2 lastHandlerSecondPoint2 = Vector2.negativeInfinity;
    uint lastPointsQuantity = 0;

    [SerializeField] Material material = null;

    EdgeCollider2D edgeCollider2D = null;
    PolygonCollider2D childPolygonCollider2D = null;

    Mesh lensMesh = null;
    // Start is called before the first frame update
    void Start()
    {
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        childPolygonCollider2D = transform.GetChild(0).GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var halfLensSize = lensSize / 2;
        firstPoint = new Vector2(halfLensSize.x, halfLensSize.y);
        secondPoint = new Vector2(-halfLensSize.x, halfLensSize.y);
        firstPoint2 = new Vector2(halfLensSize.x, -halfLensSize.y);
        secondPoint2 = new Vector2(-halfLensSize.x, -halfLensSize.y);
        if (LensChanged())
        {
            Vector2[] curve1 = calculate2DPoints(firstPoint, secondPoint, handlerFirstPoint + firstPoint, handlerSecondPoint + secondPoint, pointsQuantity);
            Vector2[] curve2 = calculate2DPoints(firstPoint2, secondPoint2, handlerFirstPoint2 + firstPoint2, handlerSecondPoint2 + secondPoint2, pointsQuantity);
            GenerateEdgeCollider2D(curve1, curve2);
            lensMesh = GenerateLensMesh(curve1, curve2);

            lastFirstPoint = firstPoint;
            lastSecondPoint = secondPoint;
            lastHandlerFirstPoint = handlerFirstPoint;
            lastHandlerSecondPoint = handlerSecondPoint;
            lastFirstPoint2 = firstPoint2;
            lastSecondPoint2 = secondPoint2;
            lastHandlerFirstPoint2 = handlerFirstPoint2;
            lastHandlerSecondPoint2 = handlerSecondPoint2;
            lastPointsQuantity = pointsQuantity;
        }
        Graphics.DrawMesh(lensMesh, transform.position, transform.rotation, material, 0);
    }

    public void setLensProperties()
    {

    }

    private bool LensChanged()
    {
        return 
            lastFirstPoint != firstPoint
            || lastSecondPoint != secondPoint
            || lastHandlerFirstPoint != handlerFirstPoint
            || lastHandlerSecondPoint != handlerSecondPoint
            || lastFirstPoint2 != firstPoint2
            || lastSecondPoint2 != secondPoint2
            || lastHandlerFirstPoint2 != handlerFirstPoint2
            || lastHandlerSecondPoint2 != handlerSecondPoint2
            || lastPointsQuantity != pointsQuantity
            ;
    }

    private void GenerateEdgeCollider2D(Vector2[] curve1, Vector2[] curve2)
    {
        List<Vector2> edgePoints = curve1.ToList();
        var tmp = curve2.ToList();
        tmp.Reverse();
        edgePoints.AddRange(tmp);
        edgePoints.Add(curve1[0]);

        edgeCollider2D.points = edgePoints.ToArray();
        childPolygonCollider2D.points = edgePoints.ToArray();
    }

    Mesh GenerateLensMesh(Vector2[] curve1, Vector2[] curve2)
    {
        Vector3[] vertices = new Vector3[curve1.Count() + curve2.Count()];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[(curve1.Count() - 1) * 6];

        for (int i = 0; i <= pointsQuantity; i++)
        {
            var tmp = i * 2;
            vertices[tmp] = curve1[i];
            vertices[tmp + 1] = curve2[i];
        }

        int counter = 0;
        for (int segment = 0; segment < curve1.Count() - 1; segment++)
        {
            var tmp = segment * 2;

            triangles[counter++] = tmp;
            triangles[counter++] = 1 + tmp;
            triangles[counter++] = 2 + tmp;

            triangles[counter++] = 1 + tmp;
            triangles[counter++] = 3 + tmp;
            triangles[counter++] = 2 + tmp;
        }

        Mesh lensMesh = new Mesh();
        lensMesh.vertices = vertices;
        lensMesh.uv = uv.ToArray();
        lensMesh.triangles = triangles;

        return lensMesh;
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 handlerP0, Vector3 handlerP1, Vector3 p1)
    {
        float u = 1.0f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; //first term
        p += 3f * uu * t * handlerP0; //second term
        p += 3f * u * tt * handlerP1; //third term
        p += ttt * p1; //fourth term

        return p;
    }

    Vector2[] calculate2DPoints(Vector2 _firstPoint, Vector2 _secondPoint, Vector2 _handlerFirstPoint, Vector2 _handlerSecondPoint, uint _pointsQuantity)
    {
        List<Vector2> points = new List<Vector2>();

        points.Add(_firstPoint);
        points.Add(_firstPoint);
        for (int i = 1; i < _pointsQuantity; i++)
        {
            points.Add(CalculateBezierPoint((1f / _pointsQuantity) * i, _firstPoint, _handlerFirstPoint, _handlerSecondPoint, _secondPoint));
        }
        points.Add(_secondPoint);

        return points.ToArray();
    }
}
