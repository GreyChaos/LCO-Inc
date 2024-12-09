using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// I have no idea how this class works, I changed some things to make it faster, but after that I have no idea.
public class Pathfinding : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase walkableTile;  // Walkable tiles
    public TileBase obstacleTile;  // Obstacle tiles (blocks the player)
    public int maxSteps = 500;

    private Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0),  // Right
        new Vector3Int(-1, 0, 0), // Left
        new Vector3Int(0, 1, 0),  // Up
        new Vector3Int(0, -1, 0)  // Down
    };
public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
{
    // Priority Queue for open list
    SortedSet<Node> openList = new SortedSet<Node>(new NodeComparer());
    HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();
    Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    Node startNode = new Node(start);
    openList.Add(startNode);
    allNodes[start] = startNode;

    int steps = 0;
    while (openList.Count > 0 && steps < maxSteps)
    {
        Node currentNode = openList.Min;  // Get the node with the lowest F cost
        openList.Remove(currentNode);
        closedList.Add(currentNode.Position);

        // If we've reached the goal, retrace the path
        if (currentNode.Position == goal)
        {
            List<Vector3Int> path = RetracePath(startNode, currentNode);
            return path;
        }

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborPos = currentNode.Position + direction;

            // Skip if the neighbor is not walkable or already in closed list
            if (!IsWalkable(neighborPos) || closedList.Contains(neighborPos))
                continue;

            float tentativeGCost = currentNode.GCost + 1;  // Each move has cost of 1
            Node neighborNode;

            if (!allNodes.ContainsKey(neighborPos))
            {
                neighborNode = new Node(neighborPos);
                allNodes[neighborPos] = neighborNode;
            }
            else
            {
                neighborNode = allNodes[neighborPos];
            }

            if (!openList.Contains(neighborNode) || tentativeGCost < neighborNode.GCost)
            {
                neighborNode.GCost = tentativeGCost;
                neighborNode.HCost = Mathf.Abs(neighborPos.x - goal.x) + Mathf.Abs(neighborPos.y - goal.y);
                neighborNode.Parent = currentNode;

                if (!openList.Contains(neighborNode))
                    openList.Add(neighborNode);
            }
        }
        steps++;
    }

    Debug.Log("No Path Found");
    return null;  // No path found
}

class NodeComparer : IComparer<Node>
{
    public int Compare(Node x, Node y)
    {
        int fCostComparison = (x.GCost + x.HCost).CompareTo(y.GCost + y.HCost);
        if (fCostComparison == 0)
        {
            return x.GCost.CompareTo(y.GCost);
        }
        return fCostComparison;
    }
}


    

    private Node GetLowestFCostNode(List<Node> nodes)
    {
        Node lowest = nodes[0];
        foreach (Node node in nodes)
        {
            if (node.FCost < lowest.FCost)
                lowest = node;
        }
        return lowest;
    }

    private List<Vector3Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }

        path.Reverse();  // Reverse path to get it from start to goal
        return path;
    }

    private bool IsWalkable(Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);
        return tile == walkableTile;  // Only walkable tiles are valid
    }
}
