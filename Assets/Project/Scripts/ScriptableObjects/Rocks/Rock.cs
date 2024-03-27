using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

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
    public int baseHealth = 50;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    [Range(0, 50)]
    public int miningDamage = 10;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    public int stoneYield = 5;

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    public bool yieldsSpecialReward;

    [Title("Custom Properties")]
    [DictionaryDrawerSettings(KeyLabel = "Property", ValueLabel = "Value")]
    public Dictionary<string, string> customProperties = new Dictionary<string, string>();

    [FoldoutGroup("Common Methods")]
    [Button("Mine Rock", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public virtual void MineRock(Vector3 position)
    {
        Debug.Log($"{rockName} rock has been mined at {position}");

        // Instantiate Stone
        Item stoneItem = Instantiate(Resources.Load<Item>("Stone"));
        stoneItem.customProperties["stoneAmount"] = stoneYield.ToString();
        stoneItem.Drop(GetRandomOffset(position));

        string logMessage = $"Obtained {stoneYield} stone";

        // Check for special reward
        if (yieldsSpecialReward)
        {
            // Handle special rewards based on custom properties
            // For example, check if there's a specific reward type defined
            if (customProperties.TryGetValue("rewardType", out string rewardType))
            {
                // Instantiate the special reward based on the reward type
                // This can be expanded to handle different reward types
                Debug.Log($"Received a special reward: {rewardType}");
            }

            logMessage += " and received a special reward";
        }

        Debug.Log($"{logMessage} from mining a {rockName} at {position}");
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
