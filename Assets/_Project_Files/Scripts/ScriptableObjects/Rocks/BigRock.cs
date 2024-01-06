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

        string logMessage = $"Obtained {stoneYield} stone";

        // Check for special reward (50% chance)
        if (yieldsSpecialReward && Random.value < 0.5f && rewardType != RewardType.None)
        {
            rewardType = GetRandomSpecialReward();
            logMessage += $" and received a rare reward ({rewardType})";
        }

        Debug.Log($"{logMessage} from mining a BigRock at {position}");

        BreakIntoMediumAndSmallRocks(position);
    }

    private void BreakIntoMediumAndSmallRocks(Vector3 position)
    {
        Debug.Log($"BigRock at {position} is breaking into MediumRocks and SmallRocks.");

        // Instantiate and place MediumRocks and SmallRocks at the same position
        // ... Your instantiation logic here ...

        // Optionally, you can set health, yield, or other properties for MediumRocks and SmallRocks
        // ...
    }

    private RewardType GetRandomSpecialReward()
    {
        float randomValue = Random.value;

        // 90% chance for coal
        if (randomValue < 0.9f)
        {
            return RewardType.Coal;
        }
        // 10.5% chance for titanium
        else if (randomValue < 0.995f)
        {
            return RewardType.Titanium;
        }
        // 0.5% chance for diamond
        else
        {
            return RewardType.Diamond;
        }
    }
}

public enum RewardType
{
    None,
    Coal,
    Titanium,
    Diamond
}
