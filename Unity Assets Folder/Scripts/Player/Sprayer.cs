using UnityEngine;

public class sprayer : MonoBehaviour
{
    public float reachDistance = 10f;
    public float sprayRate = 1f;
    public float sprayDuration = 2f;
    public LayerMask targetLayer;

    // ADD THIS LINE to control the tilt from the Inspector
    public float downwardTilt = 0.1f;

    private float nextSprayTime = 0f;
    private ParticleSystem sprayEffect;

    private void Awake()
    {
        // Initialize the particle system for the spray effect
        sprayEffect = GetComponent<ParticleSystem>();
        if (sprayEffect == null)
        {
            Debug.LogError("No ParticleSystem component found on the sprayer.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Check if the player can spray
        if (Input.GetButton("Fire1") && Time.time >= nextSprayTime)
        {
            Spray();
        }
    }
    private void Spray()
    {
        if (sprayEffect == null) return;

        sprayEffect.Play();

        // Create the new tilted direction vector
        Vector3 sprayDirection = transform.forward - (transform.up * downwardTilt);

        // Use the new direction in both the Raycast and the Debug.DrawRay
        Debug.DrawRay(transform.position, sprayDirection * reachDistance, Color.green, 1f);
        if (Physics.Raycast(transform.position, sprayDirection, out RaycastHit hit, reachDistance, targetLayer))
        {
            nextSprayTime = Time.time + sprayRate;
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Sprayed target: " + hitObject.name);

            HealthTracker enemy = hitObject.GetComponent<HealthTracker>();
            if (enemy != null)
            {
                enemy.TakeDamage(10f);
                hitObject.GetComponentInChildren<HealthBar>()?.UpdateHealthBar(enemy.health, enemy.maxHealth);
            }
        }
    }
}
