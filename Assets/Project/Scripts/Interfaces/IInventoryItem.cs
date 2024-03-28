using UnityEngine;

public interface IInventoryItem
{
    string ItemName { get; }
    Sprite Icon { get; }
    ItemCategory Category { get; }
    int Quantity { get; set; }
    void Use(GameObject user);
    // Add more common properties and methods as needed
}
