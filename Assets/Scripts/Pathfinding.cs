using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    public Tilemap tilemap;
    public List<TileBase> walkableTiles;  // List of walkable tiles
    public int maxSteps = 500;  // Limit to avoid infinite loops

    private Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0),  // Right
        new Vector3Int(-1, 0, 0), // Left
        new Vector3Int(0, 1, 0),  // Up
        new Vector3Int(0, -1, 0)  // Down
    };

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        if (!IsWalkable(goal))
        {
            Debug.LogError("Goal is not walkable!");
            return null;
        }

        // Open list for nodes to explore
        Queue<Node> openList = new Queue<Node>();
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

        // Initialize the start node
        Node startNode = new Node(start);
        openList.Enqueue(startNode);
        allNodes[start] = startNode;

        int steps = 0;

        while (openList.Count > 0 && steps < maxSteps)
        {
            Node currentNode = openList.Dequeue();
            closedList.Add(currentNode.Position);

            // If we reached the goal, retrace the path
            if (currentNode.Position == goal)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighborPos = currentNode.Position + direction;

                // Skip if not walkable or already processed
                if (!IsWalkable(neighborPos) || closedList.Contains(neighborPos))
                    continue;

                if (!allNodes.ContainsKey(neighborPos))
                {
                    Node neighborNode = new Node(neighborPos)
                    {
                        Parent = currentNode
                    };
                    allNodes[neighborPos] = neighborNode;
                    openList.Enqueue(neighborNode);
                }
            }

            steps++;
        }

        Debug.LogWarning("No Path Found or Max Steps Reached");
        return null; // No path found
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

        path.Reverse(); // Reverse to get path from start to goal
        return path;
    }

    private bool IsWalkable(Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);
        return tile != null && walkableTiles.Contains(tile); // Ensure tile exists and is walkable
    }

    private class Node
    {
        public Vector3Int Position { get; }
        public Node Parent { get; set; }

        public Node(Vector3Int position)
        {
            Position = position;
        }
    }
}
