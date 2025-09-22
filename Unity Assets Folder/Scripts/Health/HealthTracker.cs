using System;
using UnityEngine;

public class HealthTracker : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;

    // Change this from Action to Action<HealthTracker>
    public Action<HealthTracker> OnDestroyed;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}