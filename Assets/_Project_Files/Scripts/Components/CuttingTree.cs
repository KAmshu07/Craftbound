using UnityEngine;
using Sirenix.OdinInspector;

public class CuttingTree : MonoBehaviour
{
    [SerializeField]
    [Required]
    [LabelWidth(150)]
    private HealthComponent healthComponent;

    [SerializeField]
    [LabelWidth(150)]
    public int cutDamage;

    private TreeBase treeType;

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
            CutTree();
        }
    }

    [Button("Cut Tree", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    private void CutTree()
    {
        Debug.Log("Tree has been cut!");

        if (treeType != null)
        {
            treeType.CutTree(transform.position);
        }

        gameObject.SetActive(false);
    }

    [Button("Start Cutting", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public void StartCutting()
    {
        healthComponent.TakeDamage(cutDamage);
    }

    [FoldoutGroup("Collision Events")]
    [BoxGroup("Collision Events/Player Collision")]
    [Button("Player Collision", ButtonSizes.Large)]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with the tree!");
            StartCutting(); // Player automatically starts cutting when colliding
        }
    }
}
