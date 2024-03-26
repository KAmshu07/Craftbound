using UnityEngine;

[CreateAssetMenu(fileName = "NewFlint", menuName = "Items/Flint")]
public class Flint : Item
{
    [Header("Flint Properties")]
    public int flintAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {flintAmount} flint...");
    }
}
