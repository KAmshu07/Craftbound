using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent), editorForChildClasses: true)]
public class EventEditor : Editor
{
    private int eventID = 0; // Added an integer field to hold the event ID

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEvent e = target as GameEvent;

        EditorGUILayout.Space();

        // Create an integer field for Event ID
        eventID = EditorGUILayout.IntField("Event ID", eventID);

        if (GUILayout.Button("Raise Event"))
        {
            e.Raise(eventID); // Pass the event ID as an int when raising the event
            EditorUtility.SetDirty(e);
        }
    }
}
