using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class TextureQuickAccessWindow : EditorWindow
{
    private string textureSearchFilter = "";
    private Vector2 scrollPosition;
    private Dictionary<string, List<Texture2D>> categorizedTextures;
    private Dictionary<string, Texture2D> texturePreviews;
    private HashSet<string> collapsedCategories;
    private bool showPackageTextures = true; // Toggle to show/hide package textures

    [MenuItem("Tools/Texture Quick Access")]
    public static void ShowWindow()
    {
        var window = GetWindow<TextureQuickAccessWindow>("Texture Quick Access");
        window.Init();
    }

    private void Init()
    {
        categorizedTextures = new Dictionary<string, List<Texture2D>>();
        texturePreviews = new Dictionary<string, Texture2D>();
        collapsedCategories = new HashSet<string>();

        var textureGUIDs = AssetDatabase.FindAssets("t:Texture2D");
        foreach (var guid in textureGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!showPackageTextures && path.StartsWith("Packages")) continue; // Skip package textures if the option is off

            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            var category = Path.GetDirectoryName(path).Replace("Assets/", "");

            if (!categorizedTextures.ContainsKey(category))
                categorizedTextures[category] = new List<Texture2D>();
            categorizedTextures[category].Add(texture);

            texturePreviews[texture.name] = texture;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Texture Quick Access", EditorStyles.boldLabel);

        // Toggle for showing package textures
        showPackageTextures = EditorGUILayout.Toggle("Show Package Textures", showPackageTextures);
        if (GUILayout.Button("Refresh List"))
        {
            Init(); // Refresh the list when toggling the option
        }

        textureSearchFilter = EditorGUILayout.TextField("Search", textureSearchFilter);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var category in categorizedTextures.Keys)
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
                foreach (var texture in categorizedTextures[category].Where(t => string.IsNullOrEmpty(textureSearchFilter) || t.name.ToLower().Contains(textureSearchFilter.ToLower())))
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(texturePreviews.ContainsKey(texture.name) ? texturePreviews[texture.name] : null, GUILayout.Width(50), GUILayout.Height(50));
                    GUILayout.Label(texture.name, GUILayout.Width(200));

                    if (GUILayout.Button("Locate", GUILayout.Width(60)))
                    {
                        EditorGUIUtility.PingObject(texture);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
    }
}
