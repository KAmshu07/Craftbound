using UnityEngine;

public interface IInventoryItem
{
    string ItemName { get; }
    Sprite Icon { get; }
    ItemCategory Category { get; }
    int Quantity { get; set; }
    int Rarity { get; }
    string Description { get; }
    void Use(GameObject user);
    // Add more common properties and methods as needed
}
