using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages multiple timers, providing methods to start, pause, resume, and stop timers.
/// </summary>
public class TimerManager : MonoBehaviour
{
    private static TimerManager _instance;

    /// <summary>
    /// Gets the singleton instance of the TimerManager.
    /// </summary>
    public static TimerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("TimerManager");
                _instance = obj.AddComponent<TimerManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
    public bool EnableDebugLogging { get; set; } = false;

    /// <summary>
    /// Starts a new timer or restarts an existing timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="duration">Duration of the timer in seconds.</param>
    /// <param name="onTimerFinished">Action to be called when the timer finishes.</param>
    /// <param name="countUp">If true, the timer counts up. If false, the timer counts down.</param>
    /// <param name="onTimerTick">Action to be called at each tick interval with remaining time.</param>
    /// <param name="tickInterval">Interval in seconds for the tick action.</param>
    /// <param name="autoReset">If true, the timer will reset and start again after finishing.</param>
    /// <param name="useCompactFormat">If true, the timer will use compact time format. Otherwise, it will use detailed format.</param>
    public void StartTimer(string timerId, float duration, Action onTimerFinished, bool countUp = false, Action<float> onTimerTick = null, float tickInterval = 1.0f, bool autoReset = false, bool useCompactFormat = true)
    {
        if (timers.ContainsKey(timerId))
        {
            StopTimer(timerId);
        }

        Timer timer = new Timer(timerId, duration, onTimerFinished, countUp, onTimerTick, tickInterval, autoReset, RemoveTimer, useCompactFormat);
        timers[timerId] = timer;

        if (EnableDebugLogging)
        {
            Debug.Log($"Timer '{timerId}' started: Duration = {duration}, CountUp = {countUp}, AutoReset = {autoReset}, UseCompactFormat = {useCompactFormat}");
        }
    }

    /// <summary>
    /// Starts a new timer or restarts an existing timer with a scheduled task.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="duration">Duration of the timer in seconds.</param>
    /// <param name="scheduledTask">Action to be called when the scheduled task is executed.</param>
    /// <param name="recurringInterval">Interval for recurring tasks. Set to 0 for non-recurring tasks.</param>
    public void StartTimer(string timerId, float duration, Action scheduledTask, float recurringInterval = 0)
    {
        if (timers.ContainsKey(timerId))
        {
            StopTimer(timerId);
        }

        Timer timer = new Timer(timerId, duration, null, false, null, 1.0f, false, RemoveTimer, true, scheduledTask, recurringInterval);
        timers[timerId] = timer;

        if (EnableDebugLogging)
        {
            Debug.Log($"Timer '{timerId}' started for scheduling task: Duration = {duration}, RecurringInterval = {recurringInterval}");
        }
    }

    /// <summary>
    /// Pauses the specified timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    public void PauseTimer(string timerId)
    {
        if (timers.TryGetValue(timerId, out Timer timer))
        {
            timer.Pause();

            if (EnableDebugLogging)
            {
                Debug.Log($"Timer '{timerId}' paused.");
            }
        }
    }

    /// <summary>
    /// Resumes the specified paused timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    public void ResumeTimer(string timerId)
    {
        if (timers.TryGetValue(timerId, out Timer timer))
        {
            timer.Resume();

            if (EnableDebugLogging)
            {
                Debug.Log($"Timer '{timerId}' resumed.");
            }
        }
    }

    /// <summary>
    /// Stops the specified timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    public void StopTimer(string timerId)
    {
        if (timers.Remove(timerId) && EnableDebugLogging)
        {
            Debug.Log($"Timer '{timerId}' stopped.");
        }
    }

    /// <summary>
    /// Gets the elapsed time of the specified timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>Elapsed time in seconds.</returns>
    public float GetElapsedTime(string timerId)
    {
        return timers.TryGetValue(timerId, out Timer timer) ? timer.ElapsedTime : 0;
    }

    /// <summary>
    /// Checks if the specified timer is running.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>True if the timer is running, otherwise false.</returns>
    public bool IsTimerRunning(string timerId)
    {
        return timers.ContainsKey(timerId);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        foreach (var timer in new Dictionary<string, Timer>(timers))
        {
            timer.Value.Update(deltaTime);
        }
    }

    /// <summary>
    /// Gets a dictionary of active timers.
    /// </summary>
    /// <returns>Dictionary of active timers.</returns>
    public Dictionary<string, Timer> GetActiveTimers()
    {
        return new Dictionary<string, Timer>(timers);
    }

    /// <summary>
    /// Removes a timer from the manager.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    private void RemoveTimer(string timerId)
    {
        timers.Remove(timerId);
        if (EnableDebugLogging)
        {
            Debug.Log($"Timer '{timerId}' removed.");
        }
    }

    /// <summary>
    /// Gets the remaining time of the specified timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>Remaining time in seconds.</returns>
    public float GetRemainingTime(string timerId)
    {
        return timers.TryGetValue(timerId, out Timer timer) ? timer.RemainingTime : 0;
    }

    /// <summary>
    /// Checks if the specified timer counts up.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>True if the timer counts up, otherwise false.</returns>
    public bool IsCountUp(string timerId)
    {
        return timers.TryGetValue(timerId, out Timer timer) && timer.CountUp;
    }

    /// <summary>
    /// Checks if the specified timer uses compact format.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>True if the timer uses compact format, otherwise false.</returns>
    public bool UseCompactFormat(string timerId)
    {
        return timers.TryGetValue(timerId, out Timer timer) && timer.UseCompactFormat;
    }

    /// <summary>
    /// Updates the recurring interval for a specified timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="newInterval">The new interval for recurring tasks.</param>
    public void UpdateRecurringInterval(string timerId, float newInterval)
    {
        if (timers.TryGetValue(timerId, out Timer timer))
        {
            timer.UpdateRecurringInterval(newInterval);

            if (EnableDebugLogging)
            {
                Debug.Log($"Timer '{timerId}' recurring interval updated to {newInterval} seconds.");
            }
        }
    }

    /// <summary>
    /// Updates the scheduled task for a specified timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="newTask">The new task to be executed.</param>
    public void UpdateScheduledTask(string timerId, Action newTask)
    {
        if (timers.TryGetValue(timerId, out Timer timer))
        {
            timer.UpdateScheduledTask(newTask);

            if (EnableDebugLogging)
            {
                Debug.Log($"Timer '{timerId}' scheduled task updated.");
            }
        }
    }
}
