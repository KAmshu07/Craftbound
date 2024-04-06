using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChanged;

    private Dictionary<ItemCategory, List<IInventoryItem>> itemsByCategory = new Dictionary<ItemCategory, List<IInventoryItem>>();

    public void AddItem(IInventoryItem item)
    {
        if (item == null)
        {
            Debug.LogWarning("Attempted to add a null item to the inventory.");
            return;
        }

        if (!itemsByCategory.ContainsKey(item.Category))
        {
            itemsByCategory[item.Category] = new List<IInventoryItem>();
        }

        var existingItem = itemsByCategory[item.Category].Find(i => i.ItemName == item.ItemName);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            itemsByCategory[item.Category].Add(item);
        }

        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(IInventoryItem item, int quantityToRemove)
    {
        if (item == null)
        {
            Debug.LogWarning("Attempted to remove a null item from the inventory.");
            return;
        }

        if (quantityToRemove <= 0)
        {
            Debug.LogWarning("Attempted to remove a non-positive quantity of an item.");
            return;
        }

        if (itemsByCategory.ContainsKey(item.Category))
        {
            var existingItem = itemsByCategory[item.Category].Find(i => i.ItemName == item.ItemName);
            if (existingItem != null && existingItem.Quantity >= quantityToRemove)
            {
                existingItem.Quantity -= quantityToRemove;
                if (existingItem.Quantity <= 0)
                {
                    itemsByCategory[item.Category].Remove(existingItem);
                }
            }
            else
            {
                Debug.LogWarning($"Attempted to remove an item that does not exist in the inventory: {item.ItemName}");
            }
        }
        else
        {
            Debug.LogWarning($"Attempted to remove an item from a category that does not exist: {item.Category}");
        }

        OnInventoryChanged?.Invoke();
    }

    public List<IInventoryItem> GetItemsByCategory(ItemCategory category)
    {
        return itemsByCategory.ContainsKey(category) ? itemsByCategory[category] : new List<IInventoryItem>();
    }
}
