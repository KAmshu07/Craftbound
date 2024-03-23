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

        // Instantiate Stone
        Stone stoneItem = Instantiate(Resources.Load<Stone>("Stone"));
        stoneItem.stoneAmount = stoneYield;
        stoneItem.Drop(GetRandomOffset(position));

        string logMessage = $"Obtained {stoneYield} stone";

        // Check for special reward (50% chance)
        if (yieldsSpecialReward && Random.value < 0.5f && rewardType != OreType.None)
        {
            // Instantiate Ore based on the reward type
            InstantiateOre(position);

            logMessage += $" and received a special reward ({rewardType})";
        }

        Debug.Log($"{logMessage} from mining a MediumRock at {position}");
    }

    private void InstantiateOre(Vector3 position)
    {
        Ore oreItem = null;

        switch (rewardType)
        {
            case OreType.Iron:
                oreItem = Instantiate(Resources.Load<IronOre>("IronOre"));
                break;
            case OreType.Copper:
                oreItem = Instantiate(Resources.Load<CopperOre>("CopperOre"));
                break;
            case OreType.Tin:
                oreItem = Instantiate(Resources.Load<TinOre>("TinOre"));
                break;
            default:
                break;
        }

        if (oreItem != null)
        {
            oreItem.Drop(GetRandomOffset(position));
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

public enum OreType
{
    None,
    Iron,
    Copper,
    Tin
}
