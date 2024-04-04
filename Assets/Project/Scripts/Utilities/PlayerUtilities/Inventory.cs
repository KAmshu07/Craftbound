using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action OnInventoryChanged;

    private Dictionary<ItemCategory, List<IInventoryItem>> itemsByCategory = new Dictionary<ItemCategory, List<IInventoryItem>>();

    public void AddItem(IInventoryItem item)
    {
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
        }

        OnInventoryChanged?.Invoke();
    }

    public List<IInventoryItem> GetItemsByCategory(ItemCategory category)
    {
        return itemsByCategory.ContainsKey(category) ? itemsByCategory[category] : new List<IInventoryItem>();
    }
}
