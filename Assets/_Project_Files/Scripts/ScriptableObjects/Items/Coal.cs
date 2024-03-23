using UnityEngine;

[CreateAssetMenu(fileName = "NewCoal", menuName = "Items/Coal")]
public class Coal : Item
{
    [Header("Coal Properties")]
    public int coalAmount = 1;

    public override void Use()
    {
        base.Use();
        Debug.Log($"Using {coalAmount} coal...");
    }
}
