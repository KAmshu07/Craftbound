using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

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

    [BoxGroup("Common Properties")]
    [LabelWidth(70)]
    public int woodYield = 5;

    [Title("Custom Properties")]
    [DictionaryDrawerSettings(KeyLabel = "Property", ValueLabel = "Value")]
    public Dictionary<string, string> customProperties = new Dictionary<string, string>();

    [FoldoutGroup("Common Methods")]
    [Button("Cut Tree", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public virtual void CutTree(Vector3 position)
    {
        Debug.Log($"{treeName} tree has been cut at {position}");

        // Instantiate Wood
        Item woodItem = Instantiate(Resources.Load<Item>("Wood"));
        woodItem.customProperties["woodAmount"] = woodYield.ToString();
        woodItem.Drop(GetRandomOffset(position));

        string logMessage = $"Obtained {woodYield} wood";

        // Check for special rewards based on custom properties
        // For example, check if the tree yields fibre
        if (customProperties.TryGetValue("fibreYield", out string fibreYieldStr))
        {
            int fibreYield = int.Parse(fibreYieldStr);
            Item fibreItem = Instantiate(Resources.Load<Item>("Fibre"));
            fibreItem.customProperties["fibreAmount"] = fibreYield.ToString();
            fibreItem.Drop(GetRandomOffset(position));
            logMessage += $" and {fibreYield} fibre";
        }

        // Add more checks for other rewards as needed

        Debug.Log($"{logMessage} from cutting a {treeName} at {position}");
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
