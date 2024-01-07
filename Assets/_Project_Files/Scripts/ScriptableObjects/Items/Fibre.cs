using UnityEngine;

[CreateAssetMenu(fileName = "NewFibre", menuName = "Items/Fibre")]
public class Fibre : Item
{
    [Header("Fibre Properties")]
    public int fibreAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Crafting with {fibreAmount} fibre...");
    }
}
