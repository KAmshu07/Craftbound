using UnityEngine;
using Sirenix.OdinInspector;

public class CuttingTree : MonoBehaviour, IInteractable
{
    [SerializeField]
    [Required]
    [LabelWidth(150)]
    private HealthComponent healthComponent;

    [SerializeField]
    [LabelWidth(150)]
    public int cutDamage;

    [SerializeField] public Tree tree;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();

        if (healthComponent == null)
        {
            Debug.LogError("CuttingTree script requires a HealthComponent on the tree GameObject.");
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
            CutTree(tree);
        }
    }

    private void CutTree(Tree tree)
    {
        if (tree == null)
        {
            Debug.LogError("Tree is not assigned.");
            return;
        }

        // Call the CutTree method on the tree
        tree.CutTree(transform.position);

        // Deactivate the tree GameObject
        gameObject.SetActive(false);
    }

    public void StartCutting(Tree tree)
    {
        healthComponent.TakeDamage(cutDamage);
    }

    public void Interact()
    {
        Debug.Log("Player has cut the tree!");
        StartCutting(tree);
    }
}
