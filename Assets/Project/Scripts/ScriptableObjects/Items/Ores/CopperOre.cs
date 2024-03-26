using UnityEngine;

[CreateAssetMenu(fileName = "NewCopperOre", menuName = "Items/Ore/Copper Ore")]
public class CopperOre : Ore
{
    [Header("Copper Ore Properties")]
    public int copperAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {copperAmount} copper ore...");
    }

    // Override the SmeltOre method for copper ore
    protected override void SmeltOre()
    {
        Debug.Log($"Smelting {itemName} into copper ingot...");
    }
}
