using UnityEngine;
using Sirenix.OdinInspector;

public class HealthComponent : MonoBehaviour
{
    [FoldoutGroup("Health Settings")]
    [ProgressBar(0, "maxHealth", ColorGetter = "GetHealthBarColor")]
    [PropertyRange(0, "maxHealth")]
    public int currentHealth;

    [FoldoutGroup("Health Settings")]
    [MinValue(0)]
    public int maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private Color GetHealthBarColor()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        if (healthPercentage > 0.5f)
        {
            return Color.green;
        }
        else if (healthPercentage > 0.25f)
        {
            return Color.yellow;
        }
        else
        {
            return Color.red;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        HealthManager.RaiseHealthChanged(this.gameObject, currentHealth);

        if (currentHealth <= 0)
        {
            HealthManager.RaiseEntityDeath(this.gameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        HealthManager.RaiseHealthChanged(this.gameObject, currentHealth);
    }
}
