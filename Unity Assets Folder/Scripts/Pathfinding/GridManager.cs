using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class GridManager : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;
    public bool showGridGizmos = false;

    private Node[,] grid;
    private float nodeDiameter;
    public int gridSizeX, gridSizeZ;
    public bool isGridReady = false;
    public bool gridCenter = false;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        Debug.Log($"Grid Size: {gridSizeX}x{gridSizeZ}, Node Diameter: {nodeDiameter}");
        CreateGrid();
    }
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeZ];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius);
                bool isWalkable = !Physics.CheckBox(
                    worldPoint,
                    new Vector3(nodeDiameter / 2f, 10f, nodeDiameter / 2f),
                    Quaternion.identity,
                    unwalkableMask
                    );
                grid[x, z] = new Node(worldPoint, isWalkable, x, z);
            }
        }
        isGridReady = true;
    }
    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentZ = Mathf.Clamp01((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);
        return grid[x, z];
    }
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;

                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbors.Add(grid[checkX, checkZ]);
                }
            }
        }
        return neighbors;
    }
    public Node GetNode(int x, int z)
    {
        if (x >= 0 && x < gridSizeX && z >= 0 && z < gridSizeZ)
        {
            return grid[x, z];
        }
        return null;
    }
    public void SetNodeWalkability(int x, int z, bool isWalkable)
    {
        if (x >= 0 && x < gridSizeX && z >= 0 && z < gridSizeZ)
        {
            grid[x, z].isWalkable = isWalkable;
        }
    }
    public Vector3 GetAreaCenterWorld(int startX, int startZ, int width, int depth)
    {
        // Get the world position of the starting and ending nodes of the area
        Node startNode = GetNode(startX, startZ);
        Node endNode = GetNode(startX + width - 1, startZ + depth - 1);

        // Calculate the center point between them
        return (startNode.worldPosition + endNode.worldPosition) / 2f;
    }
    // Draw the grid in the editor for visualization. All white squares in edit mode, walkable in white and unwalkable in red in play mode, center marked with blue sphears.
    void OnDrawGizmos()
    {
        if (!showGridGizmos || grid == null) return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        float tempNodeDiameter = nodeRadius * 2;
        int tempGridSizeX = Mathf.RoundToInt(gridWorldSize.x / tempNodeDiameter);
        int tempGridSizeZ = Mathf.RoundToInt(gridWorldSize.y / tempNodeDiameter);
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        // In Play mode, use the actual grid and walkability
        if (Application.isPlaying && grid != null)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Node n = grid[x, z];
                    Gizmos.color = n.isWalkable ? new Color(1, 1, 1, 0.2f) : new Color(1, 0, 0, 0.2f);
                    Gizmos.DrawCube(n.worldPosition, new Vector3(tempNodeDiameter, tempNodeDiameter * 0.5f, tempNodeDiameter));
                if (gridCenter)
                    {
                        Gizmos.color = new Color(0, 0, 1);
                        Gizmos.DrawSphere(n.worldPosition, 0.1f);
                    }    
                }
            }
        }
        // In Edit mode, just draw a preview grid (all white)
        else
        {
            for (int x = 0; x < tempGridSizeX; x++)
            {
                for (int z = 0; z < tempGridSizeZ; z++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * tempNodeDiameter + nodeRadius) + Vector3.forward * (z * tempNodeDiameter + nodeRadius);
                    Gizmos.color = new Color(1, 1, 1, 0.2f);
                    Gizmos.DrawCube(worldPoint, new Vector3(tempNodeDiameter, tempNodeDiameter * 0.5f, tempNodeDiameter));
                }
            }
        }
    }
}