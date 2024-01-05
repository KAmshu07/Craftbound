using UnityEngine;

public class CuttingTree : MonoBehaviour
{
    private HealthComponent healthComponent;
    [SerializeField] private int cutDamage = 20;

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
        EventManager.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDestroy()
    {
        EventManager.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(GameObject entity, int newHealth)
    {
        if (entity == gameObject)
        {
            if (newHealth <= 0)
            {
                CutTree();
            }
        }
    }

    private void CutTree()
    {
        Debug.Log("Tree has been cut!");
        gameObject.SetActive(false);
    }

    public void StartCutting()
    {
        healthComponent.TakeDamage(cutDamage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with the tree!");

        }
    }
}
