using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewSmallTree", menuName = "Tree Type/Small Tree")]
public class SmallTree : Tree
{
    [FoldoutGroup("Small Tree Properties")]
    [LabelWidth(80)]
    public int woodYield = 5;

    [FoldoutGroup("Small Tree Properties")]
    [LabelWidth(80)]
    [InfoBox("Set to true if the Small Tree should yield fibre when cut.")]
    public bool yieldsFibre;

    [FoldoutGroup("Small Tree Properties")]
    [ShowIf("yieldsFibre")]
    [LabelWidth(80)]
    public int fibreYield = 2;

    [FoldoutGroup("Small Tree Methods")]
    [Button("Cut Small Tree", ButtonSizes.Large)]
    [GUIColor(0.8f, 0.8f, 1)]
    public override void CutTree(Vector3 position)
    {
        base.CutTree(position);

        // Instantiate Wood
        Wood woodItem = Instantiate(Resources.Load<Wood>("Wood"));
        woodItem.woodAmount = woodYield;
        woodItem.Drop(GetRandomOffset(position));

        string logMessage = $"Obtained {woodYield} wood";

        if (yieldsFibre && fibreYield > 0)
        {
            // Instantiate Fibre
            Fibre fibreItem = Instantiate(Resources.Load<Fibre>("Fibre"));
            fibreItem.fibreAmount = fibreYield;
            fibreItem.Drop(GetRandomOffset(position));

            logMessage += $" and {fibreYield} fibre";
        }

        Debug.Log($"{logMessage} from cutting a SmallTree at {position}");
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
