using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public int Id 
    { 
        get;
        set;
    }
    public int X 
    { 
        get; 
        set;    
    }
    public int Y 
    { 
        get;
        set; 
    }

    public Point(int id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }
}

public class PointData : MonoBehaviour
{
    public int pointWidth = 4; 
    public int pointHeight = 4; 
    public GameObject pointPrefab; 
    public GameObject linePrefab;  

    private List<Point> Points; 
    private List<Edge> edges;   
    private struct Edge
    {
        public Vector2 startPoint;
        public Vector2 endPoint;

        public Edge(Vector2 start, Vector2 end)
        {
            startPoint = start;
            endPoint = end;
        }
    }

    void Start()
    {
        Points = new List<Point>();
        edges = new List<Edge>();
        GenerateGrid();
    }

    void GenerateGrid()
    {
        int id = 1;
        for (int y = 0; y < pointHeight; y++)
        {
            for (int x = 0; x < pointWidth; x++)
            {
                Points.Add(new Point(id, x, y));
                Vector2 pointPosition = new Vector2(x, y);
                Instantiate(pointPrefab, pointPosition, Quaternion.identity);
                id++;
            }
        }

        GenerateEdges();
    }

    
    void GenerateEdges()
    {
        GenerateBorders();
        GenerateInternalEdges();
    }

    void GenerateBorders()
    {
        for (int x = 0; x < pointWidth - 1; x++)
        {
            AddEdge(new Vector2(x, pointHeight - 1), new Vector2(x + 1, pointHeight - 1)); // Top border
            AddEdge(new Vector2(x, 0), new Vector2(x + 1, 0));                           // Bottom border
        }
        for (int y = 0; y < pointHeight - 1; y++)
        {
            AddEdge(new Vector2(0, y), new Vector2(0, y + 1));                           // Left border
            AddEdge(new Vector2(pointWidth - 1, y), new Vector2(pointWidth - 1, y + 1));  // Right border
        }
    }


    void GenerateInternalEdges()
    {
        for (int x = 1; x < pointWidth - 1; x++)
        {
            for (int y = 1; y < pointHeight - 1; y++)
            {
                Vector2 point = new Vector2(x, y);
                Vector2 neighbor = GetRandomNeighbor(point);
                if (neighbor != Vector2.zero && !EdgeExists(point, neighbor))
                {
                    AddEdge(point, neighbor);
                }
            }
        }
    }

   
    void AddEdge(Vector2 startPoint, Vector2 endPoint)
    {
        edges.Add(new Edge(startPoint, endPoint));
        DrawLine(startPoint, endPoint);
    }

    Vector2 GetRandomNeighbor(Vector2 point)
    {
        List<Vector2> neighbors = new List<Vector2>();

        if (point.x + 1 < pointWidth) neighbors.Add(new Vector2(point.x + 1, point.y)); // Right
        if (point.x - 1 >= 0) neighbors.Add(new Vector2(point.x - 1, point.y));         // Left
        if (point.y + 1 < pointHeight) neighbors.Add(new Vector2(point.x, point.y + 1)); // Up
        if (point.y - 1 >= 0) neighbors.Add(new Vector2(point.x, point.y - 1));         // Down

        if (neighbors.Count > 0)
        {
            return neighbors[Random.Range(0, neighbors.Count)];
        }

        return Vector2.zero;
    }


    bool EdgeExists(Vector2 pointA, Vector2 pointB)
    {
        foreach (Edge edge in edges)
        {
            if ((edge.startPoint == pointA && edge.endPoint == pointB) || (edge.startPoint == pointB && edge.endPoint == pointA))
            {
                return true;
            }
        }
        return false;
    }

    void DrawLine(Vector2 startPoint, Vector2 endPoint)
    {
        GameObject lineObj = Instantiate(linePrefab);
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(startPoint.x, startPoint.y, 0));
        lineRenderer.SetPosition(1, new Vector3(endPoint.x, endPoint.y, 0));
    }
}