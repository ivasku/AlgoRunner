using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
  
    public Node[,] nodes;
    public List<Node> walls = new List<Node>();

    private int[,] m_mapData;
    private int m_width;
    private int m_height;

    public int Width
    {
        get {
            return m_width;
        }
    }

    public int Height
    {
        get
        {
            return m_height;
        }
    }

    public static readonly Vector2[] allDirections =
    {
        new Vector2(0f,1f),
        new Vector2(1f,1f),
        new Vector2(1f,0f),
        new Vector2(1f,-1f),
        new Vector2(0f,-1f),
        new Vector2(-1f,-1f),
        new Vector2(-1f,0f),
        new Vector2(-1f,1f)
    };

    public void Init(int[,] mapData)
    {
        m_mapData = mapData;
        m_width = mapData.GetLength(0);
        m_height = mapData.GetLength(1);
        nodes = new Node[m_width, m_height];

        for(int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                NodeType type = (NodeType)mapData[j, i];
                Node newNode = new Node(j, i, type);
                nodes[j, i] = newNode;

                newNode.position = new Vector3(j, 0, i);

                if (type == NodeType.Blocked)
                {
                    walls.Add(newNode);
                }
            }
        }


        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {

                if (nodes[j,i].nodeType != NodeType.Blocked)
                {
                    nodes[j, i].neighbours = GetNeighbours(j,i);
                }

            }
        }


    }

    public bool IsWithingBounds(int x, int y)
    {
        return (x >= 0 && x < m_width && y >= 0 && y < m_height);
    }

    private List<Node> GetNeighbours(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighbourNodes = new List<Node>();

        foreach (Vector2 dir in directions)
        {
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;


            if (IsWithingBounds(newX, newY) && nodeArray[newX, newY] != null && nodeArray[newX, newY].nodeType != NodeType.Blocked)
            {
                neighbourNodes.Add(nodeArray[newX, newY]);
            }

        }

        return neighbourNodes;

    }

    private List<Node> GetNeighbours(int x, int y)
    {
        return GetNeighbours(x, y, nodes, allDirections);
    }

    public float GetNodeDistance(Node source, Node target)
    {
        int dx = Mathf.Abs(source.xIndex - target.xIndex);
        int dy = Mathf.Abs(source.yIndex - target.yIndex);

        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return (1.4f * diagonalSteps + straightSteps);
    }

    public float GetNodeManhattanDistance(Node source, Node target)
    {
        int dx = Mathf.Abs(source.xIndex - target.xIndex);
        int dy = Mathf.Abs(source.yIndex - target.yIndex);

        return (dx + dy);
    }

}
