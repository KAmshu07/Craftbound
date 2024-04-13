using UnityEngine;

public class ItemPickup : MonoBehaviour, IPickable
{
    private Item item;

    public void SetItem(Item newItem)
    {
        item = newItem;
    }

    public Item GetItem()
    {
        return item;
    }

    public void Pickup(GameObject picker)
    {
        if (item != null)
        {
            Inventory playerInventory = picker.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(item, item.quantity);
                Debug.Log($"Picked up {item.itemName} x{item.quantity} by {picker.name} and added to inventory.");

                // Destroy the pickup object from the game world after the item has been picked up
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Picker does not have an Inventory component.");
            }
        }
        else
        {
            Debug.LogError("Item is null. Make sure to set the item before calling Pickup.");
        }
    }
}
