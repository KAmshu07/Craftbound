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
            item.Pickup(picker);
            Debug.Log($"Picked up {item.itemName} x{item.quantity} by {picker.name}");

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Item is null. Make sure to set the item before calling Pickup.");
        }
    }
}
