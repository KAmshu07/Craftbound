#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TreePlanter))]
public class TreePlacementTool : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TreePlanter treePlanter = (TreePlanter)target;

        if (GUILayout.Button("Place Trees"))
        {
            treePlanter.PlaceTrees();
        }
    }
}
#endif
