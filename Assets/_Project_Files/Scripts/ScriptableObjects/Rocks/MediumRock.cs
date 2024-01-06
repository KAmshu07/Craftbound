using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewMediumRock", menuName = "Rock Type/Medium Rock")]
public class MediumRock : Rock
{
    [FoldoutGroup("Medium Rock Properties")]
    [LabelWidth(90)]
    public int stoneYield = 10;

    [FoldoutGroup("Medium Rock Properties")]
    [LabelWidth(90)]
    [EnumToggleButtons]
    public OreType rewardType;

    [FoldoutGroup("Medium Rock Properties")]
    [LabelWidth(90)]
    public bool yieldsSpecialReward;

    [FoldoutGroup("Medium Rock Methods")]
    [Button("Mine Medium Rock", ButtonSizes.Large)]
    [GUIColor(1, 0.8f, 0.8f)]
    public override void MineRock(Vector3 position)
    {
        base.MineRock(position);

        string logMessage = $"Obtained {stoneYield} stone";

        // Check for special reward (50% chance)
        if (yieldsSpecialReward && Random.value < 0.5f && rewardType != OreType.None)
        {
            // Randomly select one of the available ore types
            OreType[] availableOres = { OreType.Iron, OreType.Copper, OreType.Tin };
            rewardType = availableOres[Random.Range(0, availableOres.Length)];

            logMessage += $" and received a special reward ({rewardType})";
        }

        Debug.Log($"{logMessage} from mining a MediumRock at {position}");

        BreakIntoSmallRocks(position);
    }

    private void BreakIntoSmallRocks(Vector3 position)
    {
        Debug.Log($"MediumRock at {position} is breaking into SmallRocks.");

        // Instantiate and place SmallRocks at the same position
        // ... Your instantiation logic here ...

        // Optionally, you can set health, yield, or other properties for SmallRocks
        // ...
    }
}

public enum OreType
{
    None,
    Iron,
    Copper,
    Tin
}
