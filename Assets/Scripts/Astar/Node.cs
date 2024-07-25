using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // Node object
    public bool isWalkable;
    public bool isCrystal;
    public Vector3 worldPos;

    public int xGrid;
    public int yGrid;
    public int gCost;
    public int hCost;
    public Node parent;
    
    public Node(bool walkable, Vector3 worldPos, int x, int y)
    {
        isWalkable = walkable;
        this.worldPos = worldPos;
        xGrid = x;
        yGrid = y;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
