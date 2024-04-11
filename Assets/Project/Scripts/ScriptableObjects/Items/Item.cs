using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : SerializedScriptableObject, IInventoryItem
{
    [Header("Common Properties")]
    [LabelText("Item Name")]
    public string itemName;

    [LabelText("Quantity")]
    public int quantity = 1;

    [PreviewField(50)]
    [LabelText("Icon")]
    public Sprite icon;

    [Header("Rarity")]
    [Range(0, 5)]
    public int rarity = 1;

    [TextArea]
    [LabelText("Description")]
    public string description;

    [LabelText("Category")]
    public ItemCategory category;

    [LabelText("Use Types")]
    public List<UseType> useTypes = new List<UseType>();

    [Title("Prefab")]
    [InlineEditor(InlineEditorModes.GUIAndPreview)]
    public GameObject itemPrefab;

    [Title("Custom Properties")]
    [DictionaryDrawerSettings(KeyLabel = "Property", ValueLabel = "Value")]
    public Dictionary<string, string> customProperties = new Dictionary<string, string>();

    [Title("Usage")]
    [Button("Use", ButtonSizes.Large)]
    public virtual void Use(GameObject user)
    {
        Debug.Log($"Using {itemName}");

        foreach (var useType in useTypes)
        {
            switch (useType)
            {
                case UseType.Fuel:
                    Debug.Log($"{itemName} used as fuel.");
                    // Add fuel usage code here
                    break;

                case UseType.Craft:
                    Debug.Log($"Crafting with {itemName}.");
                    // Add crafting code here
                    break;

                case UseType.Eat:
                    Debug.Log($"Eating {itemName}.");
                    // Add eating code here
                    break;

                case UseType.Smelt:
                    Debug.Log($"Smelting {itemName}.");
                    // Add smelting code here
                    break;

                    // Add more cases for other use types as needed
            }
        }

        // Raise an event when the item is used
        EventDispatcher.Publish(new ItemUsedEvent(this, user));
    }

    [Title("Dropping")]
    [Button("Drop", ButtonSizes.Large)]
    public virtual void Drop(Vector3 position)
    {
        Debug.Log($"Dropping {itemName} at {position}");
        SpawnItemInWorld(position);
        // Raise an event when the item is dropped
        EventDispatcher.Publish(new ItemDroppedEvent(this, position));
    }

    [Title("Pickup")]
    public void Pickup(GameObject picker)
    {
        Debug.Log($"Picking up {itemName}");
        // Raise an event when the item is picked up
        EventDispatcher.Publish(new ItemPickedUpEvent(this, picker));
    }

    private void SpawnItemInWorld(Vector3 position)
    {
        if (itemPrefab != null)
        {
            GameObject spawnedItem = Instantiate(itemPrefab, position, Quaternion.identity);
            ItemPickup itemPickup = spawnedItem.AddComponent<ItemPickup>();
            itemPickup.SetItem(this);
        }
        else
        {
            Debug.LogError($"Item {itemName} doesn't have a prefab assigned.");
        }
    }

    // IInventoryItem interface implementation
    public string ItemName => itemName;
    public Sprite Icon => icon;
    public ItemCategory Category => category;
    public int Rarity => rarity;
    public int Quantity
    {
        get => quantity;
        set => quantity = Mathf.Max(0, value); // Ensure quantity is never negative
    }
    public string Description => description;
}

// Define events for item actions
public class ItemUsedEvent
{
    public IInventoryItem Item { get; private set; }
    public GameObject User { get; private set; }

    public ItemUsedEvent(IInventoryItem item, GameObject user)
    {
        Item = item;
        User = user;
    }
}

public class ItemDroppedEvent
{
    public IInventoryItem Item { get; private set; }
    public Vector3 Position { get; private set; }

    public ItemDroppedEvent(IInventoryItem item, Vector3 position)
    {
        Item = item;
        Position = position;
    }
}

public class ItemPickedUpEvent
{
    public IInventoryItem Item { get; private set; }
    public GameObject Picker { get; private set; }

    public ItemPickedUpEvent(IInventoryItem item, GameObject picker)
    {
        Item = item;
        Picker = picker;
    }
}
