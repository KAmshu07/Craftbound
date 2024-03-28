using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewRock", menuName = "Rock Type/Rock")]
public class Rock : SerializedScriptableObject
{
    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    public string rockName;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [PreviewField(70, ObjectFieldAlignment.Left)]
    public GameObject rockPrefab;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [Range(0, 1000)]
    public int baseHealth = 100;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [Range(0, 100)]
    public int miningDamage = 20;

    [Title("Rock Drops")]
    public List<RockDrop> drops = new List<RockDrop>();

    [FoldoutGroup("Common Methods")]
    [Button("Mine Rock", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public virtual void MineRock(Vector3 position)
    {
        Debug.Log($"{rockName} rock has been mined at {position}");

        // Handle drops
        foreach (var drop in drops)
        {
            if (drop.item != null && (!drop.isOptional || Random.Range(0f, 100f) <= drop.chance))
            {
                Item droppedItem = Instantiate(drop.item);
                droppedItem.quantity = drop.quantity; // Set the quantity
                droppedItem.Drop(GetRandomOffset(position));
                Debug.Log($"Dropped {drop.quantity} x {droppedItem.itemName} from {rockName}");
            }
        }
    }

    private Vector3 GetRandomOffset(Vector3 position)
    {
        Vector3 randomOffset = Random.onUnitSphere * 2f;
        randomOffset.y = Mathf.Abs(randomOffset.y); // Ensure a positive y value

        // Adjust the position to avoid spawning below the terrain
        Vector3 spawnPosition = position + randomOffset;

        // Raycast to check the terrain height
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition + Vector3.up * 100f, Vector3.down, out hit, 200f, LayerMask.GetMask("Terrain")))
        {
            spawnPosition.y = Mathf.Max(spawnPosition.y, hit.point.y);
        }

        return spawnPosition;
    }
}

[System.Serializable]
public class RockDrop
{
    public Item item;
    public int quantity = 1; // Default quantity is 1
    public bool isOptional;
    [Range(0, 100)]
    public float chance = 100; // Chance is only used if isOptional is true
}
