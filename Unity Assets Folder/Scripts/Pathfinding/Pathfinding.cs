using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    // --- Make sure you have the Singleton from the previous step ---
    public static Pathfinding Instance { get; private set; }

    public GridManager gridManager;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        if (gridManager == null) gridManager = GetComponent<GridManager>();
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = gridManager.GetNodeFromWorldPoint(startPos);
        Node targetNode = gridManager.GetNodeFromWorldPoint(endPos);

        // --- NEW: Add this safety check ---
        // This prevents errors if the enemy or flower is on an unwalkable node (like inside an obstacle).
        if (startNode == null || targetNode == null || !startNode.isWalkable || !targetNode.isWalkable)
        {
            Debug.LogWarning("Pathfinding request failed: Start or Target node is not valid or is unwalkable.");
            return null; // Return null immediately if the path is impossible from the start.
        }
        // --- End of new code ---


        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in gridManager.GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parentNode = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return null; // Path not found
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        path.Reverse();
        return path;
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstZ = Mathf.Abs(a.gridZ - b.gridZ);
        return dstX > dstZ ? 14 * dstZ + 10 * (dstX - dstZ) : 14 * dstX + 10 * (dstZ - dstX);
    }
}
