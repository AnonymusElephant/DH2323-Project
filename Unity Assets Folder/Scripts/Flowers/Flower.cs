using UnityEngine;

public class Flower : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the flower
    public float damage = 10f; // Damage taken by the flower (if needed)
    public float currentHealth;
    public GameObject healthBarPrefab; // Prefab for the health bar

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if (healthBarPrefab == null)
        {
            Debug.LogError("Health bar prefab is not assigned in the inspector.");
            return;
        }
        Debug.Log($"Instantiating health bar for flower: {gameObject.name}");
        GameObject healthBarInstance = Instantiate(healthBarPrefab);
        healthBarInstance.transform.SetParent(transform); // Set health bar as a child of the flower
        healthBarInstance.transform.localPosition = new Vector3(0, 0.25f, 0); // Position the health bar above the flower
        healthBarInstance.transform.localRotation = Quaternion.identity; // Reset rotation of the health bar
        Debug.Log($"Health bar instantiated for flower");
        currentHealth = maxHealth; // Initialize current health to maximum health
        //healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update the health bar with initial values    
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"{gameObject.name} is taking {damage} damage.");
        currentHealth -= damage; // Reduce current health by damage amount
        var healthBar = GetComponentInChildren<HealthBar>(); // Find the health bar component in children
        if (healthBar == null)
        {
            Debug.LogError("Health bar component not found in children.");
            return;
        }
        else
        {
            Debug.Log($"Current health: {currentHealth}, Max health: {maxHealth}");
            healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update the health bar with current health
        }
        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} has been destroyed due to damage.");
            Destroy(gameObject); // Destroy the flower if health is depleted
        }
        else
        {
            //healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update the health bar
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
