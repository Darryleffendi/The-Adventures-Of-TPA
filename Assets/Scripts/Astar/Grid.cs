using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // 2D array of nodes
    Node[,] grid;

    [SerializeField]
    protected bool drawAStarGrid;
    [SerializeField]
    protected Vector2 worldSize;
    [SerializeField]
    protected float nodeRadius;
    protected float nodeDiameter;
    [SerializeField]
    protected LayerMask unwalkableLayer, playerLayer, enemyLayer, crystalLayer;
    public int gridAmountX;
    public int gridAmountY;
    private int gridHeight = 10;

    private List<Node> crystalNodes = new List<Node>();

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridAmountX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
        gridAmountY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
        InitializeGrid();
    }

    void InitializeGrid()
    {
        // Initialize 2D array of size [Xsize, Ysize]
        grid = new Node[gridAmountX, gridAmountY];
        Vector3 bottomLeft = transform.position - (Vector3.right * worldSize.x / 2) - (Vector3.forward * worldSize.y / 2);

        for (int i = 0; i < gridAmountY; i++)
        {
            for (int j = 0; j < gridAmountX; j++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (j * nodeDiameter + nodeRadius) + Vector3.forward * (i * nodeDiameter + nodeRadius);
                worldPoint.y = GetTerrainHeight(worldPoint);

                bool isWalkable = true;
                bool isCrystal = false;
                Vector3 layerPoint = worldPoint;

                // Check intersections at multiple layers
                for (int k = 0; k < 3; k++)
                {
                    isWalkable = !Physics.CheckSphere(layerPoint, nodeRadius, unwalkableLayer);
                    isCrystal = Physics.CheckSphere(layerPoint, nodeRadius, crystalLayer, QueryTriggerInteraction.Collide);
                    layerPoint.y = layerPoint.y + k * 3;
                    if (!isWalkable || isCrystal) break;
                }
                
                Node newNode = new Node(isWalkable, worldPoint, j, i);
                newNode.isCrystal = isCrystal;

                if (isCrystal)
                {
                    crystalNodes.Add(newNode);
                }

                grid[j, i] = newNode;
            }
        }
    }

    public Node NodeFromPos(Vector3 pos)    
    {
        float xPercent = Mathf.Clamp01((pos.x - transform.position.x + worldSize.x / 2) / worldSize.x);
        float yPercent = Mathf.Clamp01((pos.z - transform.position.z + worldSize.y / 2) / worldSize.y);
        
        int x = Mathf.FloorToInt(gridAmountX * xPercent);
        int y = Mathf.FloorToInt(gridAmountY * yPercent);

        return grid[x, y];
    }

    public Node GetCrystalNode(Vector3 pos)
    {
        int minDist = int.MaxValue;
        Node minNode = crystalNodes[0];

        foreach (Node node in crystalNodes)
        {
            int dist = Pathfinding.Heuristic(node, NodeFromPos(pos));
            if (dist < minDist)
            {
                minDist = dist;
                minNode = node;
            }
        }
        return minNode;
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // If currentNode, dont add to list
                if (i == 0 && j == 0) continue;

                int coorX = node.xGrid + j;
                int coorY = node.yGrid + i;

                // Make sure that node is inside of grid
                if (coorX >= 0 && coorX < gridAmountX && coorY >= 0 && coorY < gridAmountY)
                {
                    neighbors.Add(grid[coorX, coorY]);
                }
            }
        }
        return neighbors;
    }

    public float GetTerrainHeight(Vector3 worldPos)
    {
        float maxY = 0;
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            float curY = terrain.SampleHeight(worldPos);

            if (curY > maxY)
                maxY = curY;
        }
        return maxY;
    }

    // Gizmos function for testing purposes
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, gridHeight, worldSize.y));

        if (grid == null || !drawAStarGrid) return;

        foreach (Node node in grid)
        {
            if (node.isCrystal) Gizmos.color = Color.green;
            else if (node.isWalkable) Gizmos.color = Color.white;
            else Gizmos.color = Color.black;

            Gizmos.DrawCube(node.worldPos, Vector3.one * (nodeDiameter - .1f));
        }
        
    }
}
