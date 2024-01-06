using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewSmallTree", menuName = "Tree Type/Small Tree")]
public class SmallTree : Tree
{
    [FoldoutGroup("Small Tree Properties")]
    [LabelWidth(80)]
    public int woodYield = 5;

    [FoldoutGroup("Small Tree Properties")]
    [LabelWidth(80)]
    [InfoBox("Set to true if the Small Tree should yield fibre when cut.")]
    public bool yieldsFibre;

    [FoldoutGroup("Small Tree Properties")]
    [ShowIf("yieldsFibre")]
    [LabelWidth(80)]
    public int fibreYield = 2;

    [FoldoutGroup("Small Tree Methods")]
    [Button("Cut Small Tree", ButtonSizes.Large)]
    [GUIColor(0.8f, 0.8f, 1)]
    public override void CutTree(Vector3 position)
    {
        base.CutTree(position);

        string logMessage = $"Obtained {woodYield} wood";

        if (yieldsFibre && fibreYield > 0)
        {
            logMessage += $" and {fibreYield} fibre";
        }

        Debug.Log($"{logMessage} from cutting a SmallTree at {position}");
    }
}
