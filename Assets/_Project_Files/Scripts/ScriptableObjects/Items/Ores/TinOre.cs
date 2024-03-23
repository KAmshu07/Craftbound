using UnityEngine;

[CreateAssetMenu(fileName = "NewTinOre", menuName = "Items/Ore/Tin Ore")]
public class TinOre : Ore
{
    [Header("Tin Ore Properties")]
    public int tinAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {tinAmount} tin ore...");
    }

    // Override the SmeltOre method for tin ore
    protected override void SmeltOre()
    {
        Debug.Log($"Smelting {itemName} into tin ingot...");
    }
}
