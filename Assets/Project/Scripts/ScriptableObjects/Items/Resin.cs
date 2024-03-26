using UnityEngine;

[CreateAssetMenu(fileName = "NewResin", menuName = "Items/Resin")]
public class Resin : Item
{
    [Header("Resin Properties")]
    public int resinAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {resinAmount} resin...");
    }
}
