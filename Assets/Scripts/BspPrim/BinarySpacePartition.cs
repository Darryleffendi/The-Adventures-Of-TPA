using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BinarySpacePartition
{
    BspNode rootNode;
    Queue<BspNode> graph;
    List<BspNode> resultList;

    public BinarySpacePartition(int worldWidth, int worldHeight)
    {
        this.rootNode = new BspNode(new Vector2Int(0, 0), new Vector2Int(worldWidth, worldHeight), 0, null);
    }

    public BspNode RootNode()
    {
        return rootNode;
    }

    public List<BspNode> PartitionSpace(int maxIter, int minWidth, int minHeight)
    {
        graph = new Queue<BspNode>();
        resultList = new List<BspNode>();

        graph.Enqueue(rootNode);
        resultList.Add(rootNode);

        for (int i = 0; i < maxIter && graph.Count > 0; i++)
        {
            BspNode currNode = graph.Dequeue();
            if (currNode.Width >= minWidth * 2 || currNode.Height >= minHeight)
            {
                SplitNode(currNode, minWidth, minHeight);
            }
        }

        resultList = ReduceRoom();
        return resultList;
    }

    private void SplitNode(BspNode currNode, int minWidth, int minHeight)
    {
        Line line = GetDividingLine(currNode, minWidth, minHeight);

        BspNode leftNode, rightNode;

        // If line is horizontal,
        // Divide currNode into top and bottom
        if (line.orientation == Orientation.Horizontal)
        {
            leftNode = new BspNode(currNode.bottomLeftCorner, 
                new Vector2Int(currNode.topRightCorner.x, line.coord.y),
                currNode.TreeLayerIndex + 1,
                currNode);
            rightNode = new BspNode(new Vector2Int(currNode.bottomLeftCorner.x, line.coord.y), 
                currNode.topRightCorner,
                currNode.TreeLayerIndex + 1,
                currNode);
        }
        // If line is vertical,
        // Divide currNode into left and right
        else
        {
            leftNode = new BspNode(currNode.bottomLeftCorner,
                new Vector2Int(line.coord.x, currNode.topRightCorner.y),
                currNode.TreeLayerIndex + 1,
                currNode);
            rightNode = new BspNode(new Vector2Int(line.coord.x, currNode.bottomLeftCorner.y),
                currNode.topRightCorner,
                currNode.TreeLayerIndex + 1,
                currNode);
        }

        graph.Enqueue(leftNode);
        resultList.Add(leftNode);
        graph.Enqueue(rightNode);
        resultList.Add(rightNode);
    }

    private Line GetDividingLine(BspNode currNode, int minWidth, int minHeight)
    {
        Vector2Int bottomLeftCorner = currNode.bottomLeftCorner;
        Vector2Int topRightCorner = currNode.topRightCorner;

        Orientation orientation;
        bool widthBool = (topRightCorner.x - bottomLeftCorner.x) >= 2 * minWidth;
        bool heightBool = (topRightCorner.y - bottomLeftCorner.y) >= 2 * minHeight;

        if (widthBool && heightBool)
        {
            orientation = (Orientation)(Random.Range(0, 2));
        }
        else if (widthBool)
        {
            orientation = Orientation.Vertical;
        }
        else
        {
            orientation = Orientation.Horizontal;
        }

        Vector2Int coord = Vector2Int.zero;

        if (orientation == Orientation.Horizontal)
        {
            coord = new Vector2Int(0, Random.Range(bottomLeftCorner.y + minHeight, topRightCorner.y - minHeight));
        }
        else
        {
            coord = new Vector2Int(Random.Range(bottomLeftCorner.x + minWidth, topRightCorner.x - minWidth), 0);
        }
        return new Line(orientation, coord);
    }

    public List<BspNode> ReduceRoom()
    {
        List<BspNode> roomList = new List<BspNode>();

        int i = 0;

        foreach (BspNode node in resultList)
        {
            if (!node.isLeaf) continue;

            int topBound = Random.Range(0, 6);
            int bottomBound = Random.Range(0, 6);
            int leftBound = Random.Range(0, 6);
            int rightBound = Random.Range(0, 6);

            node.bottomLeftCorner += new Vector2Int(leftBound, bottomBound);
            node.bottomRightCorner += new Vector2Int(-rightBound, bottomBound);
            node.topRightCorner += new Vector2Int(-rightBound, -topBound);
            node.topLeftCorner += new Vector2Int(leftBound, -topBound);

            if (i % 3 == 0)
                node.obstacleType = ObstacleType.Tree;
            else if (i % 3 == 1)
                node.obstacleType = ObstacleType.Rock;
            else
                node.obstacleType = ObstacleType.Tree;

            roomList.Add(node);
            i++;
        }
        return roomList;
    }
}
