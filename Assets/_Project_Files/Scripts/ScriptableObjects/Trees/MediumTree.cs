using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewMediumTree", menuName = "Tree Type/Medium Tree")]
public class MediumTree : Tree
{
    [FoldoutGroup("Medium Tree Properties")]
    [LabelWidth(90)]
    public int woodYield = 10;

    [FoldoutGroup("Medium Tree Properties")]
    [LabelWidth(90)]
    [InfoBox("Set to true if the Medium Tree should yield resin when cut.")]
    public bool yieldsResin;

    [FoldoutGroup("Medium Tree Properties")]
    [ShowIf("yieldsResin")]
    [LabelWidth(90)]
    public int resinYield = 3;

    [FoldoutGroup("Medium Tree Methods")]
    [Button("Cut Medium Tree", ButtonSizes.Large)]
    [GUIColor(1, 0.8f, 0.8f)]
    public override void CutTree(Vector3 position)
    {
        base.CutTree(position);

        string logMessage = $"Obtained {woodYield} wood";

        if (yieldsResin)
        {
            logMessage += $" and {resinYield} resin";
        }

        Debug.Log($"{logMessage} from cutting a MediumTree at {position}");
    }
}
