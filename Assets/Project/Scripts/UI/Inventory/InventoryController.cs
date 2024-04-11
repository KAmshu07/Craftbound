using UnityEngine;

public class InventoryController : EventReceiver
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryTabManager tabManager;

    private void OnEnable()
    {
        Subscribe<InventoryChangedEvent>(UpdateInventoryUI);
    }

    private void UpdateInventoryUI(InventoryChangedEvent eventArgs)
    {
        tabManager?.Refresh();
    }

    public void AddItemToInventory(IInventoryItem item)
    {
        if (item != null)
        {
            inventory?.AddItem(item);
        }
    }

    public void RemoveItemFromInventory(IInventoryItem item, int quantity)
    {
        if (item != null && quantity > 0)
        {
            inventory?.RemoveItem(item, quantity);
        }
    }
}
