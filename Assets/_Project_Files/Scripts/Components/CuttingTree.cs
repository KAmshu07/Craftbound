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

        TREE_TYPE treeType = tree.GetTreeType();

        switch (treeType)
        {
            case TREE_TYPE.SMALL:
                if (tree is SmallTree smallTree)
                {
                    smallTree.CutTree(transform.position);
                }
                else
                {
                    Debug.LogError("Invalid tree type for cutting.");
                }
                break;

            case TREE_TYPE.MEDIUM:
                if (tree is MediumTree mediumTree)
                {
                    mediumTree.CutTree(transform.position);
                }
                else
                {
                    Debug.LogError("Invalid tree type for cutting.");
                }
                break;

            case TREE_TYPE.BIG:
                if (tree is BigTree bigTree)
                {
                    bigTree.CutTree(transform.position);
                }
                else
                {
                    Debug.LogError("Invalid tree type for cutting.");
                }
                break;

            default:
                Debug.LogError($"Unknown tree type: {treeType}");
                break;
        }

        gameObject.SetActive(false);
    }


    public void StartCutting(Tree tree)
    {
        healthComponent.TakeDamage(cutDamage);
    }

    public void Interact()
    {
        Debug.Log("Player has cutting the tree!");
        StartCutting(tree);
    }
}
