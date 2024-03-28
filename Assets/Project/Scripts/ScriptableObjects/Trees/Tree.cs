using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewTree", menuName = "Tree Type/Tree")]
public class Tree : SerializedScriptableObject
{
    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    public string treeName;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [PreviewField(70, ObjectFieldAlignment.Left)]
    public GameObject treePrefab;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [Range(0, 1000)]
    public int baseHealth = 100;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [Range(0, 100)]
    public int cutDamage = 20;

    [Title("Tree Drops")]
    public List<TreeDrop> drops = new List<TreeDrop>();

    [FoldoutGroup("Common Methods")]
    [Button("Cut Tree", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public virtual void CutTree(Vector3 position)
    {
        Debug.Log($"{treeName} tree has been cut at {position}");

        // Handle drops
        foreach (var drop in drops)
        {
            if (!drop.isOptional || Random.Range(0f, 100f) <= drop.chance)
            {
                Item droppedItem = Instantiate(drop.item);
                droppedItem.quantity = drop.quantity; // Set the quantity
                droppedItem.Drop(GetRandomOffset(position));
                Debug.Log($"Dropped {drop.quantity} x {droppedItem.itemName} from {treeName}");
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
public class TreeDrop
{
    public Item item;
    public int quantity = 1; // Default quantity is 1
    public bool isOptional;
    [Range(0, 100)]
    public float chance = 100; // Chance is only used if isOptional is true
}
