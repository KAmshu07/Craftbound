using UnityEngine;

public class TreePlanter : MonoBehaviour
{
    public GameObject treePrefab;
    public int treeDensity = 100;

    public void PlaceTrees()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < treeDensity; i++)
        {
            float x = Random.Range(0f, terrainData.size.x);
            float z = Random.Range(0f, terrainData.size.z);

            float y = terrain.SampleHeight(new Vector3(x, 0f, z));

            Vector3 treePosition = new Vector3(x, y, z);

            GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity);
            tree.transform.parent = terrain.transform;

            // Attach HealthComponent and CuttingTree scripts
            HealthComponent healthComponent = tree.AddComponent<HealthComponent>();
            CuttingTree cuttingTree = tree.AddComponent<CuttingTree>();

            // Adjust other properties as needed
            healthComponent.maxHealth = 100; // Set maximum health
        }
    }
}
