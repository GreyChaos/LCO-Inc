using UnityEngine;

public class Node
{
    // Pathfinding stuff, this tells the A* how much a tile should cost to move on
    public Vector3Int Position;
    public float GCost;  // Cost to move from the start to this node
    public float HCost;  // Heuristic: estimated cost to the goal
    public float FCost { get { return GCost + HCost; } }
    public Node Parent;  // The parent node, used for retracing the path

    public Node(Vector3Int position)
    {
        Position = position;
    }
}
