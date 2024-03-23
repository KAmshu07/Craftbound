using UnityEngine;

[CreateAssetMenu(fileName = "NewStone", menuName = "Items/Stone")]
public class Stone : Item
{
    [Header("Stone Properties")]
    public int stoneAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {stoneAmount} stone...");
    }
}
