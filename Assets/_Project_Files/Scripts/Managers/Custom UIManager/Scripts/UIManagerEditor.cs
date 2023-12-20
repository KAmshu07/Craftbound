using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    private UIManager uiManager;
    private Dictionary<GameObject, bool> parentToggleStates = new Dictionary<GameObject, bool>();
    private bool showHierarchy = true;
    private int selectedCanvasIndex = 0; // Index for selecting a canvas from the dropdown
    private int selectedElement = 0; // Index for selecting an object from the scene hierarchy

    #region ____________________UI Hierarchy Section____________________

    private void OnEnable()
    {
        uiManager = (UIManager)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("UI References", EditorStyles.boldLabel);

        // UI Hierarchy Section
        GUILayout.BeginVertical("box");
        GUILayout.Label("UI Hierarchy", EditorStyles.boldLabel);

        // Toggle button to show/hide hierarchy
        if (GUILayout.Button(showHierarchy ? "Hide Hierarchy" : "Show Hierarchy"))
        {
            showHierarchy = !showHierarchy;
        }
        if (showHierarchy)
        {
            // Allow the user to select a canvas from the dropdown
            string[] sceneCanvasNames = GameObject.FindObjectsOfType<Canvas>()
                .Select(canvas => canvas.gameObject.name)
                .ToArray();
            selectedCanvasIndex = EditorGUILayout.Popup("Select Canvas", selectedCanvasIndex, sceneCanvasNames);

            // Get the selected canvas object
            GameObject selectedCanvasObject = GameObject.Find(sceneCanvasNames[selectedCanvasIndex]);

            if (selectedCanvasObject != null)
            {
                DrawUIHierarchy(selectedCanvasObject, null);
            }
        }
        GUILayout.EndVertical();


        // UI Reference Management Section
        GUILayout.BeginVertical("box");
        GUILayout.Label("UI Reference Management", EditorStyles.boldLabel);

        foreach (var category in uiManager.GetAllUICategories())
        {
            GUILayout.Label(category.name, EditorStyles.boldLabel);
            foreach (var uiReference in category.references)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(uiReference.name + " (" + uiReference.elementType.ToString() + ")", GUILayout.Width(200));
                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    uiManager.RemoveUIReference(category.name, uiReference.name);
                    return; // Refresh the UI
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(10);

        // Add UI Reference Section
        GUILayout.Label("Add UI Reference", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        // Dropdown to select an object from the scene hierarchy
        GameObject[] sceneGameObjects = GameObject.FindObjectsOfType<GameObject>()
            .Where(go => go.GetComponent<Canvas>() == null) // Exclude canvases
            .ToArray();
        string[] sceneHierarchyNames = sceneGameObjects
            .Select(go => go.name)
            .ToArray();
        selectedElement = EditorGUILayout.Popup("Select Object", selectedElement, sceneHierarchyNames);

        if (GUILayout.Button("Add", GUILayout.Width(80)))
        {
            string selectedName = sceneHierarchyNames[selectedElement];
            GameObject selectedObject = sceneGameObjects[selectedElement];
            if (selectedObject != null)
            {
                uiManager.AddUIReference(selectedName, selectedObject);
                Repaint(); // Refresh the UI
            }
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.EndVertical();

        // Default Inspector
        DrawDefaultInspector();
    }

    #endregion ____________________UI Hierarchy Section____________________

    #region ____________________Recursively draw the detailed UI hierarchy with parent-child relation____________________

    private void DrawUIHierarchy(GameObject parent, Transform parentTransform)
    {
        foreach (Transform child in parent.transform)
        {
            EditorGUILayout.BeginHorizontal();

            // Show parent-child relation
            string hierarchyName = (parentTransform != null)
                ? $"{parentTransform.name}/{child.name}"
                : child.name;

            // Toggle button for showing/hiding child objects
            if (child.childCount > 0)
            {
                bool isExpanded = false;    
                parentToggleStates.TryGetValue(child.gameObject, out isExpanded);
                isExpanded = EditorGUILayout.ToggleLeft(hierarchyName, isExpanded, GUILayout.ExpandWidth(false));
                parentToggleStates[child.gameObject] = isExpanded;

                if (isExpanded)
                {
                    // Real-time preview of UI element
                    EditorGUILayout.ObjectField(child.name, child.gameObject, typeof(GameObject), true);
                }
            }
            else
            {
                // Real-time preview of UI element for leaf nodes
                EditorGUILayout.ObjectField(hierarchyName, child.gameObject, typeof(GameObject), true);
            }

            if (GUILayout.Button("Add", GUILayout.Width(80)))
            {
                // Use the updated AddUIReference method that categorizes based on element type
                uiManager.AddUIReference(child.name, child.gameObject);
                Repaint(); // Refresh the UI
            }


            EditorGUILayout.EndHorizontal();

            if (child.childCount > 0 && parentToggleStates.ContainsKey(child.gameObject) && parentToggleStates[child.gameObject])
            {
                DrawUIHierarchy(child.gameObject, child);
            }
        }
    }

    #endregion ____________________Recursively draw the detailed UI hierarchy with parent-child relation____________________
}
