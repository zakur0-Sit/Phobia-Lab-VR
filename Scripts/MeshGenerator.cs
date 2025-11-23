using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;
    public MeshFilter walls;
    public MeshFilter cave;
    
    List<Vector3> vertices;
    List<int> triangles;
    
    Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVertices = new HashSet<int>();
    
    public void GenerateMesh(int[,] map, float squareSize)
    {
        triangleDictionary.Clear();
        outlines.Clear();
        checkedVertices.Clear();
        
        squareGrid = new SquareGrid(map, squareSize);
        vertices = new List<Vector3>();
        triangles = new List<int>();
        
        for (int x = 0; x < squareGrid.squares.GetLength(0); ++x)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); ++y)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }
        }
        Mesh mesh = new Mesh();
        cave.mesh = mesh;
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        CreateWallMesh();
    }

    void CreateWallMesh()
    {
        CalculateOutlines();
        
        List<Vector3> wallVertices = new List<Vector3>();
        List<int> wallTriangles = new List<int>();
        Mesh wallMesh = new Mesh();
        float wallHeight = 5f;

        foreach (List<int> outline in outlines)
        {
            for (int i = 0; i < outline.Count - 1; ++i)
            {
                int startIndex = wallVertices.Count;
                wallVertices.Add(vertices[outline[i]]); // left vertex
                wallVertices.Add(vertices[outline[i + 1]]); // right vertex
                wallVertices.Add(vertices[outline[i]] - Vector3.up * wallHeight); // left bottom vertex
                wallVertices.Add(vertices[outline[i + 1]] - Vector3.up * wallHeight); // right bottom vertex
                
                wallTriangles.Add(startIndex);
                wallTriangles.Add(startIndex + 2);
                wallTriangles.Add(startIndex + 3);
                
                wallTriangles.Add(startIndex + 3);
                wallTriangles.Add(startIndex + 1);
                wallTriangles.Add(startIndex);
            }
        }

        wallMesh.vertices = wallVertices.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        walls.mesh = wallMesh;
        
        MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider>();
        wallCollider.sharedMesh = wallMesh;
    }

    void TriangulateSquare(Square sqare)
    {
        switch (sqare.configuration)
        {
            case 0:
                break;
            case 1:
                MeshFromPoints(sqare.centerLeft, sqare.centerBottom, sqare.bottomLeft);
                break;
            case 2:
                MeshFromPoints(sqare.bottomRight, sqare.centerBottom, sqare.centerRight);
                break;
            case 4:
                MeshFromPoints(sqare.topRight, sqare.centerRight, sqare.centerTop);
                break;
            case 8:
                MeshFromPoints(sqare.topLeft, sqare.centerTop, sqare.centerLeft);
                break;
            
            case 3:
                MeshFromPoints(sqare.centerRight, sqare.bottomRight, sqare.bottomLeft, sqare.centerLeft);
                break;
            case 6:
                MeshFromPoints(sqare.centerTop, sqare.topRight, sqare.bottomRight, sqare.centerBottom);
                break;
            case 9:
                MeshFromPoints(sqare.topLeft, sqare.centerTop, sqare.centerBottom, sqare.bottomLeft);
                break;
            case 12:
                MeshFromPoints(sqare.topLeft, sqare.topRight, sqare.centerRight, sqare.centerLeft);
                break;
            
            case 5:
                MeshFromPoints(sqare.centerTop, sqare.topRight, sqare.centerRight, sqare.centerBottom, sqare.bottomLeft, sqare.centerLeft);
                break;
            case 10:
                MeshFromPoints(sqare.topLeft, sqare.centerTop, sqare.centerRight, sqare.bottomRight, sqare.centerBottom, sqare.centerLeft);
                break;
            
            case 7:
                MeshFromPoints(sqare.centerTop, sqare.topRight, sqare.bottomRight, sqare.bottomLeft, sqare.centerLeft);
                break;
            case 11:
                MeshFromPoints(sqare.topLeft, sqare.centerTop, sqare.centerRight, sqare.bottomRight, sqare.bottomLeft);
                break;
            case 13:
                MeshFromPoints(sqare.topLeft, sqare.topRight, sqare.centerRight, sqare.centerBottom, sqare.bottomLeft);
                break;
            case 14:
                MeshFromPoints(sqare.topLeft, sqare.topRight, sqare.bottomRight, sqare.centerBottom, sqare.centerLeft);
                break;
            
            case 15:
                MeshFromPoints(sqare.topLeft, sqare.topRight, sqare.bottomRight, sqare.bottomLeft);
                checkedVertices.Add(sqare.topLeft.vertexIndex);
                checkedVertices.Add(sqare.topRight.vertexIndex);
                checkedVertices.Add(sqare.bottomRight.vertexIndex);
                checkedVertices.Add(sqare.bottomLeft.vertexIndex);
                break;
        }
    }

    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; ++i)
        {
            if (points[i].vertexIndex == -1)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);
        
        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToDictionary(a.vertexIndex, triangle);
        AddTriangleToDictionary(b.vertexIndex, triangle);
        AddTriangleToDictionary(c.vertexIndex, triangle);
    }

    void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        if (triangleDictionary.ContainsKey(vertexIndexKey))
        {
            triangleDictionary[vertexIndexKey].Add(triangle);
        }
        else
        {
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            triangleDictionary.Add(vertexIndexKey, triangleList);
        }
    }

    void CalculateOutlines()
    {
        for (int vertexIndex = 0;  vertexIndex < vertices.Count; ++ vertexIndex)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if (newOutlineVertex != -1)
                {
                    checkedVertices.Add(vertexIndex);
                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);
                    FollowOutline(newOutlineVertex, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }

    void FollowOutline(int vertexIndex, int outlineIndex)
    {
        outlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);
        int nexVertexIndex = GetConnectedOutlineVertex(vertexIndex);
        if(nexVertexIndex != -1)
        {
            FollowOutline(nexVertexIndex, outlineIndex);
        }
    }
        
    int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];

        for (int i = 0; i < trianglesContainingVertex.Count; ++i)
        {
            Triangle triangle = trianglesContainingVertex[i];

            for (int j = 0; j < 3; ++j)
            {
                int vertexB = triangle[j];
                if (vertexB != vertexIndex)
                {
                    if (IsOutlineEdge(vertexIndex, vertexB) && !checkedVertices.Contains(vertexB))
                    {
                        return vertexB;
                    }
                }
            }
        }

        return -1;
    }

    bool IsOutlineEdge(int vertexA, int vertexB)
    {
        List<Triangle> trianglesWithVertexA = triangleDictionary[vertexA];
        int sharedTriangleCount = 0;

        for (int i = 0; i < trianglesWithVertexA.Count; ++i)
        {
            if (trianglesWithVertexA[i].Contains(vertexB))
            {
                sharedTriangleCount++;
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }

        return sharedTriangleCount == 1;
    }
    
    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);
        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);
        
    }

    struct Triangle
    {
        public int vertexIndexA;
        public int vertexIndexB;
        public int vertexIndexC;
        
        int[] vertices;
        
        public Triangle(int vertexIndexA, int vertexIndexB, int vertexIndexC)
        {
            this.vertexIndexA = vertexIndexA;
            this.vertexIndexB = vertexIndexB;
            this.vertexIndexC = vertexIndexC;

            vertices = new int[3];
            vertices[0] = vertexIndexA;
            vertices[1] = vertexIndexB;
            vertices[2] = vertexIndexC;
        }

        public int this[int i]
        {
            get
            {
                return vertices[i];
            }
        }
        
        public bool Contains(int vertexIndex)
        {
            return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
        }
    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;
            
            ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

            for (int x = 0; x < nodeCountX; ++x)
            {
                for (int y = 0; y < nodeCountY; ++y)
                {
                    Vector3 position = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode(position, map[x, y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; ++x)
            {
                for (int y = 0; y < nodeCountY - 1; ++y)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1],
                        controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }
        }
    }
    
    public class Square
    {
        public ControlNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centerTop, centerRight, centerBottom, centerLeft;
        public int configuration;
        
        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
        {
            this.topLeft = topLeft;
            this.topRight = topRight;
            this.bottomRight = bottomRight;
            this.bottomLeft = bottomLeft;
            
            centerTop = this.topLeft.right;
            centerRight = this.bottomRight.above;
            centerBottom = this.bottomLeft.right;
            centerLeft = this.bottomLeft.above;

            if (topLeft.active)
                configuration += 8;
            if (topRight.active)
                configuration += 4;
            if (bottomRight.active)
                configuration += 2;
            if (bottomLeft.active)
                configuration += 1;
        }
    }
    
    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 position)
        {
            this.position = position;
        }
    }
    
    public class ControlNode : Node
    {
        public bool active;
        public Node above, right;

        public ControlNode(Vector3 position, bool active, float squareSize) : base(position)
        {
            this.active = active;
            above = new Node(this.position + Vector3.forward* squareSize/2f);
            right = new Node(this.position + Vector3.right* squareSize/2f);
        }
    }
}
