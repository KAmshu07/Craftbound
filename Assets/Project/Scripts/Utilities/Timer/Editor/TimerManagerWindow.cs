using UnityEditor;
using UnityEngine;

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
                    Timer.PauseTimer(timer.Key);
                }

                if (GUILayout.Button("Resume"))
                {
                    Timer.ResumeTimer(timer.Key);
                }

                if (GUILayout.Button("Stop"))
                {
                    Timer.StopTimer(timer.Key);
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
