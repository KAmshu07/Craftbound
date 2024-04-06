using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryTabManager tabManager;

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateInventoryUI;
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateInventoryUI;
        }
    }

    private void UpdateInventoryUI()
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
