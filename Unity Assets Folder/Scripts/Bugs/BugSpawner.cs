using UnityEngine;
using System.Collections.Generic;

public class BugSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject bugPrefab;
    public LayerMask groundLayer;
    public FlowerSpawner flowerSpawner;
    public GridManager gridManager;
    public Transform playerTransform;

    [Header("Spawning Rules")]
    public float minDistanceToFlowers = 10f;
    public float minDistanceToPlayer = 5f;
    public int maxBugs = 10;
    public float spawnDelay = 5f;

    private List<GameObject> spawnedBugs = new List<GameObject>();
    private float spawnTimer = 0f;
    private bool hasStartedBugSpawning = false;

    private void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in the inspector for BugSpawner.");
            enabled = false;
            return;
        }

        if (flowerSpawner == null)
        {
            flowerSpawner = FindFirstObjectByType<FlowerSpawner>();
            if (flowerSpawner == null)
            {
                Debug.LogError("FlowerSpawner not found in the scene. Please assign it or ensure one exists.");
                enabled = false;
                return;
            }
        }
        if (gridManager == null)
        {
            gridManager = FindFirstObjectByType<GridManager>();
            if (gridManager == null)
            {
                Debug.LogError("GridManager not found in the scene. Please assign it or ensure one exists.");
                enabled = false;
                return;
            }
        }
    }

    private void Update()
    {
        if (!hasStartedBugSpawning && flowerSpawner != null && flowerSpawner.hasSpawnedFlowers && gridManager != null && gridManager.isGridReady)
        {
            Debug.Log("[BugSpawner] Flower spawning complete and Grid is ready. Initializing bug spawning.");
            hasStartedBugSpawning = true;
            spawnTimer = spawnDelay;
        }

        if (!hasStartedBugSpawning) return;

        if (!CanSpawn()) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnDelay)
        {
            TrySpawnBug();
        }

        CleanupDestroyedBugs();
    }

    private bool CanSpawn()
    {
        return playerTransform != null && bugPrefab != null && spawnedBugs.Count < maxBugs;
    }

    private void TrySpawnBug()
    {
        Debug.Log("Attempting to spawn a bug...");
        Vector3 spawnPos;
        int attempts = 0;
        int maxSpawnAttempts = 50;

        while (attempts < maxSpawnAttempts)
        {
            if (!TryGetRandomWalkableGridPosition(out spawnPos))
            {
                attempts++;
                continue;
            }

            // Use the new public variable minDistanceToPlayer
            if (Vector3.Distance(playerTransform.position, spawnPos) <= minDistanceToPlayer)
            {
                attempts++;
                continue;
            }

            if (!IsFarEnoughFromFlowers(spawnPos))
            {
                attempts++;
                continue;
            }

            SpawnBugAt(spawnPos);
            spawnTimer = 0f;
            return;
        }
        Debug.LogWarning($"Failed to spawn a bug after {maxSpawnAttempts} attempts. No suitable position found on the grid.");
    }

    private bool TryGetRandomWalkableGridPosition(out Vector3 walkableWorldPosition)
    {
        int maxGridSearchAttempts = 100;
        for (int i = 0; i < maxGridSearchAttempts; i++)
        {
            int randomX = Random.Range(0, gridManager.gridSizeX);
            int randomZ = Random.Range(0, gridManager.gridSizeZ);

            Node node = gridManager.GetNode(randomX, randomZ);
            if (node != null && node.isWalkable)
            {
                walkableWorldPosition = node.worldPosition;
                walkableWorldPosition.y = 0.6f;
                return true;
            }
        }
        walkableWorldPosition = Vector3.zero;
        return false;
    }

    private bool IsFarEnoughFromFlowers(Vector3 bugPotentialPos)
    {
        if (flowerSpawner == null || flowerSpawner.GetSpawnedFlowers() == null)
        {
            return true;
        }

        foreach (GameObject flower in flowerSpawner.GetSpawnedFlowers())
        {
            if (flower != null && Vector3.Distance(bugPotentialPos, flower.transform.position) < minDistanceToFlowers)
            {
                Debug.Log($"Bug potential spawn position {bugPotentialPos} is too close to flower at {flower.transform.position}");
                return false;
            }
        }
        return true;
    }

    private void SpawnBugAt(Vector3 position)
    {
        GameObject newBugObject = Instantiate(bugPrefab, position, Quaternion.identity);
        HealthTracker newBugHealth = newBugObject.GetComponent<HealthTracker>();
        spawnedBugs.Add(newBugObject);
        Debug.Log("Spawned a new bug at position: " + position);
        if (newBugHealth != null)
        {
            // Assuming HealthTracker has an OnDestroyed event: public event System.Action<HealthTracker> OnDestroyed;
            // newBugHealth.OnDestroyed += OnBugDestroyed; 
        }
    }

    private void OnBugDestroyed(HealthTracker bugThatDied)
    {
        Debug.Log("A bug was destroyed: " + bugThatDied.name);
        if (bugThatDied != null)
        {
            spawnedBugs.Remove(bugThatDied.gameObject);
        }
    }

    private void CleanupDestroyedBugs()
    {
        spawnedBugs.RemoveAll(b => b == null);
    }
}