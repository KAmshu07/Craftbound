using UnityEngine;

[CreateAssetMenu(fileName = "NewWood", menuName = "Items/Wood")]
public class Wood : Item
{
    [Header("Wood Properties")]
    public int woodAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {woodAmount} wood...");
    }
}