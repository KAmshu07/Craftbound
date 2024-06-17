using System;
using UnityEngine;

/// <summary>
/// Provides a simplified static interface for interacting with the TimerManager.
/// </summary>
public static class TimerUtility
{
    /// <summary>
    /// Starts a simple countdown timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="duration">Duration of the timer in seconds.</param>
    /// <param name="onTimerFinished">Action to be called when the timer finishes.</param>
    /// <param name="useCompactFormat">If true, the timer will use compact time format. Otherwise, it will use detailed format.</param>
    public static void StartTimer(string timerId, float duration, Action onTimerFinished, bool useCompactFormat = true) =>
        TimerManager.Instance.StartTimer(timerId, duration, onTimerFinished, false, null, 1.0f, false, useCompactFormat);

    /// <summary>
    /// Starts a countdown timer with a tick interval.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="duration">Duration of the timer in seconds.</param>
    /// <param name="onTimerFinished">Action to be called when the timer finishes.</param>
    /// <param name="onTimerTick">Action to be called at each tick interval with remaining time.</param>
    /// <param name="tickInterval">Interval in seconds for the tick action.</param>
    /// <param name="useCompactFormat">If true, the timer will use compact time format. Otherwise, it will use detailed format.</param>
    public static void StartTimer(string timerId, float duration, Action onTimerFinished, Action<float> onTimerTick, float tickInterval = 1.0f, bool useCompactFormat = true) =>
        TimerManager.Instance.StartTimer(timerId, duration, onTimerFinished, false, onTimerTick, tickInterval, false, useCompactFormat);

    /// <summary>
    /// Starts a timer with full customization.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="duration">Duration of the timer in seconds.</param>
    /// <param name="onTimerFinished">Action to be called when the timer finishes.</param>
    /// <param name="countUp">If true, the timer counts up. If false, the timer counts down.</param>
    /// <param name="onTimerTick">Action to be called at each tick interval with remaining time.</param>
    /// <param name="tickInterval">Interval in seconds for the tick action.</param>
    /// <param name="autoReset">If true, the timer will reset and start again after finishing.</param>
    /// <param name="useCompactFormat">If true, the timer will use compact time format. Otherwise, it will use detailed format.</param>
    public static void StartTimer(string timerId, float duration, Action onTimerFinished, bool countUp, Action<float> onTimerTick = null, float tickInterval = 1.0f, bool autoReset = false, bool useCompactFormat = true) =>
        TimerManager.Instance.StartTimer(timerId, duration, onTimerFinished, countUp, onTimerTick, tickInterval, autoReset, useCompactFormat);

    /// <summary>
    /// Schedules a task to be executed after a specified delay.
    /// </summary>
    /// <param name="taskId">Unique identifier for the scheduled task.</param>
    /// <param name="delay">Delay in seconds before the task is executed.</param>
    /// <param name="task">Action to be called when the task is executed.</param>
    /// <param name="recurringInterval">Interval for recurring tasks. Set to 0 for non-recurring tasks.</param>
    public static void ScheduleTask(string taskId, float delay, Action task, float recurringInterval = 0) =>
        TimerManager.Instance.StartTimer(taskId, delay, task, recurringInterval);

    /// <summary>
    /// Pauses a timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    public static void PauseTimer(string timerId) =>
        TimerManager.Instance.PauseTimer(timerId);

    /// <summary>
    /// Resumes a paused timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    public static void ResumeTimer(string timerId) =>
        TimerManager.Instance.ResumeTimer(timerId);

    /// <summary>
    /// Stops a timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    public static void StopTimer(string timerId) =>
        TimerManager.Instance.StopTimer(timerId);

    /// <summary>
    /// Gets the elapsed time of a timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>Elapsed time in seconds.</returns>
    public static float GetElapsedTime(string timerId) =>
        TimerManager.Instance.GetElapsedTime(timerId);

    /// <summary>
    /// Checks if a timer is running.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>True if the timer is running, otherwise false.</returns>
    public static bool IsTimerRunning(string timerId) =>
        TimerManager.Instance.IsTimerRunning(timerId);

    /// <summary>
    /// Enables or disables debug logging for the timer system.
    /// </summary>
    /// <param name="enable">If true, debug logging is enabled. If false, debug logging is disabled.</param>
    public static void EnableDebugLogging(bool enable) =>
        TimerManager.Instance.EnableDebugLogging = enable;

    /// <summary>
    /// Gets the remaining time of a timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>Remaining time in seconds.</returns>
    public static float GetRemainingTime(string timerId) =>
        TimerManager.Instance.GetRemainingTime(timerId);

    /// <summary>
    /// Checks if a timer counts up.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>True if the timer counts up, otherwise false.</returns>
    public static bool IsCountUp(string timerId) =>
        TimerManager.Instance.IsCountUp(timerId);

    /// <summary>
    /// Checks if a timer uses compact format.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>True if the timer uses compact format, otherwise false.</returns>
    public static bool UseCompactFormat(string timerId) =>
        TimerManager.Instance.UseCompactFormat(timerId);

    /// <summary>
    /// Updates the recurring interval for a specified timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="newInterval">The new interval for recurring tasks.</param>
    public static void UpdateRecurringInterval(string timerId, float newInterval) =>
        TimerManager.Instance.UpdateRecurringInterval(timerId, newInterval);

    /// <summary>
    /// Updates the scheduled task for a specified timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="newTask">The new task to be executed.</param>
    public static void UpdateScheduledTask(string timerId, Action newTask) =>
        TimerManager.Instance.UpdateScheduledTask(timerId, newTask);

    /// <summary>
    /// Formats the given time in seconds to a detailed human-readable string.
    /// </summary>
    /// <param name="timeInSeconds">The time in seconds.</param>
    /// <returns>A formatted string representing the time.</returns>
    public static string FormatDetailedTime(float timeInSeconds)
    {
        int years = Mathf.FloorToInt(timeInSeconds / 31536000F);
        int months = Mathf.FloorToInt((timeInSeconds % 31536000F) / 2592000F);
        int weeks = Mathf.FloorToInt((timeInSeconds % 2592000F) / 604800F);
        int days = Mathf.FloorToInt((timeInSeconds % 604800F) / 86400F);
        int hours = Mathf.FloorToInt((timeInSeconds % 86400F) / 3600F);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600F) / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);

        string yearsPart = years > 0 ? $"{years} year{(years > 1 ? "s" : "")} " : "";
        string monthsPart = months > 0 ? $"{months} month{(months > 1 ? "s" : "")} " : "";
        string weeksPart = weeks > 0 ? $"{weeks} week{(weeks > 1 ? "s" : "")} " : "";
        string daysPart = days > 0 ? $"{days} day{(days > 1 ? "s" : "")} " : "";
        string hoursPart = hours > 0 ? $"{hours} hour{(hours > 1 ? "s" : "")} " : "";
        string minutesPart = minutes > 0 ? $"{minutes} minute{(minutes > 1 ? "s" : "")} " : "";
        string secondsPart = $"{seconds} second{(seconds > 1 ? "s" : "")}";

        return $"{yearsPart}{monthsPart}{weeksPart}{daysPart}{hoursPart}{minutesPart}{secondsPart}".Trim();
    }

    /// <summary>
    /// Formats the given time in seconds to a compact human-readable string.
    /// </summary>
    /// <param name="timeInSeconds">The time in seconds.</param>
    /// <returns>A formatted string representing the time in a compact format.</returns>
    public static string FormatCompactTime(float timeInSeconds)
    {
        int years = Mathf.FloorToInt(timeInSeconds / 31536000F);
        int months = Mathf.FloorToInt((timeInSeconds % 31536000F) / 2592000F);
        int weeks = Mathf.FloorToInt((timeInSeconds % 2592000F) / 604800F);
        int days = Mathf.FloorToInt((timeInSeconds % 604800F) / 86400F);
        int hours = Mathf.FloorToInt((timeInSeconds % 86400F) / 3600F);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600F) / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);

        if (years > 0)
        {
            return $"{years:D2}:{months:D2}"; // YY:MM
        }
        else if (months > 0)
        {
            return $"{months:D2}:{weeks:D2}"; // MM:WW
        }
        else if (weeks > 0)
        {
            return $"{weeks:D2}:{days:D2}"; // WW:DD
        }
        else if (days > 0)
        {
            return $"{days:D2}:{hours:D2}"; // DD:HH
        }
        else if (hours > 0)
        {
            return $"{hours:D2}:{minutes:D2}"; // HH:MM
        }
        else
        {
            return $"{minutes:D2}:{seconds:D2}"; // MM:SS
        }
    }
}

