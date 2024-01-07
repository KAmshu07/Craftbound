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
        item.Pickup(picker);
        Destroy(gameObject);
    }
}