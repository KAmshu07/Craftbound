using UnityEngine;

[CreateAssetMenu(fileName = "NewIronOre", menuName = "Items/Ore/Iron Ore")]
public class IronOre : Ore
{
    [Header("Iron Ore Properties")]
    public int ironAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {ironAmount} iron ore...");
    }

    // Override the SmeltOre method for iron ore
    protected override void SmeltOre()
    {
        Debug.Log($"Smelting {itemName} into iron ingot...");
    }
}
