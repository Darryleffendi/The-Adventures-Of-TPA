using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BspNode
{
    public List<BspNode> ChildrenNode { get; private set; }
    
    public bool visited;
    public Vector2Int bottomLeftCorner;
    public Vector2Int bottomRightCorner;
    public Vector2Int topLeftCorner;
    public Vector2Int topRightCorner;
    public BspNode parent;
    public int TreeLayerIndex;
    public bool isLeaf = true;
    public ObstacleType obstacleType;

    public BspNode(Vector2Int bottomLeftCorner, Vector2Int topRightCorner, int index, BspNode parentNode)
    {
        ChildrenNode = new List<BspNode>();
        parent = parentNode;

        this.bottomLeftCorner = bottomLeftCorner;
        this.topRightCorner = topRightCorner;
        this.TreeLayerIndex = index;
        bottomRightCorner = new Vector2Int(topRightCorner.x, bottomLeftCorner.y);
        topLeftCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);

        if (parentNode != null)
        {
            parentNode.AddChild(this);
            parentNode.isLeaf = false;
        }
    }

    public void AddChild(BspNode node)
    {
        ChildrenNode.Add(node);
    }

    public void RemoveChild(BspNode node)
    {
        ChildrenNode.Remove(node);
    }

    public bool IsInBound(Vector2Int pos)
    {
        if (pos.x >= bottomLeftCorner.x && pos.x <= topRightCorner.x)
        {
            if (pos.y >= bottomLeftCorner.y && pos.y <= topRightCorner.y)
            {
                return true;
            }
        }
        return false;
    }

    public int Width { get => (int)(topRightCorner.x - bottomLeftCorner.x); }
    public int Height { get => (int)(topRightCorner.y - bottomLeftCorner.y); }
}

public enum ObstacleType
{
    Tree = 0,
    Rock = 1
}