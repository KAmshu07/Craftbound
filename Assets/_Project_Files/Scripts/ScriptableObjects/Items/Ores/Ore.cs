using UnityEngine;

public class Ore : Item
{
    [Header("Ore Properties")]
    public int oreAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {oreAmount} {itemName} ore...");
        SmeltOre(); // Call the common method
    }

    // Common method for smelting ore
    protected virtual void SmeltOre()
    {
        Debug.Log($"Smelting {itemName} ore...");
    }
}
