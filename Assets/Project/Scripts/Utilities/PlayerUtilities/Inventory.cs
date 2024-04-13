using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<ItemCategory, List<IInventoryItem>> itemsByCategory = new Dictionary<ItemCategory, List<IInventoryItem>>();

    public void AddItem(IInventoryItem item, int quantity)
    {
        if (item == null)
        {
            Debug.LogWarning("Attempted to add a null item to the inventory.");
            return;
        }

        if (quantity <= 0)
        {
            Debug.LogWarning("Attempted to add an item with non-positive quantity.");
            return;
        }

        if (!itemsByCategory.ContainsKey(item.Category))
        {
            itemsByCategory[item.Category] = new List<IInventoryItem>();
        }

        var existingItem = itemsByCategory[item.Category].Find(i => i.ItemName == item.ItemName);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            // If the item does not exist, clone it and set the quantity
            // Here, we ensure that the item is actually an instance of Item, which can be instantiated
            if (item is Item concreteItem)
            {
                Item newItem = Instantiate(concreteItem); // Instantiating the ScriptableObject
                newItem.Quantity = quantity; // Setting quantity
                itemsByCategory[item.Category].Add(newItem);
            }
            else
            {
                Debug.LogError("The item is not a concrete instance that can be instantiated.");
            }
        }

        // Publish an inventory change event
        EventDispatcher.Publish(new InventoryChangedEvent());
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

        // Publish an inventory change event
        EventDispatcher.Publish(new InventoryChangedEvent());
    }

    public List<IInventoryItem> GetItemsByCategory(ItemCategory category)
    {
        return itemsByCategory.ContainsKey(category) ? itemsByCategory[category] : new List<IInventoryItem>();
    }

    public bool HasItem(IInventoryItem item, int quantity)
    {
        if (item == null) return false;

        if (itemsByCategory.TryGetValue(item.Category, out var items))
        {
            var foundItem = items.Find(i => i.ItemName == item.ItemName);
            if (foundItem != null && foundItem.Quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }

    public int GetItemCount(Item item)
    {
        if (itemsByCategory.TryGetValue(item.Category, out var itemList))
        {
            var foundItem = itemList.Find(i => i.ItemName == item.ItemName);
            return foundItem != null ? foundItem.Quantity : 0;
        }
        return 0;
    }

}

// Define an event for inventory changes
public class InventoryChangedEvent { }
