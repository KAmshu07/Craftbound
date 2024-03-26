using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewBigTree", menuName = "Tree Type/Big Tree")]
public class BigTree : Tree
{
    [FoldoutGroup("Big Tree Properties")]
    [LabelWidth(80)]
    public int woodYield = 20;

    [FoldoutGroup("Big Tree Properties")]
    [LabelWidth(100)]
    [InfoBox("Set to true if the Big Tree should yield Fruits when cut.")]
    public bool dropsFruits;

    [FoldoutGroup("Big Tree Properties")]
    [ShowIf("dropsFruits")]
    [LabelWidth(90)]
    public int fruitYield = 1;

    [FoldoutGroup("Big Tree Methods")]
    [Button("Cut Big Tree", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    public override void CutTree(Vector3 position)
    {
        base.CutTree(position);

        // Instantiate Wood
        Wood woodItem = Instantiate(Resources.Load<Wood>("Wood"));
        woodItem.woodAmount = woodYield;
        woodItem.Drop(GetRandomOffset(position));

        string logMessage = $"Obtained {woodYield} wood";

        if (dropsFruits)
        {
            // Instantiate Fruits
            Fruit fruitItem = Instantiate(Resources.Load<Fruit>("Fruit"));
            fruitItem.fruitAmount = fruitYield;
            fruitItem.Drop(GetRandomOffset(position));

            logMessage += $" and found some fruits";
        }

        Debug.Log($"{logMessage} from cutting a BigTree at {position}");
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