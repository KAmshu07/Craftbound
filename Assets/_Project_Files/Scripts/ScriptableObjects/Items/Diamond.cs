using UnityEngine;

[CreateAssetMenu(fileName = "NewDiamond", menuName = "Items/Diamond")]
public class Diamond : Item
{
    [Header("Diamond Properties")]
    public int diamondAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {diamondAmount} diamond...");
    }
}
