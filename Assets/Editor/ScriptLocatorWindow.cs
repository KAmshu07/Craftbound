using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ScriptLocatorWindow : EditorWindow
{
    private MonoScript scriptToFind;
    private List<(string sceneName, GameObject obj)> foundObjects;
    private Vector2 scrollPosition;
    private bool isFinding;

    [MenuItem("Tools/Script Locator")]
    public static void ShowWindow()
    {
        GetWindow<ScriptLocatorWindow>("Script Locator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Script Locator", EditorStyles.boldLabel);

        scriptToFind = EditorGUILayout.ObjectField("Select Script:", scriptToFind, typeof(MonoScript), false) as MonoScript;

        GUI.enabled = scriptToFind != null;
        if (GUILayout.Button(isFinding ? "Finding..." : "Find Objects", GUILayout.Height(30)))
        {
            if (!isFinding)
            {
                FindObjectsUsingScript();
            }
        }
        GUI.enabled = true;

        // Display found objects
        GUILayout.Space(10);
        GUILayout.Label("Found Objects:", EditorStyles.boldLabel);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        if (foundObjects != null)
        {
            if (foundObjects.Count == 0)
            {
                EditorGUILayout.LabelField("No objects are using this script in this scene.");
            }
            else
            {
                foreach (var (sceneName, obj) in foundObjects)
                {
                    EditorGUILayout.BeginVertical("box");

                    if (obj == null)
                    {
                        GUIStyle missingStyle = new GUIStyle(EditorStyles.boldLabel);
                        missingStyle.normal.textColor = Color.red;

                        EditorGUILayout.LabelField("Missing Object", missingStyle);
                    }
                    else
                    {
                        GUIStyle objectStyle = new GUIStyle(GUI.skin.button);
                        objectStyle.alignment = TextAnchor.MiddleLeft;
                        objectStyle.fixedHeight = 25;

                        string label = obj.name;

                        if (!string.IsNullOrEmpty(sceneName))
                        {
                            label += " (Scene: " + sceneName + ")";
                        }

                        if (GUILayout.Button(label, objectStyle))
                        {
                            EditorGUIUtility.PingObject(obj);
                        }
                    }

                    EditorGUILayout.EndVertical();
                }
            }
        }

        GUILayout.EndScrollView();
    }

    private void FindObjectsUsingScript()
    {
        foundObjects = new List<(string sceneName, GameObject obj)>();
        isFinding = true;

        // Find objects in all scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (!scene.isLoaded)
            {
                // Handle unloaded scenes
                foundObjects.Add((scene.name, null));
                continue;
            }

            var sceneObjects = scene.GetRootGameObjects();

            foreach (var obj in sceneObjects)
            {
                SearchForObjectWithScript(obj, scene.name);
            }
        }

        isFinding = false;
    }

    private void SearchForObjectWithScript(GameObject obj, string sceneName)
    {
        var components = obj.GetComponents<Component>();

        foreach (var component in components)
        {
            if (component != null && MonoScript.FromMonoBehaviour(component as MonoBehaviour) == scriptToFind)
            {
                foundObjects.Add((sceneName, obj));
                return;
            }
        }

        // Recursively search in child objects
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SearchForObjectWithScript(obj.transform.GetChild(i).gameObject, sceneName);
        }
    }

}
