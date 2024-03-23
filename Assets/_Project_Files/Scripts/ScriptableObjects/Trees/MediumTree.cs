using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewMediumTree", menuName = "Tree Type/Medium Tree")]
public class MediumTree : Tree
{
    [FoldoutGroup("Medium Tree Properties")]
    [LabelWidth(90)]
    public int woodYield = 10;

    [FoldoutGroup("Medium Tree Properties")]
    [LabelWidth(90)]
    [InfoBox("Set to true if the Medium Tree should yield resin when cut.")]
    public bool yieldsResin;

    [FoldoutGroup("Medium Tree Properties")]
    [ShowIf("yieldsResin")]
    [LabelWidth(90)]
    public int resinYield = 3;

    [FoldoutGroup("Medium Tree Methods")]
    [Button("Cut Medium Tree", ButtonSizes.Large)]
    [GUIColor(1, 0.8f, 0.8f)]
    public override void CutTree(Vector3 position)
    {
        base.CutTree(position);

        // Instantiate Wood
        Wood woodItem = Instantiate(Resources.Load<Wood>("Wood"));
        woodItem.woodAmount = woodYield;
        woodItem.Drop(GetRandomOffset(position));

        string logMessage = $"Obtained {woodYield} wood";

        if (yieldsResin)
        {
            // Instantiate Resin
            Resin resinItem = Instantiate(Resources.Load<Resin>("Resin"));
            resinItem.resinAmount = resinYield;
            resinItem.Drop(GetRandomOffset(position));

            logMessage += $" and {resinYield} resin";
        }

        Debug.Log($"{logMessage} from cutting a MediumTree at {position}");
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