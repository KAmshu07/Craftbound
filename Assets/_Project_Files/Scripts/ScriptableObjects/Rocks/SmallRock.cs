using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewSmallRock", menuName = "Rock Type/Small Rock")]
public class SmallRock : Rock
{
    [FoldoutGroup("Small Rock Properties")]
    [LabelWidth(80)]
    public int stoneYield = 5;

    [FoldoutGroup("Small Rock Properties")]
    [LabelWidth(80)]
    [InfoBox("Set to true if the Small Rock should yield flint when mined.")]
    public bool yieldsFlint;

    [FoldoutGroup("Small Rock Properties")]
    [ShowIf("yieldsFlint")]
    [LabelWidth(80)]
    public int flintYield = 2;

    [FoldoutGroup("Small Rock Methods")]
    [Button("Mine Small Rock", ButtonSizes.Large)]
    [GUIColor(0.8f, 0.8f, 1)]
    public override void MineRock(Vector3 position)
    {
        base.MineRock(position);

        // Instantiate Stone
        Stone stoneItem = Instantiate(Resources.Load<Stone>("Stone"));
        stoneItem.stoneAmount = stoneYield;
        stoneItem.Drop(GetRandomOffset(position));

        string logMessage = $"Obtained {stoneYield} stone";

        if (yieldsFlint && flintYield > 0)
        {
            // Instantiate Flint
            Flint flintItem = Instantiate(Resources.Load<Flint>("Flint"));
            flintItem.flintAmount = flintYield;
            flintItem.Drop(GetRandomOffset(position));

            logMessage += $" and {flintYield} flint";
        }

        Debug.Log($"{logMessage} from mining a SmallRock at {position}");
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