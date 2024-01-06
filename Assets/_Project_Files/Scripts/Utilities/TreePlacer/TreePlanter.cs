using UnityEngine;
using Sirenix.OdinInspector;

public class TreePlanter : MonoBehaviour
{
    [SerializeField]
    [Required]
    [InfoBox("Assign different tree types here.")]
    private Tree[] treeTypes;

    [SerializeField]
    [MinValue(1)]
    [LabelWidth(150)]
    [Tooltip("Number of trees to be planted.")]
    private int treeDensity = 100;

    private Tree treeType;

    private void SetTreeType(Tree selectedTree)
    {
        if (selectedTree != null)
        {
            treeType = selectedTree;
            Debug.Log($"Tree type set to: {treeType.typeName}");
        }
        else
        {
            Debug.LogError("Select a TreeBase scriptable object to set as the tree type.");
        }
    }

    [Button("Place Trees", ButtonSizes.Large)]
    [GUIColor(0.8f, 1, 0.8f)]
    private void PlaceTrees()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < treeDensity; i++)
        {
            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);

            float y = terrain.SampleHeight(new Vector3(x, 0f, z));

            Vector3 treePosition = new Vector3(x, y, z);

            // Randomly choose a tree type
            Tree treeType = treeTypes[Random.Range(0, treeTypes.Length)];

            GameObject tree = Instantiate(treeType.treePrefab, treePosition, Quaternion.identity);
            tree.transform.parent = terrain.transform;

            // Attach HealthComponent only if not already present
            HealthComponent healthComponent = tree.GetComponent<HealthComponent>();
            if (healthComponent == null)
            {
                healthComponent = tree.AddComponent<HealthComponent>();
                healthComponent.maxHealth = treeType.baseHealth;
                healthComponent.currentHealth = healthComponent.maxHealth;
            }

            // Attach CuttingTree only if not already present
            CuttingTree cuttingTree = tree.GetComponent<CuttingTree>();
            if (cuttingTree == null)
            {
                cuttingTree = tree.AddComponent<CuttingTree>();
                SetTreeType(treeType);
                cuttingTree.cutDamage = treeType.cutDamage;
                cuttingTree.tree = treeType;
            }
        }
    }




    [Button("Remove All Trees", ButtonSizes.Large)]
    [GUIColor(1, 0.8f, 0.8f)]
    private void RemoveAllTrees()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");

        foreach (var tree in trees)
        {
            DestroyImmediate(tree);
        }
    }
}
