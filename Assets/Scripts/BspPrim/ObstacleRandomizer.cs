using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRandomizer : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    protected int worldWidth;
    [SerializeField]
    protected int worldHeight;

    [Header("BSP Fields")]
    [SerializeField]
    protected bool drawBspGrid = false;
    [SerializeField]
    protected int minRoomWidth, minRoomHeight, maxIter;

    [Header("Prim Fields")]
    [SerializeField]
    protected bool drawPrimGrid = false;
    [SerializeField]
    protected int nodeDiameter, nodeDistance;

    [Header("Prefabs")]
    [SerializeField]
    protected GameObject treePrefab;
    [SerializeField]
    protected GameObject rockPrefab;
    [SerializeField]
    protected Transform parentObject;

    BspNode rootNode = null;
    List<BspNode> bspTree = new List<BspNode>();

    PrimNode[,] primGraph;
    int graphAmountX;
    int graphAmountY;

    Vector3 topRight;

    void Awake()
    {
        topRight = transform.position + (Vector3.right * worldWidth / 2) + (Vector3.forward * worldHeight / 2);

        // Run BSP
        BinarySpacePartition bsp = new BinarySpacePartition(worldWidth, worldHeight);
        bspTree = bsp.PartitionSpace(maxIter, minRoomWidth, minRoomHeight);

        // Run Prim
        graphAmountX = Mathf.RoundToInt(worldWidth / (nodeDiameter + nodeDistance));
        graphAmountY = Mathf.RoundToInt(worldHeight / (nodeDiameter + nodeDistance));
        InitializePrimGrid();
        Prim prim = new Prim(primGraph, worldWidth, worldHeight, graphAmountX, graphAmountY);
        primGraph = prim.GenerateMST();

        // Generate Obstacles
        GenerateObstacles();
    }

    void InitializePrimGrid()
    {
        int radius = nodeDiameter / 2;
        primGraph = new PrimNode[graphAmountY, graphAmountX];
        for (int i = 0; i < graphAmountY; i ++)
        {
            for (int j = 0; j < graphAmountX; j ++)
            {
                primGraph[i, j] = new PrimNode(new Vector2Int(j * (nodeDistance + nodeDiameter) + radius, i * (nodeDistance + nodeDiameter) + radius), j, i);
            }
        }
    }

    void GenerateObstacles()
    {
        // Breadth-First Search Transversal
        Queue<PrimNode> queueFrontier = new Queue<PrimNode>();
        queueFrontier.Enqueue(primGraph[0, 0]);
        int stepCounter = 0;

        while (queueFrontier.Count > 0)
        {
            PrimNode curr = queueFrontier.Dequeue();
            if (curr.transversed) continue;
            
            // Generate obstacle every n steps
            if (stepCounter % 7 == 0)
            {
                curr.isObstacle = true;
                SpawnObstacle(curr.worldPoint);
            }

            foreach (PrimNode child in curr.ChildrenNodes)
            {
                queueFrontier.Enqueue(child);
            }
            curr.transversed = true;

            stepCounter++;
        }
    }

    void SpawnObstacle(Vector2Int currPos)
    {
        BspNode node = null;

        foreach (BspNode n in bspTree)
        {
            if (n.IsInBound(currPos))
            {
                node = n;
                break;
            }
        }

        if (node == null) return;

        Vector3 objPos = topRight - new Vector3(currPos.x, 0, currPos.y);
        objPos.y = GetTerrainHeight(objPos);

        float checkRadius = 1f;
        if (node.obstacleType == ObstacleType.Rock)
            checkRadius = 4f;

        bool isObstacle = Physics.CheckSphere(objPos, checkRadius, LayerMask.NameToLayer("Obstacle"));
        bool isPlatform = Physics.CheckSphere(objPos, checkRadius, LayerMask.NameToLayer("Stone"));
        bool isSpawnpoint = Physics.CheckSphere(objPos, 10, LayerMask.NameToLayer("Spawnpoint"));

        if (isObstacle || isPlatform || isSpawnpoint) return;

        if (node.obstacleType == ObstacleType.Tree)
            Instantiate(treePrefab, objPos, transform.rotation, parentObject);
        else if (node.obstacleType == ObstacleType.Rock)
            Instantiate(rockPrefab, objPos, transform.rotation, parentObject);
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
        Gizmos.DrawWireCube(transform.position, new Vector3(worldWidth, 5, worldHeight));

        // Draw BSP
        if (bspTree != null && drawBspGrid)
        {
            int i = 0;

            foreach (BspNode node in bspTree)
            {
                if (node.obstacleType == ObstacleType.Tree)
                    Gizmos.color = Color.black;
                else if (node.obstacleType == ObstacleType.Rock)
                    Gizmos.color = Color.gray;

                Gizmos.DrawCube(topRight - new Vector3(node.bottomLeftCorner.x + node.Width / 2, 0, node.bottomLeftCorner.y + node.Height / 2), new Vector3(node.Width, 5, node.Height));
                i++;
            }
        }

        // Draw Prim
        if (primGraph != null && drawPrimGrid)
        {
            foreach (PrimNode node in primGraph)
            {
                Gizmos.color = Color.blue;

                if (node.parent != null)
                {
                    for (float i = -0.2f; i < 0.3f; i += 0.08f)
                    {
                        Gizmos.DrawLine(topRight - new Vector3(node.parent.worldPoint.x + i, -8 + i, node.parent.worldPoint.y + i), topRight - new Vector3(node.worldPoint.x + i, -8 + i, node.worldPoint.y + i));
                    }
                }

                if (node.isObstacle)
                    Gizmos.color = Color.green;

                Gizmos.DrawCube(topRight - new Vector3(node.worldPoint.x, -8, node.worldPoint.y), new Vector3(nodeDiameter, nodeDiameter, nodeDiameter));
            
            }
        }
    }
}
