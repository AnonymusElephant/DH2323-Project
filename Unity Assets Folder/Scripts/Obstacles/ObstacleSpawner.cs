using UnityEngine;

// The ObstacleType struct must be in the same file, or another file,
// but it needs the [System.Serializable] attribute to be configured in the Inspector.
[System.Serializable]

public struct ObstacleType
{
    public GameObject prefab;
    public int width;  // Width in grid nodes (along X-axis)
    public int depth; // Depth in grid nodes (along Z-axis)
}

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;

    [Header("Spawning Configuration")]
    public ObstacleType[] obstacleTypes;
    public bool isObstaclesDone = false;
    [Tooltip("The total number of obstacles the spawner will attempt to place.")]
    public int numberOfObstaclesToSpawn = 15;
    [Tooltip("How many times to try finding a random spot for a single obstacle before giving up on it.")]
    public int maxPlacementAttempts = 20;

    // This flag ensures we only run the spawn logic once.
    private bool isSpawning = false;

    void Start()
    {
        // We still find the GridManager here, but we don't spawn yet.
        if (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
        }
    }

    // The spawning logic is now in Update to wait for the grid to be ready.
    void Update()
    {
        // Only run this if the grid is ready AND we haven't spawned yet.
        if (gridManager != null && gridManager.isGridReady && !isSpawning)
        {
            // Set the flag to true immediately to prevent this from running again.
            isSpawning = true;
            SpawnAllObstacles();
            isObstaclesDone = true;
        }
    }

    public void SpawnAllObstacles()
    {
        if (obstacleTypes == null || obstacleTypes.Length == 0)
        {
            Debug.LogError("No Obstacle Types are assigned to the spawner!");
            return;
        }

        int successfulSpawns = 0;
        for (int i = 0; i < numberOfObstaclesToSpawn; i++)
        {
            if (TrySpawnSingleObstacle())
            {
                successfulSpawns++;
            }
        }

        Debug.Log($"[ObstacleSpawner] Spawn run complete. Successfully placed {successfulSpawns} of {numberOfObstaclesToSpawn} requested obstacles.");
    }

    private bool TrySpawnSingleObstacle()
    {
        ObstacleType selectedObstacle = obstacleTypes[Random.Range(0, obstacleTypes.Length)];

        for (int i = 0; i < maxPlacementAttempts; i++)
        {
            // --- FIX: Decide rotation FIRST ---
            bool rotate = Random.value < 0.5f;

            // --- FIX: Determine effective dimensions based on the chosen rotation ---
            int effectiveWidth = rotate ? selectedObstacle.depth : selectedObstacle.width;
            int effectiveDepth = rotate ? selectedObstacle.width : selectedObstacle.depth;

            // Check if the obstacle can even fit on the grid with this rotation
            if (effectiveWidth > gridManager.gridSizeX || effectiveDepth > gridManager.gridSizeZ)
            {
                continue; // This obstacle is too large for the grid in this orientation, try another attempt.
            }

            // Find a random spot using the correct effective dimensions
            int randomX = Random.Range(0, gridManager.gridSizeX - effectiveWidth + 1);
            int randomZ = Random.Range(0, gridManager.gridSizeZ - effectiveDepth + 1);

            // Check placement with the effective dimensions
            if (CanPlaceObstacle(randomX, randomZ, effectiveWidth, effectiveDepth))
            {
                // If placement is valid, place the obstacle with the chosen rotation
                float yRotation = rotate ? 90f : 0f;
                PlaceObstacle(randomX, randomZ, selectedObstacle, yRotation, effectiveWidth, effectiveDepth);
                return true;
            }
        }

        Debug.LogWarning($"Failed to find a placement for {selectedObstacle.prefab.name} after {maxPlacementAttempts} attempts.");
        return false;
    }

    // --- FIX: This method now takes effective width and depth directly ---
    private bool CanPlaceObstacle(int startX, int startZ, int width, int depth)
    {
        Vector3 checkCenter = gridManager.GetAreaCenterWorld(startX, startZ, width, depth);
        checkCenter.y = 0;

        Vector3 checkHalfExtents = new Vector3(
            width * gridManager.nodeRadius,
            0.1f,
            depth * gridManager.nodeRadius
        );

        Collider[] hitColliders = Physics.OverlapBox(checkCenter, checkHalfExtents, Quaternion.identity, gridManager.unwalkableMask);

        return hitColliders.Length == 0;
    }

    // --- FIX: This method now takes the chosen rotation and effective dimensions ---
    private void PlaceObstacle(int startX, int startZ, ObstacleType obstacle, float yRotation, int effectiveWidth, int effectiveDepth)
    {
        // Use effective dimensions to find the center
        Vector3 spawnPosition = gridManager.GetAreaCenterWorld(startX, startZ, effectiveWidth, effectiveDepth);
        float prefabHeight = obstacle.prefab.GetComponent<Collider>().bounds.size.y;

        spawnPosition.y = prefabHeight / 2f;

        // Use the pre-determined rotation
        Quaternion spawnRotation = Quaternion.Euler(0, yRotation, 0);

        Instantiate(obstacle.prefab, spawnPosition, spawnRotation);

        // Use effective dimensions to mark the correct grid nodes as unwalkable
        for (int x = 0; x < effectiveWidth; x++)
        {
            for (int z = 0; z < effectiveDepth; z++)
            {
                gridManager.SetNodeWalkability(startX + x, startZ + z, false);
            }
        }
    }
}
