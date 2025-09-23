using UnityEngine;

public class Node
{
    public Vector3 worldPosition;
    public bool isWalkable;
    public int gCost;
    public int hCost;
    public Node parentNode;
    public int gridX;
    public int gridZ;

    public int fCost => gCost + hCost;

    public Node(Vector3 worldPosition, bool isWalkable, int gridX, int gridZ)
    {
        this.worldPosition = worldPosition;
        this.isWalkable = isWalkable;
        this.gridX = gridX;
        this.gridZ = gridZ;
    }
}