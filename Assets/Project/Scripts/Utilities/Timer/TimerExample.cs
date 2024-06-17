using UnityEngine;

/// <summary>
/// Example usage of the Timer and TimerManager classes.
/// </summary>
public class TimerExample : MonoBehaviour
{
    private void Start()
    {
        // Enable debug logging
        TimerUtility.EnableDebugLogging(true);

        // Start a simple countdown timer for 5 seconds with compact format (default)
        TimerUtility.StartTimer("SimpleCountdown", 5.0f, SimpleCountdownFinished);

        // Start a countdown timer for 5 seconds with a tick interval and compact format (default)
        TimerUtility.StartTimer("CountdownWithTick", 5.0f, CountdownWithTickFinished, CountdownWithTickTick, 1.0f);

        // Start a count-up timer for 5 seconds with full customization and detailed format
        TimerUtility.StartTimer("FullCustomTimer", 5.0f, FullCustomTimerFinished, countUp: true, onTimerTick: FullCustomTimerTick, tickInterval: 0.5f, autoReset: true, useCompactFormat: false);

        // Schedule a task to be executed after 5 seconds
        TimerUtility.ScheduleTask("ScheduledTask", 5.0f, ScheduledTaskExecuted);

        // Schedule a recurring task to be executed every 3 seconds
        TimerUtility.ScheduleTask("RecurringTask", 3.0f, RecurringTaskExecuted, 3.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pausing SimpleCountdown");
            TimerUtility.PauseTimer("SimpleCountdown");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Resuming SimpleCountdown");
            TimerUtility.ResumeTimer("SimpleCountdown");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Stopping SimpleCountdown");
            TimerUtility.StopTimer("SimpleCountdown");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            float elapsedTime = TimerUtility.GetElapsedTime("FullCustomTimer");
            Debug.Log("Elapsed Time of FullCustomTimer: " + elapsedTime);
        }
    }

    // Callbacks for SimpleCountdown timer
    private void SimpleCountdownFinished()
    {
        Debug.Log("SimpleCountdown Finished!");
    }

    // Callbacks for CountdownWithTick timer
    private void CountdownWithTickFinished()
    {
        Debug.Log("CountdownWithTick Finished!");
    }

    private void CountdownWithTickTick(float remainingTime)
    {
        Debug.Log("CountdownWithTick Remaining Time: " + remainingTime);
    }

    // Callbacks for FullCustomTimer timer
    private void FullCustomTimerFinished()
    {
        Debug.Log("FullCustomTimer Finished!");
    }

    private void FullCustomTimerTick(float remainingTime)
    {
        Debug.Log("FullCustomTimer Tick: Remaining Time: " + remainingTime);
    }

    // Callback for ScheduledTask
    private void ScheduledTaskExecuted()
    {
        Debug.Log("ScheduledTask executed after 5 seconds");
    }

    // Callback for RecurringTask
    private void RecurringTaskExecuted()
    {
        Debug.Log("Recurring task executed every 3 seconds");
    }
}
