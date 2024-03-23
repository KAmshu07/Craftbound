using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewBigRock", menuName = "Rock Type/Big Rock")]
public class BigRock : Rock
{
    [FoldoutGroup("Big Rock Properties")]
    [LabelWidth(80)]
    public int stoneYield = 20;

    [FoldoutGroup("Big Rock Properties")]
    [LabelWidth(100)]
    public RewardType rewardType;

    [FoldoutGroup("Big Rock Properties")]
    [LabelWidth(90)]
    public bool yieldsSpecialReward;

    [FoldoutGroup("Big Rock Methods")]
    [Button("Mine Big Rock", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public override void MineRock(Vector3 position)
    {
        base.MineRock(position);

        // Instantiate Stone
        Stone stoneItem = Instantiate(Resources.Load<Stone>("Stone"));
        stoneItem.stoneAmount = stoneYield;
        stoneItem.Drop(GetRandomOffset(position));

        string logMessage = $"Obtained {stoneYield} stone";

        // Check for special reward (50% chance)
        if (yieldsSpecialReward && Random.value < 0.5f && rewardType != RewardType.None)
        {
            // Check for special reward based on specific chances
            float randomValue = Random.value;

            // 90% chance for coal
            if (randomValue < 0.9f)
            {
                rewardType = RewardType.Coal;
            }
            // 10.5% chance for titanium
            else if (randomValue < 0.995f)
            {
                rewardType = RewardType.Titanium;
            }
            // 0.5% chance for diamond
            else
            {
                rewardType = RewardType.Diamond;
            }

            // Instantiate Special Reward based on the determined reward type
            InstantiateSpecialReward(position);

            logMessage += $" and received a rare reward ({rewardType})";
        }

        Debug.Log($"{logMessage} from mining a BigRock at {position}");
    }

    private void InstantiateSpecialReward(Vector3 position)
    {
        // Instantiate the appropriate special reward based on the determined reward type
        switch (rewardType)
        {
            case RewardType.Coal:
                Coal coalItem = Instantiate(Resources.Load<Coal>("Coal"));
                coalItem.Drop(GetRandomOffset(position));
                break;
            case RewardType.Titanium:
                TitaniumOre titaniumItem = Instantiate(Resources.Load<TitaniumOre>("Titanium"));
                titaniumItem.Drop(GetRandomOffset(position));
                break;
            case RewardType.Diamond:
                Diamond diamondItem = Instantiate(Resources.Load<Diamond>("Diamond"));
                diamondItem.Drop(GetRandomOffset(position));
                break;
        }
    }

    private Vector3 GetRandomOffset(Vector3 position)
    {
        Vector3 randomOffset = Random.onUnitSphere * 2f;
        randomOffset.y = Mathf.Abs(randomOffset.y); // Ensure a positive y value

        // Adjust the position to avoid spawning below the terrain
        Vector3 spawnPosition = position + randomOffset;

        // Raycast to check the terrain height
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition + Vector3.up * 100f, Vector3.down, out hit, 200f, LayerMask.GetMask("Terrain")))
        {
            spawnPosition.y = Mathf.Max(spawnPosition.y, hit.point.y);
        }

        return spawnPosition;
    }
}

public enum RewardType
{
    None,
    Coal,
    Titanium,
    Diamond
}
