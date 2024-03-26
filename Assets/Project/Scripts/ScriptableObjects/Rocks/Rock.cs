using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewRock", menuName = "Rock Type/Base Rock")]
public class Rock : ScriptableObject
{
    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [EnumPaging]
    public ROCK_TYPE typeName;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [PreviewField(70, ObjectFieldAlignment.Left)]
    public GameObject rockPrefab;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [Range(0, 1000)]
    public int baseHealth = 50;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [Range(0, 50)]
    public int miningDamage = 10;

    [FoldoutGroup("Common Methods")]
    [Button("Mine Rock", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public virtual void MineRock(Vector3 position)
    {
        Debug.Log($"{typeName} rock has been mined at {position}");
    }

    [FoldoutGroup("Common Methods")]
    [Button("Get Rock Type", ButtonSizes.Large, ButtonStyle.FoldoutButton)]
    [GUIColor(1, 0.8f, 0.8f)]
    public ROCK_TYPE GetRockType()
    {
        Debug.Log($"This rock is of type: {typeName}");
        return typeName;
    }

    private void OnValidate()
    {
        if (rockPrefab == null)
        {
            Debug.LogWarning($"{typeName} rockPrefab is not assigned in {name}.");
        }
    }
}

public enum ROCK_TYPE
{
    None,
    SMALL,
    MEDIUM,
    BIG
}
