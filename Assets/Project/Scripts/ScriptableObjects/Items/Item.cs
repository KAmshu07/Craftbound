using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Item : ScriptableObject, IPickable
{
    [Header("Common Properties")]
    [LabelText("Item Name")]
    public string itemName;

    [PreviewField(50)]
    [LabelText("Icon")]
    public Sprite icon;

    [Title("Prefab")]
    [InlineEditor(InlineEditorModes.GUIOnly)]
    public GameObject itemPrefab;

    [Title("Usage")]
    [Button("Use", ButtonSizes.Large)]
    public virtual void Use()
    {
        Debug.Log($"Using {itemName}");
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
