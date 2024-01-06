using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewSmallTree", menuName = "Tree Type/Small Tree")]
public class SmallTree : TreeBase
{
    [FoldoutGroup("Small Tree Properties")]
    [LabelWidth(80)]
    public int woodYield = 5;

    [FoldoutGroup("Small Tree Methods")]
    [Button("Cut Small Tree", ButtonSizes.Large)]
    [GUIColor(0.8f, 0.8f, 1)]
    public override void CutTree(Vector3 position)
    {
        base.CutTree(position);
        Debug.Log($"Obtained {woodYield} wood from cutting a SmallTree at {position}");
    }
}
