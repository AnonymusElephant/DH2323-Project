using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class BugAI : MonoBehaviour
{
    public float eatRate = 2f; // How often the bug eats a flower
    public float nextEatTime = 0f; // Time until the next eat action
    public float eatDamage = 5f; // Amount of damage the bug does to the flower when eating
    private Transform targetFlower;
    private PathFollower pathFollower;
    private bool isAtFlower = false;

    void Awake()
    {
        // Get the required component on the same GameObject
        pathFollower = GetComponent<PathFollower>();
    }

    private void Start()
    {
        pathFollower.OnPathCompleted += OnReachedFlower;
        StartCoroutine(WaitForGridAndFindPath());
    }

    private void Update()
    {
        if (targetFlower == null)
        {
            FindClosestFlower();
        }
        if (isAtFlower && Time.time >= nextEatTime) {
            EatFlower();
            nextEatTime = Time.time + eatRate; // Set the next eat time
        }
    }
    void OnDestroy()
    {
        if (pathFollower != null)
        {
            pathFollower.OnPathCompleted -= OnReachedFlower;
        }
    }

    IEnumerator WaitForGridAndFindPath()
    {
        // This loop waits until the Pathfinding instance and its grid are fully initialized.
        while (Pathfinding.Instance == null || Pathfinding.Instance.gridManager == null || !Pathfinding.Instance.gridManager.isGridReady)
        {
            yield return null; // Wait for the next frame before checking again.
        }
        Debug.Log($"<color=white>{name}: Grid is ready. Looking for a flower.</color>");
        FindClosestFlower();
    }

    void FindClosestFlower()
    {
         isAtFlower = false; // Reset the flag to indicate we are not at a flower yet
        // We find all GameObjects with the built-in "Flower" tag.
        GameObject[] allFlowers = GameObject.FindGameObjectsWithTag("Flower");

        if (allFlowers.Length == 0)
        {
            Debug.LogError($"<color=red>{name}: CRITICAL - No GameObjects with the 'Flower' tag found. The enemy has no target and will do nothing.</color>");
            return;
        }

        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        // Iterate over the found GameObjects to find the closest one.
        foreach (GameObject flower in allFlowers)
        {
            float dist = Vector3.Distance(transform.position, flower.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                bestTarget = flower.transform;
            }
        }

        targetFlower = bestTarget;
        Debug.Log($"<color=yellow>{name}: Found closest flower: {targetFlower.name}. Requesting path...</color>");

        // Request a path from the Pathfinding singleton.
        List<Node> path = Pathfinding.Instance.FindPath(transform.position, targetFlower.position);

        if (path != null && path.Count > 0)
        {
            // Tell the PathFollower component to use the new path
            pathFollower.SetPath(path);
        }
        else
        {
            Debug.LogWarning($"<color=red>{name}: Path failed.</color>");
        }
    }

    private void OnReachedFlower()
    {
        Debug.Log($"<color=green>{name}: Reached the flower: {targetFlower.name}.</color>");
        isAtFlower = true; // Set the flag to indicate we are at the flower
        nextEatTime = Time.time + eatRate; // Reset the next eat time
    }
    private void EatFlower()
    {
       
        if (targetFlower != null)
        {
            Debug.Log($"<color=green>{name} is eating {targetFlower.name}. Om nom nom...</color>");
            targetFlower.GetComponent<Flower>()?.TakeDamage(eatDamage);
        }
    }
}
