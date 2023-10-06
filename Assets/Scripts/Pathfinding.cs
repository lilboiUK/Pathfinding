using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Sprite mazeSprite;
    public int startX, startY;
    public int targetX, targetY;

    int width, height;
    bool pathFound;

    Node currentNode;
    Node[,] nodeGrid;
    BinaryHeap<Node> openList;
    HashSet<Node> closedList;
    List<Node> path;

    void Start()
    {
        width = mazeSprite.texture.width;
        height = mazeSprite.texture.height;
        ResetMaze();

        nodeGrid = new Node[width, height];
        CreateGrid();

        openList = new BinaryHeap<Node>();
        closedList = new HashSet<Node>();

        openList.Insert(nodeGrid[startX, startY]);
        currentNode = null;
        pathFound = false;

        Stopwatch sw = Stopwatch.StartNew();
        while (!pathFound)
        {
            Iteration();
        }
        sw.Stop();
        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");

        DrawPath();
    }
    void DrawPath()
    {
        Draw(currentNode);
        mazeSprite.texture.Apply();
    }
    void Draw(Node node)
    {
        mazeSprite.texture.SetPixel(node.x, node.y, Color.blue);
        if (node.parent != null)
        {
            Draw(node.parent);
        }
    }
    void ResetMaze()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (mazeSprite.texture.GetPixel(x, y) != Color.black)
                {
                    mazeSprite.texture.SetPixel(x, y, Color.white);
                }
            }
        }
        mazeSprite.texture.Apply();
    }
    void CreateGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (mazeSprite.texture.GetPixel(x, y) == Color.black)
                {
                    nodeGrid[x, y] = new Node(x, y, false);
                }
                else
                {
                    nodeGrid[x, y] = new Node(x, y, true);
                }
            }
        }
    }
    void Iteration()
    {
        if (currentNode == null)
        {
            currentNode = nodeGrid[startX, startY];
        }
        else
        {
            currentNode = openList.ExtractMin();
        }
        closedList.Add(currentNode);

        if (currentNode.x == targetX && currentNode.y == targetY)
        {
            pathFound = true;
            return;
        }

        foreach (Node neighbourNode in GetNeighbours(currentNode))
        {
            if (!neighbourNode.walkable || closedList.Contains(neighbourNode))
            {
                continue;
            }
            else
            {
                if (openList.Contains(neighbourNode))
                {
                    if (currentNode.gCost + CalculateMovementCost(currentNode, neighbourNode) < neighbourNode.gCost) // Check if new path is shorter
                    {
                        neighbourNode.gCost = currentNode.gCost + CalculateMovementCost(currentNode, neighbourNode);
                        neighbourNode.hCost = CalculateHCost(neighbourNode, nodeGrid[targetX, targetY]);
                        neighbourNode.parent = currentNode;
                    }
                }
                else
                {
                    neighbourNode.gCost = currentNode.gCost + CalculateMovementCost(currentNode, neighbourNode);
                    neighbourNode.hCost = CalculateHCost(neighbourNode, nodeGrid[targetX, targetY]);
                    neighbourNode.parent = currentNode;
                    openList.Insert(neighbourNode);
                }
            }
        }
    }
    List<Node> GetNeighbours(Node currentNode)
    {
        List<Node> neighbours = new List<Node>();

        Node north = TryGetNode(currentNode.x, currentNode.y + 1);
        if (north != null) { neighbours.Add(north); }

        Node south = TryGetNode(currentNode.x, currentNode.y - 1);
        if (south != null) { neighbours.Add(south); }

        Node east = TryGetNode(currentNode.x + 1, currentNode.y);
        if (east != null) { neighbours.Add(east); }

        Node west = TryGetNode(currentNode.x - 1, currentNode.y);
        if (west != null) { neighbours.Add(west); }

        Node northEast = TryGetNode(currentNode.x + 1, currentNode.y + 1);
        if (northEast != null) { neighbours.Add(northEast); }

        Node northWest = TryGetNode(currentNode.x - 1, currentNode.y + 1);
        if (northWest != null) { neighbours.Add(northWest); }

        Node southEast = TryGetNode(currentNode.x + 1, currentNode.y - 1);
        if (southEast != null) { neighbours.Add(southEast); }

        Node southWest = TryGetNode(currentNode.x - 1, currentNode.y - 1);
        if (southWest != null) { neighbours.Add(southWest); }

        return neighbours;
    }
    Node TryGetNode(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) // Check if position is out of bounds
        {
            return null;
        }
        else
        {
            return nodeGrid[x, y];
        }
    }
    int CalculateMovementCost(Node current, Node neighbor)
    {
        int dx = Mathf.Abs(current.x - neighbor.x);
        int dy = Mathf.Abs(current.y - neighbor.y);

        int diagonalCost = 14;
        int straightCost = 10;

        return diagonalCost * Mathf.Min(dx, dy) + straightCost * (dx + dy);
    }
    int CalculateHCost(Node node, Node targetNode)
    {
        int dx = Mathf.Abs(node.x - targetNode.x);
        int dy = Mathf.Abs(node.y - targetNode.y);
        return 10 * (dx + dy);
    }
}

