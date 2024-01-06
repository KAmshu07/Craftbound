using UnityEngine;
using Sirenix.OdinInspector;

public class HealthComponent : MonoBehaviour
{
    [FoldoutGroup("Health Settings")]
    public int currentHealth;

    [FoldoutGroup("Health Settings")]
    public int maxHealth;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        EventManager.RaiseHealthChanged(this.gameObject, currentHealth);

        if (currentHealth <= 0)
        {
            EventManager.RaiseEntityDeath(this.gameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        EventManager.RaiseHealthChanged(this.gameObject, currentHealth);
    }
}
