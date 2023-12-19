using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System.Linq;

public class SceneQuickAccessWindow : EditorWindow
{
    private string sceneSearchFilter = "";
    private Vector2 scrollPosition;

    [MenuItem("Tools/Scene Quick Access")]
    public static void ShowWindow()
    {
        GetWindow<SceneQuickAccessWindow>("Scene Quick Access");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene Quick Access", EditorStyles.boldLabel);

        sceneSearchFilter = EditorGUILayout.TextField("Search", sceneSearchFilter);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        var scenes = AssetDatabase.FindAssets("t:Scene")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(scenePath => string.IsNullOrEmpty(sceneSearchFilter) || Path.GetFileNameWithoutExtension(scenePath).ToLower().Contains(sceneSearchFilter.ToLower()))
            .OrderBy(scenePath => scenePath);

        foreach (var scenePath in scenes)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(sceneName, GUILayout.Width(200));
            if (GUILayout.Button("Open", GUILayout.Width(100)))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
            if (GUILayout.Button("Locate", GUILayout.Width(100)))
            {
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(scenePath));
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}
