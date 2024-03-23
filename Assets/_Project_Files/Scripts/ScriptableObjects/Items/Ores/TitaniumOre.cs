using UnityEngine;

[CreateAssetMenu(fileName = "NewTitaniumOre", menuName = "Items/Ore/Titanium Ore")]
public class TitaniumOre : Ore
{
    [Header("Titanium Ore Properties")]
    public int titaniumAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {titaniumAmount} titanium ore...");
    }

    // Override the SmeltOre method for titanium ore
    protected override void SmeltOre()
    {
        Debug.Log($"Smelting {itemName} into titanium ingot...");
    }
}
