using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject FlowerPrefab; // Prefab for the flower
    public GameObject HealthBarPrefab; // Prefab for the health bar
    public ObstacleSpawner obstacleSpawner; // Reference to the ObstacleSpawner
    public GridManager gridManager; // Reference to the GridManager

    [Header("Spawning Configuration")]
    public int flowerCount = 10; // Number of flowers to spawn  
    public float minimumDistance = 5f; // Minimum distance between flowers
    public bool hasSpawnedFlowers = false; // Flag to check if flowers have been spawned
    
    private bool startedSpawning = false; // Flag to check if spawning has started
    private List<GameObject> flowers = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
            if (gridManager == null)
            {
                Debug.LogError("GridManager not found in the scene.");
                return;
            }
        }
        if (obstacleSpawner == null)
        {
            obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
            if (obstacleSpawner == null)
            {
                Debug.LogError("ObstacleSpawner not found in the scene.");
                return;
            }
        }
        if (FlowerPrefab == null)
        {
            Debug.LogError("FlowerPrefab is not assigned in the inspector.");
            return;
        }
        if (HealthBarPrefab == null)
        {
            Debug.LogError("HealthBarPrefab is not assigned in the inspector.");
            return;
        }
    }

    void Update()
    {
        //Debug.Log("FlowerSpawner Update called.");
        //Debug.Log($"GridManager Ready: {gridManager.isGridReady}, ObstacleSpawner Done: {obstacleSpawner.isObstaclesDone}, Flowers Spawned: {hasSpawnedFlowers}");
        // Only run if GridManager and ObstacleSpawner are ready, and flowers haven't been spawned yet
        if (gridManager != null && gridManager.isGridReady &&
            obstacleSpawner != null && obstacleSpawner.isObstaclesDone &&
            !hasSpawnedFlowers && !startedSpawning)
        {
            startedSpawning = true; // Set flag to prevent multiple calls
            Debug.Log("Spawning flowers...");
            SpawnFlowersOnWalkableNodes();
            Debug.Log("Flowers spawned successfully.");
            hasSpawnedFlowers = true; // Set flag to true after spawning
        }
    }
    void SpawnFlowersOnWalkableNodes()
    {
        var flowerPositions = new List<Vector3>();
        int maxAttemptsPerFlower = 50; // Maximum attempts to find a walkable spot for a single flower

        for (int i = 0; i < flowerCount; i++)
        {
            Debug.Log($"Attempting to spawn flower {i + 1}/{flowerCount}");
            bool placed = false;
            for (int j = 0; j < maxAttemptsPerFlower; j++)
            {
                // Get random grid coordinates
                int randomX = Random.Range(0, gridManager.gridSizeX);
                int randomZ = Random.Range(0, gridManager.gridSizeZ);

                Node node = gridManager.GetNode(randomX, randomZ);

                // Check if the node is walkable and if it's far enough from existing flowers
                if (node != null && node.isWalkable)
                {
                    Vector3 potentialPosition = node.worldPosition;
                    // Adjust Y position for the flower's base (assuming flower prefab's pivot is at its base)
                    potentialPosition.y = 0.5f;
                    Debug.Log($"Checking position: {potentialPosition} for flower {i + 1}");

                    bool isFarEnough = true;
                    foreach (Vector3 pos in flowerPositions)
                    {
                        if (Vector3.Distance(pos, potentialPosition) < gridManager.nodeRadius * 2) // Use nodeDiameter or a similar measure
                        {
                            Debug.Log($"Position {potentialPosition} is too close to existing flower at {pos}");
                            isFarEnough = false;
                            break;
                        }
                    }

                    if (isFarEnough)
                    {
                        Debug.Log($"Placing flower at position: {potentialPosition}");
                        flowerPositions.Add(potentialPosition);
                        GameObject flowerInstance = Instantiate(FlowerPrefab, potentialPosition, Quaternion.identity);
                        flowers.Add(flowerInstance);
                        Debug.Log($"Flower spawned at position: {potentialPosition}");


                        // Instantiate and position health bar if needed (original logic had commented out health bar)
                        // GameObject healthBarInstance = Instantiate(HealthBarPrefab, flowerInstance.transform.position + Vector3.up * 1f, Quaternion.identity, flowerInstance.transform);
                        // Debug.Log($"Health bar positioned above flower at: {flowerInstance.transform.position}");
                        placed = true;
                        break; // Move to the next flower
                    }
                }
            }

            if (!placed)
            {
                Debug.LogWarning($"Could not find a suitable walkable spot for flower {i + 1} after {maxAttemptsPerFlower} attempts.");
            }
        }
    }
    public List<GameObject> GetSpawnedFlowers()
    {
        return flowers;
    }
}

        