using UnityEngine;
using Sirenix.OdinInspector;

public class MiningRock : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Required]
    [LabelWidth(150)]
    private HealthComponent healthComponent;

    [SerializeField]
    [LabelWidth(150)]
    public int breakDamage;

    public Rock rock;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();

        if (healthComponent == null)
        {
            Debug.LogError("MiningRock script requires a HealthComponent on the rock GameObject.");
            enabled = false;
        }
    }

    private void Start()
    {
        HealthManager.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDestroy()
    {
        HealthManager.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(GameObject entity, int newHealth)
    {
        if (entity == gameObject && newHealth <= 0)
        {
            BreakRock(rock);
        }
    }

    private void BreakRock(Rock rock)
    {
        if (rock == null)
        {
            Debug.LogError("Rock is not assigned.");
            return;
        }

        // Call the MineRock method on the rock
        rock.MineRock(transform.position);

        // Deactivate the rock GameObject
        gameObject.SetActive(false);
    }

    public void StartBreaking(Rock rock)
    {
        healthComponent.TakeDamage(breakDamage);
    }

    public void Interact()
    {
        Debug.Log("Player has broken the rock!");
        StartBreaking(rock);
    }
}
