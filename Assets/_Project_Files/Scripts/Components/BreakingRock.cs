using UnityEngine;
using Sirenix.OdinInspector;

public class BreakingRock : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Required]
    [LabelWidth(150)]
    private HealthComponent healthComponent;

    [SerializeField]
    [LabelWidth(150)]
    public int breakDamage;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();

        if (healthComponent == null)
        {
            Debug.LogError("BreakingRock script requires a HealthComponent on the rock GameObject.");
            enabled = false;
        }
    }

    private void Start()
    {
        EventManager.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDestroy()
    {
        EventManager.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(GameObject entity, int newHealth)
    {
        if (entity == gameObject && newHealth <= 0)
        {
            BreakRock();
        }
    }

    private void BreakRock()
    {
        Debug.Log("Rock has been broken!");

        // Custom logic for what happens when a rock is broken.

        gameObject.SetActive(false);
    }

    public void StartBreaking()
    {
        healthComponent.TakeDamage(breakDamage);
    }

    public void Interact()
    {
        Debug.Log("Player has broken the rock!");
        StartBreaking();
    }
}
