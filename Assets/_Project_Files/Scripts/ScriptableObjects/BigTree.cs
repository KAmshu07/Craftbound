using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewBigTree", menuName = "Tree Type/Big Tree")]
public class BigTree : TreeBase
{
    [FoldoutGroup("Big Tree Properties")]
    [LabelWidth(80)]
    public int woodYield = 20;

    [FoldoutGroup("Big Tree Properties")]
    [LabelWidth(100)]
    public bool dropsFruits = true;

    [FoldoutGroup("Big Tree Methods")]
    [Button("Cut Big Tree", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public override void CutTree(Vector3 position)
    {
        base.CutTree(position);
        Debug.Log($"Obtained {woodYield} wood from cutting a BigTree at {position}");

        if (dropsFruits)
        {
            Debug.Log($"Found some fruits near the fallen BigTree at {position}");
        }
    }
}
