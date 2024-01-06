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

        ROCK_TYPE rockType = rock.GetRockType();

        switch (rockType)
        {
            case ROCK_TYPE.SMALL:
                if (rock is SmallRock smallRock)
                {
                    smallRock.MineRock(transform.position);
                }
                else
                {
                    Debug.LogError("Invalid rock type for breaking.");
                }
                break;

            case ROCK_TYPE.MEDIUM:
                if (rock is MediumRock mediumRock)
                {
                    mediumRock.MineRock(transform.position);
                }
                else
                {
                    Debug.LogError("Invalid rock type for breaking.");
                }
                break;

            case ROCK_TYPE.BIG:
                if (rock is BigRock bigRock)
                {
                    bigRock.MineRock(transform.position);
                }
                else
                {
                    Debug.LogError("Invalid rock type for breaking.");
                }
                break;

            default:
                Debug.LogError($"Unknown rock type: {rockType}");
                break;
        }

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
