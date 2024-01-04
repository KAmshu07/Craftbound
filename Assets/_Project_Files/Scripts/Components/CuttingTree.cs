using UnityEngine;

public class CuttingTree : MonoBehaviour
{
    private HealthComponent healthComponent;

    [SerializeField] private int cutDamage = 20; // Damage required to cut the tree

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();

        if (healthComponent == null)
        {
            Debug.LogError("TreeCuttable script requires a HealthComponent on the tree GameObject.");
            enabled = false;
        }
    }

    private void Start()
    {
        // Subscribe to health changed event
        EventManager.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to avoid memory leaks
        EventManager.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(GameObject entity, int newHealth)
    {
        // Check if the health change is for this tree
        if (entity == gameObject)
        {
            // Check if the tree's health has reached or fallen below 0
            if (newHealth <= 0)
            {
                CutTree();
            }
        }
    }

    private void CutTree()
    {
        // Add tree cutting logic here
        Debug.Log("Tree has been cut!");
        // Optionally, you can disable the tree GameObject or play an animation
        gameObject.SetActive(false);
    }

    // Method to initiate cutting the tree
    public void StartCutting()
    {
        // Inflict damage to the tree when cutting is initiated
        healthComponent.TakeDamage(cutDamage);
    }
}
