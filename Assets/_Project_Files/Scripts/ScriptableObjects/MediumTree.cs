using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewMediumTree", menuName = "Tree Type/Medium Tree")]
public class MediumTree : TreeBase
{
    [FoldoutGroup("Medium Tree Properties")]
    [LabelWidth(90)]
    public int woodYield = 10;

    [FoldoutGroup("Medium Tree Methods")]
    [Button("Cut Medium Tree", ButtonSizes.Large)]
    [GUIColor(1, 0.8f, 0.8f)]
    public override void CutTree(Vector3 position)
    {
        base.CutTree(position);
        Debug.Log($"Obtained {woodYield} wood from cutting a MediumTree at {position}");
    }
}
