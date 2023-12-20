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
    private Dictionary<string, bool> categoryFoldoutStates = new Dictionary<string, bool>();
    private GameObject currentDraggedPrefab;

    // Define a more extensive color palette
    private Color buttonNormalColor = Color.red;  
    private Color buttonHoverColor = Color.green; 
    private Color buttonActiveColor = Color.blue; 

    [MenuItem("Tools/Prefab Quick Access")]
    public static void ShowWindow()
    {
        var window = GetWindow<PrefabQuickAccessWindow>("Prefab Quick Access");
        window.Init();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
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
        // Label for the window title with red text color
        GUILayout.Label("Prefab Quick Access", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.green }
        });

        // Text field for search
        prefabSearchFilter = EditorGUILayout.TextField("Search", prefabSearchFilter, EditorStyles.textField);

        // Begin a scrolling view
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        // Loop through each category in the categorizedPrefabs dictionary
        foreach (var category in categorizedPrefabs.Keys)
        {
            if (!categoryFoldoutStates.ContainsKey(category))
                categoryFoldoutStates[category] = true; // Default state

            GUILayout.Space(5);

            // Custom foldout style with red text color
            var foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                normal = { textColor = Color.cyan },
                onNormal = { textColor = Color.blue },
                active = { background = EditorGUIUtility.whiteTexture },
                onActive = { background = EditorGUIUtility.whiteTexture },
                padding = new RectOffset(10, 10, 5, 5)
            };

            // Draw the category header
            categoryFoldoutStates[category] = EditorGUILayout.Foldout(categoryFoldoutStates[category], category, true, foldoutStyle);

            // Display prefabs in the category if the foldout is expanded
            if (categoryFoldoutStates[category])
            {
                DisplayPrefabsInCategory(category);
            }
        }

        // End the scrolling view
        GUILayout.EndScrollView();

        // Repaint the window if previews are loading
        if (AssetPreview.IsLoadingAssetPreviews())
        {
            Repaint();
        }
    }

    private void DisplayPrefabsInCategory(string category)
    {
        foreach (var prefab in categorizedPrefabs[category].Where(p => string.IsNullOrEmpty(prefabSearchFilter) || p.name.ToLower().Contains(prefabSearchFilter.ToLower())))
        {
            EditorGUILayout.BeginHorizontal();

            // Left Column for Preview
            GUILayout.BeginVertical();
            if (prefabPreviews.ContainsKey(prefab.name))
            {
                var previewTexture = prefabPreviews[prefab.name];
                Rect previewRect = GUILayoutUtility.GetRect(120, 120); // Fixed size for prefab previews
                if (Event.current.type == EventType.Repaint)
                {
                    GUI.DrawTexture(previewRect, previewTexture, ScaleMode.ScaleToFit);
                }

                HandlePrefabPreviewDrag(previewRect, prefab);
            }
            else
            {
                GUILayout.Label("No Preview", GUILayout.Width(70), GUILayout.Height(70));
            }
            GUILayout.EndVertical();

            // Right Column for Buttons
            DisplayPrefabButtons(prefab);

            EditorGUILayout.EndHorizontal();
        }
    }



    private void HandlePrefabPreviewDrag(Rect previewRect, GameObject prefab)
    {
        if (Event.current.type == EventType.MouseDown && previewRect.Contains(Event.current.mousePosition))
        {
            currentDraggedPrefab = prefab;
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.objectReferences = new[] { prefab };
            DragAndDrop.StartDrag(prefab.name);
            Event.current.Use();
        }
    }

    private void DisplayPrefabButtons(GameObject prefab)
    {

        var buttonStyle = new GUIStyle(GUI.skin.button)
        {
            margin = new RectOffset(4, 4, 4, 4),
            normal = { background = EditorGUIUtility.whiteTexture, textColor = buttonNormalColor },
            hover = { background = EditorGUIUtility.whiteTexture, textColor = buttonHoverColor },
            active = { background = EditorGUIUtility.whiteTexture, textColor = buttonActiveColor }
        };

        if (GUILayout.Button(prefab.name, buttonStyle, GUILayout.Width(120), GUILayout.Height(20)))
        {
            PrefabUtility.InstantiatePrefab(prefab);
        }

        if (GUILayout.Button("Locate", buttonStyle, GUILayout.Width(60), GUILayout.Height(20)))
        {
            EditorGUIUtility.PingObject(prefab);
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (DragAndDrop.visualMode == DragAndDropVisualMode.Copy)
        {
            Event e = Event.current;
            if (e.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                e.Use();
            }
            else if (e.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (Object dragged_object in DragAndDrop.objectReferences)
                {
                    GameObject go = PrefabUtility.InstantiatePrefab(dragged_object) as GameObject;
                    if (go != null)
                    {
                        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            go.transform.position = hit.point;
                        }
                    }
                }
                e.Use();
            }
        }
    }
}
