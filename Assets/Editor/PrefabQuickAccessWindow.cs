using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class PrefabQuickAccessWindow : EditorWindow
{
    private string prefabSearchFilter = "";
    private Vector2 scrollPosition;
    private Dictionary<string, List<GameObject>> categorizedPrefabs;
    private Dictionary<string, Texture2D> prefabPreviews;

    [MenuItem("Tools/Prefab Quick Access")]
    public static void ShowWindow()
    {
        var window = GetWindow<PrefabQuickAccessWindow>("Prefab Quick Access");
        window.Init();
    }

    private void Init()
    {
        categorizedPrefabs = new Dictionary<string, List<GameObject>>();
        prefabPreviews = new Dictionary<string, Texture2D>();

        var prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        foreach (var guid in prefabGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var category = Path.GetDirectoryName(path).Replace("Assets/", "");

            if (!categorizedPrefabs.ContainsKey(category))
                categorizedPrefabs[category] = new List<GameObject>();
            categorizedPrefabs[category].Add(prefab);

            var preview = AssetPreview.GetAssetPreview(prefab);
            if (preview != null)
                prefabPreviews[prefab.name] = preview;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Quick Access", EditorStyles.boldLabel);
        prefabSearchFilter = EditorGUILayout.TextField("Search", prefabSearchFilter);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var category in categorizedPrefabs.Keys)
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label(category, EditorStyles.boldLabel);

            foreach (var prefab in categorizedPrefabs[category].Where(p => string.IsNullOrEmpty(prefabSearchFilter) || p.name.ToLower().Contains(prefabSearchFilter.ToLower())))
            {
                EditorGUILayout.BeginHorizontal();
                if (prefabPreviews.ContainsKey(prefab.name))
                    GUILayout.Label(prefabPreviews[prefab.name], GUILayout.Width(50), GUILayout.Height(50));
                else
                    GUILayout.Label("No Preview", GUILayout.Width(50), GUILayout.Height(50));

                if (GUILayout.Button(prefab.name, GUILayout.ExpandWidth(false)))
                {
                    PrefabUtility.InstantiatePrefab(prefab);
                }

                // Locate button
                if (GUILayout.Button("Locate", GUILayout.Width(60)))
                {
                    EditorGUIUtility.PingObject(prefab);
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        GUILayout.EndScrollView();

        if (AssetPreview.IsLoadingAssetPreviews())
        {
            Repaint();
        }
    }
}
