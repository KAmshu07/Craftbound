using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor window for managing timers in Play mode.
/// </summary>
public class TimerManagerWindow : EditorWindow
{
    [MenuItem("Tools/Timer Manager")]
    public static void ShowWindow()
    {
        GetWindow<TimerManagerWindow>("Timer Manager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Active Timers", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (Application.isPlaying)
        {
            EditorGUILayout.BeginVertical("box");

            foreach (var timer in TimerManager.Instance.GetActiveTimers())
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.LabelField("Timer ID:", timer.Key);
                EditorGUILayout.LabelField("Remaining Time:", timer.Value.RemainingTime.ToString("F2") + "s");
                EditorGUILayout.LabelField("Elapsed Time:", timer.Value.ElapsedTime.ToString("F2") + "s");
                EditorGUILayout.LabelField("Is Paused:", timer.Value.IsPaused.ToString());

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Pause"))
                {
                    TimerUtility.PauseTimer(timer.Key);
                }

                if (GUILayout.Button("Resume"))
                {
                    TimerUtility.ResumeTimer(timer.Key);
                }

                if (GUILayout.Button("Stop"))
                {
                    TimerUtility.StopTimer(timer.Key);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();
        }
        else
        {
            GUILayout.Label("Timer management is only available in Play mode.");
        }
    }
}
