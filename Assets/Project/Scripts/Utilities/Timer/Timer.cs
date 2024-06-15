using System;

public static class Timer
{
    /// <summary>
    /// Starts a simple countdown timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="duration">Duration of the timer in seconds.</param>
    /// <param name="onTimerFinished">Action to be called when the timer finishes.</param>
    public static void StartTimer(string timerId, float duration, Action onTimerFinished)
    {
        TimerManager.Instance.StartTimer(timerId, duration, onTimerFinished);
    }

    /// <summary>
    /// Starts a countdown timer with a tick interval.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="duration">Duration of the timer in seconds.</param>
    /// <param name="onTimerFinished">Action to be called when the timer finishes.</param>
    /// <param name="onTimerTick">Action to be called at each tick interval with remaining time.</param>
    /// <param name="tickInterval">Interval in seconds for the tick action.</param>
    public static void StartTimer(string timerId, float duration, Action onTimerFinished, Action<float> onTimerTick, float tickInterval = 1.0f)
    {
        TimerManager.Instance.StartTimer(timerId, duration, onTimerFinished, false, onTimerTick, tickInterval);
    }

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
    public static void StartTimer(string timerId, float duration, Action onTimerFinished, bool countUp, Action<float> onTimerTick = null, float tickInterval = 1.0f, bool autoReset = false)
    {
        TimerManager.Instance.StartTimer(timerId, duration, onTimerFinished, countUp, onTimerTick, tickInterval, autoReset);
    }

    /// <summary>
    /// Pauses a timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    public static void PauseTimer(string timerId)
    {
        TimerManager.Instance.PauseTimer(timerId);
    }

    /// <summary>
    /// Resumes a paused timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    public static void ResumeTimer(string timerId)
    {
        TimerManager.Instance.ResumeTimer(timerId);
    }

    /// <summary>
    /// Stops a timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    public static void StopTimer(string timerId)
    {
        TimerManager.Instance.StopTimer(timerId);
    }

    /// <summary>
    /// Gets the elapsed time of a timer.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>Elapsed time in seconds.</returns>
    public static float GetElapsedTime(string timerId)
    {
        return TimerManager.Instance.GetElapsedTime(timerId);
    }

    /// <summary>
    /// Checks if a timer is running.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <returns>True if the timer is running, otherwise false.</returns>
    public static bool IsTimerRunning(string timerId)
    {
        return TimerManager.Instance.IsTimerRunning(timerId);
    }

    /// <summary>
    /// Enables or disables debug logging for the timer system.
    /// </summary>
    /// <param name="enable">If true, debug logging is enabled. If false, debug logging is disabled.</param>
    public static void EnableDebugLogging(bool enable)
    {
        TimerManager.Instance.EnableDebugLogging = enable;
    }
}
