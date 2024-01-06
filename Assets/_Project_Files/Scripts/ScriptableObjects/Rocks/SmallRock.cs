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

        string logMessage = $"Obtained {stoneYield} stone";

        if (yieldsFlint && flintYield > 0)
        {
            logMessage += $" and {flintYield} flint";
        }

        Debug.Log($"{logMessage} from mining a SmallRock at {position}");
    }
}
