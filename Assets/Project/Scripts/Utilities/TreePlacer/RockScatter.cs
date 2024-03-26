using UnityEngine;
using Sirenix.OdinInspector;

public class RockScatterer : MonoBehaviour
{
    [SerializeField]
    [Required]
    [InfoBox("Assign different rock types here.")]
    private Rock[] rockTypes;

    [SerializeField]
    [MinValue(1)]
    [LabelWidth(150)]
    [Tooltip("Number of rocks to be scattered.")]
    private int rockDensity = 100;

    private Rock rockType;

    private void SetRockType(Rock selectedRock)
    {
        if (selectedRock != null)
        {
            rockType = selectedRock;
            Debug.Log($"Rock type set to: {rockType.typeName}");
        }
        else
        {
            Debug.LogError("Select a Rock scriptable object to set as the rock type.");
        }
    }

    [Button("Scatter Rocks", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    private void ScatterRocks()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < rockDensity; i++)
        {
            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);

            float y = terrain.SampleHeight(new Vector3(x, 0f, z));

            Vector3 rockPosition = new Vector3(x, y, z);

            // Randomly choose a rock type
            Rock originalRockType = rockTypes[Random.Range(0, rockTypes.Length)];

            // Create a copy of the original rock type to ensure uniqueness
            Rock rockType = Instantiate(originalRockType);

            // Instantiate the rock prefab
            GameObject rock = Instantiate(rockType.rockPrefab, rockPosition, Quaternion.identity);
            rock.transform.parent = terrain.transform;

            // Attach HealthComponent only if not already present
            HealthComponent healthComponent = rock.GetComponent<HealthComponent>();
            if (healthComponent == null)
            {
                healthComponent = rock.AddComponent<HealthComponent>();
                healthComponent.maxHealth = rockType.baseHealth;
                healthComponent.currentHealth = healthComponent.maxHealth;
            }

            // Attach MiningRock only if not already present
            MiningRock miningRock = rock.GetComponent<MiningRock>();
            if (miningRock == null)
            {
                miningRock = rock.AddComponent<MiningRock>();
                SetRockType(rockType);
                miningRock.breakDamage = rockType.miningDamage;
                miningRock.rock = rockType;

                // Randomize boolean properties
                if (rockType is SmallRock smallRock)
                {
                    smallRock.yieldsFlint = Random.value > 0.5f; // 50% chance
                }
                else if (rockType is MediumRock mediumRock)
                {
                    mediumRock.yieldsSpecialReward = Random.value > 0.5f; // 50% chance
                    mediumRock.rewardType = GetRandomOreType();
                }
                else if (rockType is BigRock bigRock)
                {
                    bigRock.yieldsSpecialReward = Random.value > 0.5f; // 50% chance
                    bigRock.rewardType = GetRandomSpecialRewardType();
                }
            }
        }
    }

    private OreType GetRandomOreType()
    {
        // Implement logic to get a random ore type here
        // For example: return OreType.Iron;
        return (OreType)Random.Range(1, System.Enum.GetValues(typeof(OreType)).Length);
    }

    private RewardType GetRandomSpecialRewardType()
    {
        // Implement logic to get a random special reward type here
        // For example: return RewardType.Coal;
        return (RewardType)Random.Range(1, System.Enum.GetValues(typeof(RewardType)).Length);
    }

    [Button("Remove All Rocks", ButtonSizes.Large)]
    [GUIColor(1, 0.8f, 0.8f)]
    private void RemoveAllRocks()
    {
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");

        foreach (var rock in rocks)
        {
            DestroyImmediate(rock);
        }
    }
}
