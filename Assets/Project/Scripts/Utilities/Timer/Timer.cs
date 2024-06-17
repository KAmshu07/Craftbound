using System;
using UnityEngine;

/// <summary>
/// Represents a timer with various customizable properties and behaviors.
/// </summary>
public class Timer
{
    /// <summary>
    /// Gets the unique identifier of the timer.
    /// </summary>
    public string TimerId { get; private set; }

    /// <summary>
    /// Gets the duration of the timer.
    /// </summary>
    public float Duration { get; private set; }

    /// <summary>
    /// Gets the remaining time of the timer.
    /// </summary>
    public float RemainingTime { get; private set; }

    /// <summary>
    /// Gets the elapsed time of the timer.
    /// </summary>
    public float ElapsedTime { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the timer is paused.
    /// </summary>
    public bool IsPaused { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the timer counts up.
    /// </summary>
    public bool CountUp { get; private set; }

    /// <summary>
    /// Gets the action to be called when the timer finishes.
    /// </summary>
    public Action OnTimerFinished { get; private set; }

    /// <summary>
    /// Gets the action to be called at each tick interval with the remaining time.
    /// </summary>
    public Action<float> OnTimerTick { get; private set; }

    /// <summary>
    /// Gets the interval in seconds for the tick action.
    /// </summary>
    public float TickInterval { get; private set; }

    /// <summary>
    /// Gets the last tick time.
    /// </summary>
    public float LastTickTime { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the timer automatically resets after finishing.
    /// </summary>
    public bool AutoReset { get; private set; }

    /// <summary>
    /// Gets a value indicating whether to use compact format for time display.
    /// </summary>
    public bool UseCompactFormat { get; private set; }

    /// <summary>
    /// Gets the action to be called when the scheduled task is executed.
    /// </summary>
    public Action ScheduledTask { get; private set; }

    /// <summary>
    /// Gets or sets the interval for recurring tasks.
    /// </summary>
    public float RecurringInterval { get; private set; }

    private Action<string> removeTimerCallback;

    /// <summary>
    /// Initializes a new instance of the <see cref="Timer"/> class.
    /// </summary>
    /// <param name="timerId">Unique identifier for the timer.</param>
    /// <param name="duration">Duration of the timer in seconds.</param>
    /// <param name="onTimerFinished">Action to be called when the timer finishes.</param>
    /// <param name="countUp">If true, the timer counts up. If false, the timer counts down.</param>
    /// <param name="onTimerTick">Action to be called at each tick interval with remaining time.</param>
    /// <param name="tickInterval">Interval in seconds for the tick action.</param>
    /// <param name="autoReset">If true, the timer will reset and start again after finishing.</param>
    /// <param name="removeTimerCallback">Callback to remove the timer from the manager.</param>
    /// <param name="useCompactFormat">If true, the timer will use compact time format. Otherwise, it will use detailed format.</param>
    /// <param name="scheduledTask">Action to be called when the scheduled task is executed.</param>
    /// <param name="recurringInterval">Interval for recurring tasks. Set to 0 for non-recurring tasks.</param>
    public Timer(string timerId, float duration, Action onTimerFinished, bool countUp = false, Action<float> onTimerTick = null, float tickInterval = 1.0f, bool autoReset = false, Action<string> removeTimerCallback = null, bool useCompactFormat = true, Action scheduledTask = null, float recurringInterval = 0)
    {
        TimerId = timerId;
        Duration = duration;
        RemainingTime = countUp ? 0 : duration;
        ElapsedTime = 0;
        IsPaused = false;
        CountUp = countUp;
        OnTimerFinished = onTimerFinished;
        OnTimerTick = onTimerTick;
        TickInterval = tickInterval;
        LastTickTime = Time.time;
        AutoReset = autoReset;
        this.removeTimerCallback = removeTimerCallback;
        UseCompactFormat = useCompactFormat;
        ScheduledTask = scheduledTask;
        RecurringInterval = recurringInterval;
    }

    /// <summary>
    /// Updates the timer based on the elapsed time.
    /// </summary>
    /// <param name="deltaTime">The time that has passed since the last update.</param>
    public void Update(float deltaTime)
    {
        if (IsPaused) return;

        ElapsedTime += deltaTime;
        RemainingTime = CountUp ? RemainingTime + deltaTime : RemainingTime - deltaTime;

        if (Time.time - LastTickTime >= TickInterval)
        {
            OnTimerTick?.Invoke(RemainingTime);
            LastTickTime = Time.time;
        }

        if ((CountUp && RemainingTime >= Duration) || (!CountUp && RemainingTime <= 0))
        {
            OnTimerFinished?.Invoke();
            ScheduledTask?.Invoke(); // Execute the scheduled task if any

            if (RecurringInterval > 0)
            {
                RemainingTime = RecurringInterval;
                ElapsedTime = 0;
                LastTickTime = Time.time;
            }
            else if (AutoReset)
            {
                Reset();
            }
            else
            {
                RemainingTime = 0;
                Stop();
            }
        }
    }

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    public void Pause() => IsPaused = true;

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    public void Resume() => IsPaused = false;

    /// <summary>
    /// Resets the timer.
    /// </summary>
    public void Reset()
    {
        RemainingTime = CountUp ? 0 : Duration;
        ElapsedTime = 0;
        LastTickTime = Time.time;
        IsPaused = false;
    }

    /// <summary>
    /// Stops the timer by invoking the remove callback.
    /// </summary>
    public void Stop()
    {
        removeTimerCallback?.Invoke(TimerId);
    }

    /// <summary>
    /// Updates the recurring interval for the timer.
    /// </summary>
    /// <param name="newInterval">The new interval for recurring tasks.</param>
    public void UpdateRecurringInterval(float newInterval)
    {
        RecurringInterval = newInterval;
    }

    /// <summary>
    /// Updates the scheduled task.
    /// </summary>
    /// <param name="newTask">The new task to be executed.</param>
    public void UpdateScheduledTask(Action newTask)
    {
        ScheduledTask = newTask;
    }
}
