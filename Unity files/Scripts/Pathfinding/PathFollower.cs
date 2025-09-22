using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public float speed = 5f;
    public float stoppingDistance = 0.5f;
    
    public Action OnPathCompleted;

    private List<Node> path;
    private int pathIndex;
    private Coroutine movementCoroutine;

    public void SetPath(List<Node> newPath)
    {
        if (newPath == null || newPath.Count == 0) return;

        path = newPath;
        if (movementCoroutine != null) StopCoroutine(movementCoroutine);
        movementCoroutine = StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        Debug.Log($"<color=lime>{name}: Coroutine 'FollowPath' has started.</color>");
        pathIndex = 0;
        Vector3 currentWaypoint = path[pathIndex].worldPosition;

        while (true)
        {
            // Check if we are close enough to the current waypoint.
            // Create "flat" versions of the positions to check the horizontal distance
            Vector3 currentPos2D = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 waypointPos2D = new Vector3(currentWaypoint.x, 0f, currentWaypoint.z);

            // Check if we are horizontally close enough to the current waypoint.
            if (Vector3.Distance(currentPos2D, waypointPos2D) < stoppingDistance)
            {
                pathIndex++;
                // Check if we have reached the end of the path.
                if (pathIndex >= path.Count)
                {
                    Debug.Log($"<color=green>{name}: Reached the end of the path.</color>");
                    OnPathCompleted?.Invoke();
                    yield break; // Exit the coroutine.
                }
                // Set the next waypoint.
                currentWaypoint = path[pathIndex].worldPosition;
            }
            Vector3 targetPosition = new Vector3(currentWaypoint.x, transform.position.y, currentWaypoint.z);
            // Move towards the current waypoint.
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Wait for the next frame.
        }
    }

    void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = Color.cyan;
            for (int i = pathIndex; i < path.Count; i++)
            {
                Gizmos.DrawCube(path[i].worldPosition, Vector3.one * 0.3f); // Make cubes bigger
                if (i > 0)
                {
                    Gizmos.DrawLine(path[i - 1].worldPosition, path[i].worldPosition);
                }
            }
        }
    }
}
