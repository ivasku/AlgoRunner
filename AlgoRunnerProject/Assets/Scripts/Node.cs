using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum NodeType
{
    Open = 0,
    Blocked = 1
}

public class Node: IComparable<Node>
{

    public NodeType nodeType = NodeType.Open;
    public int xIndex = -1;
    public int yIndex = -1;

    public Vector3 position;

    public List<Node> neighbours = new List<Node>();
    public Node previous = null;

    public float distanceTraveled = Mathf.Infinity;

    public float priority;

    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;

    }

    public int CompareTo(Node other)
    {
        if (this.priority < other.priority)
        {
            return -1;
        }
        else if (this.priority > other.priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public void Reset()
    {
        previous = null;
    }


}
