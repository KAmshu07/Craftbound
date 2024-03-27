using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : SerializedScriptableObject, IPickable
{
    [Header("Common Properties")]
    [LabelText("Item Name")]
    public string itemName;

    [LabelText("Quantity")]
    public int quantity = 1;

    [PreviewField(50)]
    [LabelText("Icon")]
    public Sprite icon;

    [LabelText("Use Types")]
    public List<UseType> useTypes = new List<UseType>();

    [Title("Prefab")]
    [InlineEditor(InlineEditorModes.GUIOnly)]
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
    }

    [Title("Dropping")]
    [Button("Drop", ButtonSizes.Large)]
    public virtual void Drop(Vector3 position)
    {
        Debug.Log($"Dropping {itemName} at {position}");
        SpawnItemInWorld(position);
    }

    [Title("Pickup")]
    public void Pickup(GameObject picker)
    {
        Debug.Log($"Picking up {itemName}");
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
}
