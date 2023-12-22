using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    private UIManager uiManager;
    private Dictionary<GameObject, bool> parentToggleStates = new Dictionary<GameObject, bool>();
    private bool showHierarchy = true;
    private int selectedCanvasIndex = 0;
    private int selectedElement = 0;

    private void OnEnable()
    {
        uiManager = (UIManager)target;
        if (uiManager == null)
        {
            Debug.LogError("UIManagerEditor: UIManager target not found.");
            return;
        }
    }

    public override void OnInspectorGUI()
    {
        if (uiManager == null)
        {
            EditorGUILayout.HelpBox("UIManager not found.", MessageType.Error);
            return;
        }

        GUILayout.Label("UI References", EditorStyles.boldLabel);

        ShowUIHierarchySection();
        ShowUIReferenceManagementSection();
        DrawDefaultInspector();
    }

    private void ShowUIHierarchySection()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("UI Hierarchy", EditorStyles.boldLabel);

        if (GUILayout.Button(showHierarchy ? "Hide Hierarchy" : "Show Hierarchy"))
            showHierarchy = !showHierarchy;

        if (showHierarchy)
        {
            DisplayCanvasDropdown();
            GameObject selectedCanvasObject = GetSelectedCanvas();
            if (selectedCanvasObject != null)
                DrawUIHierarchy(selectedCanvasObject, null);
            else
                EditorGUILayout.HelpBox("Selected canvas not found in the scene.", MessageType.Warning);
        }
        GUILayout.EndVertical();
    }

    private void ShowUIReferenceManagementSection()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("UI Reference Management", EditorStyles.boldLabel);

        if (uiManager.GetAllUICategories() == null)
        {
            EditorGUILayout.HelpBox("No UI categories found.", MessageType.Info);
            return;
        }

        foreach (var category in uiManager.GetAllUICategories())
        {
            GUILayout.Label(category.name, EditorStyles.boldLabel);
            foreach (var uiReference in category.references)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(uiReference.fullPath + " (" + uiReference.elementType.ToString() + ")", GUILayout.Width(200));
                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    uiManager.RemoveUIReference(category.name, uiReference.fullPath);
                    EditorUtility.SetDirty(uiManager);
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        GUILayout.Space(10);
        DisplayAddUIReferenceSection();
        GUILayout.EndVertical();
    }

    private void DisplayCanvasDropdown()
    {
        var canvases = GameObject.FindObjectsOfType<Canvas>();
        if (canvases.Length == 0)
        {
            EditorGUILayout.HelpBox("No canvases found in the scene.", MessageType.Info);
            return;
        }

        string[] sceneCanvasNames = canvases.Select(canvas => canvas.gameObject.name).ToArray();
        selectedCanvasIndex = EditorGUILayout.Popup("Select Canvas", selectedCanvasIndex, sceneCanvasNames);
    }

    private GameObject GetSelectedCanvas()
    {
        var canvases = GameObject.FindObjectsOfType<Canvas>();
        if (canvases.Length == 0 || selectedCanvasIndex >= canvases.Length)
        {
            return null;
        }

        return canvases[selectedCanvasIndex].gameObject;
    }

    private void DisplayAddUIReferenceSection()
    {
        GUILayout.Label("Add UI Reference", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        var sceneGameObjects = GameObject.FindObjectsOfType<GameObject>().Where(go => go.GetComponent<Canvas>() == null).ToArray();
        if (sceneGameObjects.Length == 0)
        {
            EditorGUILayout.HelpBox("No valid UI elements found in the scene.", MessageType.Info);
            EditorGUILayout.EndHorizontal();
            return;
        }

        string[] sceneHierarchyNames = sceneGameObjects.Select(go => go.name).ToArray();
        selectedElement = EditorGUILayout.Popup("Select Object", selectedElement, sceneHierarchyNames);

        string fullPath = EditorGUILayout.TextField("Full Path", "");
        if (GUILayout.Button("Add", GUILayout.Width(80)))
        {
            GameObject selectedObject = sceneGameObjects[selectedElement];
            if (selectedObject != null)
            {
                uiManager.AddUIReference(selectedObject);
                EditorUtility.SetDirty(uiManager);
                Repaint();
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawUIHierarchy(GameObject parent, Transform parentTransform)
    {
        if (parent == null)
        {
            EditorGUILayout.HelpBox("Parent GameObject is null.", MessageType.Warning);
            return;
        }

        foreach (Transform child in parent.transform)
        {
            EditorGUILayout.BeginHorizontal();
            string hierarchyName = parentTransform != null ? $"{parentTransform.name}/{child.name}" : child.name;

            if (child.childCount > 0)
            {
                bool isExpanded = false;
                parentToggleStates.TryGetValue(child.gameObject, out isExpanded);
                isExpanded = EditorGUILayout.ToggleLeft(hierarchyName, isExpanded, GUILayout.ExpandWidth(false));
                parentToggleStates[child.gameObject] = isExpanded;

                if (isExpanded)
                    EditorGUILayout.ObjectField(child.name, child.gameObject, typeof(GameObject), true);
            }
            else
                EditorGUILayout.ObjectField(hierarchyName, child.gameObject, typeof(GameObject), true);

            if (GUILayout.Button("Add", GUILayout.Width(80)))
            {
                uiManager.AddUIReference(child.gameObject);
                Repaint();
            }

            EditorGUILayout.EndHorizontal();
            if (child.childCount > 0 && parentToggleStates.ContainsKey(child.gameObject) && parentToggleStates[child.gameObject])
                DrawUIHierarchy(child.gameObject, child);
        }
    }
}
