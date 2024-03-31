using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<ItemCategory, List<IInventoryItem>> itemsByCategory = new Dictionary<ItemCategory, List<IInventoryItem>>();

    public void AddItem(IInventoryItem item)
    {
        if (item == null)
        {
            Debug.LogError("Cannot add a null item to the inventory.");
            return;
        }

        if (item.Quantity <= 0)
        {
            Debug.LogWarning($"Attempting to add an item with non-positive quantity: {item.ItemName} x{item.Quantity}");
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
            Debug.Log($"Increased quantity of {item.ItemName} to {existingItem.Quantity} in inventory under category {item.Category}.");
        }
        else
        {
            itemsByCategory[item.Category].Add(item);
            Debug.Log($"Added {item.ItemName} (x{item.Quantity}) to inventory under category {item.Category}.");
        }
    }

    public void RemoveItem(IInventoryItem item, int quantityToRemove)
    {
        if (item == null)
        {
            Debug.LogError("Cannot remove a null item from the inventory.");
            return;
        }

        if (quantityToRemove <= 0)
        {
            Debug.LogWarning($"Attempting to remove a non-positive quantity of {item.ItemName}: {quantityToRemove}");
            return;
        }

        if (itemsByCategory.ContainsKey(item.Category))
        {
            var existingItem = itemsByCategory[item.Category].Find(i => i.ItemName == item.ItemName);
            if (existingItem != null && existingItem.Quantity >= quantityToRemove)
            {
                existingItem.Quantity -= quantityToRemove;
                Debug.Log($"Removed {quantityToRemove} of {item.ItemName} from inventory. New quantity: {existingItem.Quantity}");

                if (existingItem.Quantity <= 0)
                {
                    itemsByCategory[item.Category].Remove(existingItem);
                    Debug.Log($"{item.ItemName} removed from inventory as quantity is now zero.");
                }
            }
            else
            {
                Debug.LogWarning($"Failed to remove {quantityToRemove} of {item.ItemName} from inventory. Not enough quantity or item not found.");
            }
        }
        else
        {
            Debug.LogWarning($"Failed to remove {item.ItemName} from inventory. Category {item.Category} not found.");
        }
    }

    public List<IInventoryItem> GetItemsByCategory(ItemCategory category)
    {
        if (itemsByCategory.ContainsKey(category))
        {
            Debug.Log($"Retrieved items under category {category}: {string.Join(", ", itemsByCategory[category].ConvertAll(item => $"{item.ItemName} x{item.Quantity}"))}");
            return itemsByCategory[category];
        }
        else
        {
            Debug.Log($"No items found under category {category}.");
            return new List<IInventoryItem>();
        }
    }
}
