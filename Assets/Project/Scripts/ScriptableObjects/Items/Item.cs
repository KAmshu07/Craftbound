using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class Item : SerializedScriptableObject
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
    public virtual void Use()
    {
        Debug.Log($"Using {itemName}");
        // You can access custom properties here using customProperties["propertyName"]
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
