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
        // Refresh the UI when the inventory changes
        tabManager?.Refresh();
    }

    public void AddItemToInventory(IInventoryItem item)
    {
        if (item != null)
        {
            // Add the item with its specific quantity to the inventory
            inventory?.AddItem(item, item.Quantity);
        }
    }

    public void RemoveItemFromInventory(IInventoryItem item, int quantity)
    {
        if (item != null && quantity > 0)
        {
            // Remove the specified quantity of the item from the inventory
            inventory?.RemoveItem(item, quantity);
        }
    }
}
