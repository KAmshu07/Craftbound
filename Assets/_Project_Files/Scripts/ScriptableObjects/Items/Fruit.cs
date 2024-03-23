using UnityEngine;

[CreateAssetMenu(fileName = "NewFruit", menuName = "Items/Fruit")]
public class Fruit : Item
{
    [Header("Fruit Properties")]
    public int fruitAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Eating {fruitAmount} fruit...");
    }
}
