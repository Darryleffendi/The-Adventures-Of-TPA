using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimNode
{
    public bool visited;
    public bool transversed = false;
    public bool isObstacle = false;
    public List<PrimNode> ChildrenNodes { get; private set; }
    public PrimNode parent = null;
    public int xPos;
    public int yPos;
    public Vector2Int worldPoint;

    public PrimNode(Vector2Int worldPoint, int xPos, int yPos)
    {
        this.worldPoint = worldPoint;
        this.ChildrenNodes = new List<PrimNode>();
        this.xPos = xPos;
        this.yPos = yPos;
    }
    
    public void AddChildren(PrimNode node)
    {
        ChildrenNodes.Add(node);
    }
}
