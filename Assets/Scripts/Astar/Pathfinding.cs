using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/* Study References
 * https://www.youtube.com/watch?v=D5aJNFWsWew&list=PLBw9d_OueVJS_084gYQexJ38LC2LEhpR4&index=1&ab_channel=CS50
 * https://youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW
 */

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    protected Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartPathfinding(Vector3 startPos, Vector3 targetPos, bool findCrystal = false)
    {
        StartCoroutine(FindPath(startPos, targetPos, findCrystal));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, bool findCrystal)
    {
        Node startNode = grid.NodeFromPos(startPos);
        Node targetNode = grid.NodeFromPos(targetPos);
        if (findCrystal)
            targetNode = grid.GetCrystalNode(startPos);

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        // Set containing nodes to be evaluated
        Heap frontierSet = new Heap(grid.gridAmountX * grid.gridAmountY + 1);
        if(startNode != null) frontierSet.Insert(startNode);
        // Set containing nodes that has been evaluated
        HashSet<Node> exploredSet = new HashSet<Node>();

        while (frontierSet.Size > 0)
        {
            // Get smallest value from heap
            Node curr = frontierSet.GetMin();
            exploredSet.Add(curr);

            // If node is found, proceed to backtracing
            if (curr == targetNode)
            {
                pathSuccess = true;
                break;
            }

            // Iterate over neighbors
            foreach (Node neighbor in grid.GetNeighbors(curr))
            {
                // If not walkable or if exists in closed set, do not proceed
                if (!neighbor.isWalkable || exploredSet.Contains(neighbor) || (!findCrystal && neighbor.isCrystal))
                {
                    if(neighbor != targetNode)
                        continue;
                }

                int costToNeighbor = curr.gCost + Heuristic(curr, neighbor);
                bool inFrontier = frontierSet.Contains(neighbor);
                
                if (!inFrontier || costToNeighbor < neighbor.gCost)
                {
                    neighbor.gCost = costToNeighbor;
                    neighbor.hCost = Heuristic(neighbor, targetNode);
                    neighbor.parent = curr;

                    if (!inFrontier) frontierSet.Insert(neighbor);
                }
            }

        }

        yield return null;
        if (pathSuccess)
        {
            waypoints = BackTrace(startNode, targetNode);
        }
        requestManager.FinishPathProcessing(waypoints, pathSuccess);

    }

    Vector3[] BackTrace(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node curr = targetNode;

        while (curr != startNode)
        {
            path.Add(curr);
            curr = curr.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].xGrid - path[i].xGrid, path[i - 1].yGrid - path[i].yGrid);

            if (newDirection != oldDirection)
            {
                waypoints.Add(path[i].worldPos);
            }
            oldDirection = newDirection;    
        }
        return waypoints.ToArray();
    }

    // Function to calculate distance approximation h(n)
    public static int Heuristic(Node a, Node b)
    {
        int xDist = Mathf.Abs(a.xGrid - b.xGrid);
        int yDist = Mathf.Abs(a.yGrid - b.yGrid);

        if (xDist > yDist)
        {
            return 14 * yDist + 10 * (xDist - yDist);
        }
        return 14 * xDist + 10 * (yDist - xDist);
    }
}