using UnityEditor;
using UnityEngine;

public class ObjectPoolerWindow : EditorWindow
{
    [MenuItem("Tools/Object Pooler")]
    public static void ShowWindow()
    {
        GetWindow<ObjectPoolerWindow>("Object Pooler");
    }

    private void OnGUI()
    {
        GUILayout.Label("Active Object Pools", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (Application.isPlaying)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Pools Overview", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            foreach (var pool in ObjectPooler.Instance.GetActivePools())
            {
                DrawPoolInfo(pool.Key, pool.Value);
            }

            EditorGUILayout.EndVertical();
        }
        else
        {
            GUILayout.Label("Pool management is only available in Play mode.", EditorStyles.helpBox);
        }
    }

    private void DrawPoolInfo(string poolTag, ObjectPooler.Pool pool)
    {
        EditorGUILayout.BeginVertical("box");

        // Header
        EditorGUILayout.LabelField(poolTag, EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Pool details
        EditorGUILayout.LabelField("Prefab:", pool.prefab.name);
        EditorGUILayout.LabelField("Size:", pool.size.ToString());
        EditorGUILayout.LabelField("Active Objects:", pool.activeCount.ToString());

        // Expand Pool button
        if (GUILayout.Button("Expand Pool", GUILayout.Width(200)))
        {
            ObjectPooler.Instance.ExpandPool(poolTag, 5); // Expands the pool by 5 objects
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }
}
