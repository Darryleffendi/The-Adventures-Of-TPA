using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prim
{
    PrimNode startNode;
    Stack<PrimNode> primStack;
    PrimNode[,] nodeList;
    int worldWidth;
    int worldHeight;
    int graphAmountX;
    int graphAmountY;

    public Prim(PrimNode[,] nodeList, int worldWidth, int worldHeight, int graphAmountX, int graphAmountY)
    {
        this.startNode = nodeList[0,0];
        this.nodeList = nodeList;
        this.worldHeight = worldHeight;
        this.worldWidth = worldWidth;
        this.graphAmountX = graphAmountX;
        this.graphAmountY = graphAmountY;
        primStack = new Stack<PrimNode>();
    }

    public PrimNode[,] GenerateMST()
    {
        primStack.Push(startNode);
        PrimNode prevNode = null;

        while (primStack.Count > 0)
        {
            PrimNode curr = primStack.Pop();
            curr.visited = true;

            if (curr.parent != null)
            {
                curr.parent.AddChildren(curr);
            }
            else if (prevNode != null)
            {
                curr.parent = prevNode;
                prevNode.AddChildren(curr);
            }

            List<PrimNode> neighbors = GetNeighborNode(curr);
            
            foreach (PrimNode node in neighbors)
            {
                node.parent = curr;
                primStack.Push(node);
            }

            prevNode = curr;
        }
        return nodeList;
    }

    List<PrimNode> GetNeighborNode(PrimNode curr)
    {
        /*
         *   0 
         * 3 - 1
         *   2 
         */

        List<PrimNode> neighbors = new List<PrimNode>();

        if (curr.yPos > 0) // Case 0
        {
            PrimNode node = nodeList[curr.yPos - 1, curr.xPos];
            if (!node.visited) neighbors.Add(node);
        }
        if (curr.xPos < graphAmountX - 1)  // Case 1
        {
            PrimNode node = nodeList[curr.yPos, curr.xPos + 1];
            if (!node.visited) neighbors.Add(node);
        }
        if (curr.yPos < graphAmountY - 1)  // Case 2
        {
            PrimNode node = nodeList[curr.yPos + 1, curr.xPos];
            if (!node.visited) neighbors.Add(node);
        }
        if (curr.xPos > 0)  // Case 3
        {
            PrimNode node = nodeList[curr.yPos, curr.xPos - 1];
            if (!node.visited) neighbors.Add(node);
        }
        return Randomize(neighbors);
    }

    private List<PrimNode> Randomize(List<PrimNode> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            var k = Random.Range(0, i + 1);
            var value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }
}
