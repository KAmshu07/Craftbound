using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class MaterialQuickAccessWindow : EditorWindow
{
    private string materialSearchFilter = "";
    private Vector2 scrollPosition;
    private Dictionary<string, List<Material>> categorizedMaterials;
    private HashSet<string> collapsedCategories;
    private bool showPackageMaterials = true; // Toggle to show/hide package materials

    [MenuItem("Tools/Material Quick Access")]
    public static void ShowWindow()
    {
        var window = GetWindow<MaterialQuickAccessWindow>("Material Quick Access");
        window.Init();
    }

    private void Init()
    {
        categorizedMaterials = new Dictionary<string, List<Material>>();
        collapsedCategories = new HashSet<string>();

        var materialGUIDs = AssetDatabase.FindAssets("t:Material");
        foreach (var guid in materialGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!showPackageMaterials && path.StartsWith("Packages")) continue; // Skip package materials if the option is off

            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var category = Path.GetDirectoryName(path).Replace("Assets/", "");

            if (!categorizedMaterials.ContainsKey(category))
                categorizedMaterials[category] = new List<Material>();
            categorizedMaterials[category].Add(material);
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Material Quick Access", EditorStyles.boldLabel);

        // Toggle for showing package materials
        showPackageMaterials = EditorGUILayout.Toggle("Show Package Materials", showPackageMaterials);
        if (GUILayout.Button("Refresh List"))
        {
            Init(); // Refresh the list when toggling the option
        }

        materialSearchFilter = EditorGUILayout.TextField("Search", materialSearchFilter);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var category in categorizedMaterials.Keys)
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical("box");

            if (GUILayout.Button(category, EditorStyles.boldLabel))
            {
                if (collapsedCategories.Contains(category))
                    collapsedCategories.Remove(category);
                else
                    collapsedCategories.Add(category);
            }

            if (!collapsedCategories.Contains(category))
            {
                foreach (var material in categorizedMaterials[category].Where(m => string.IsNullOrEmpty(materialSearchFilter) || m.name.ToLower().Contains(materialSearchFilter.ToLower())))
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(material.name, GUILayout.Width(200));

                    if (GUILayout.Button("Locate", GUILayout.Width(60)))
                    {
                        EditorGUIUtility.PingObject(material);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
    }
}
