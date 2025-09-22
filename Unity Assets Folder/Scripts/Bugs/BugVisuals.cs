using UnityEngine;

public class BugVisuals : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject[] assetsToSpawn;
    public GameObject healthBarPrefab; // Prefab for the health bar
    void Start()
    {
        SpawnVisualAsset();
    }

    private void SpawnVisualAsset()
    {
        if(assetsToSpawn == null || assetsToSpawn.Length == 0)
        {
            Debug.LogWarning("No assets to spawn assigned in BugVisuals.");
            return;
        }
        int randomIndex = Random.Range(0, assetsToSpawn.Length);
        GameObject asset = assetsToSpawn[randomIndex];
        if (asset != null)
        {
            Instantiate(asset, transform.position, transform.rotation, transform);
            GameObject healthBarInstance = Instantiate(healthBarPrefab);
            healthBarInstance.transform.SetParent(transform); // Set health bar as a child of the flower
            healthBarInstance.transform.localPosition = new Vector3(0, 4f, 0); // Position the health bar above the flower
            healthBarInstance.transform.localRotation = Quaternion.identity; // Reset rotation of the health bar
                                                                             // Change color of health bar to red
            var healthBar = healthBarInstance.GetComponent<HealthBar>();
            if (healthBar != null)
            {
                healthBar.SetColor(Color.red); // Set the color of the health bar to red
            }
            else
            {
                Debug.LogError("HealthBar component not found on the instantiated health bar prefab.");
            }
            Debug.Log($"Health bar instantiated for bug");
        }
    }
}
