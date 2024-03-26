using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewTree", menuName = "Tree Type/Base Tree")]
public class Tree : ScriptableObject
{
    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [EnumPaging]
    public TREE_TYPE typeName;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [PreviewField(70, ObjectFieldAlignment.Left)]
    public GameObject treePrefab;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [Range(0, 1000)]
    public int baseHealth = 100;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [Range(0, 100)]
    public int cutDamage = 20;

    [FoldoutGroup("Common Methods")]
    [Button("Cut Tree", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public virtual void CutTree(Vector3 position)
    {
        Debug.Log($"{typeName} tree has been cut at {position}");
    }

    [FoldoutGroup("Common Methods")]
    [Button("Get Tree Type", ButtonSizes.Large, ButtonStyle.FoldoutButton)]
    [GUIColor(1, 0.8f, 0.8f)]
    public TREE_TYPE GetTreeType()
    {
        Debug.Log($"This tree is of type: {typeName}");
        return typeName;
    }

    private void OnValidate()
    {
        if (treePrefab == null)
        {
            Debug.LogWarning($"{typeName} treePrefab is not assigned in {name}.");
        }
    }
}

public enum TREE_TYPE
{
    None,
    SMALL,
    MEDIUM,
    BIG
}
