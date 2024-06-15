using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private static TimerManager _instance;

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

    private Dictionary<string, TimerData> timers = new Dictionary<string, TimerData>();
    public bool EnableDebugLogging { get; set; } = false;

    public void StartTimer(string timerId, float duration, Action onTimerFinished, bool countUp = false, Action<float> onTimerTick = null, float tickInterval = 1.0f, bool autoReset = false)
    {
        if (timers.ContainsKey(timerId))
        {
            StopTimer(timerId);
        }

        TimerData timerData = new TimerData
        {
            Duration = duration,
            RemainingTime = countUp ? 0 : duration,
            ElapsedTime = 0,
            IsPaused = false,
            CountUp = countUp,
            OnTimerFinished = onTimerFinished,
            OnTimerTick = onTimerTick,
            TickInterval = tickInterval,
            LastTickTime = Time.time,
            AutoReset = autoReset
        };

        timers[timerId] = timerData;
        StartCoroutine(TimerCoroutine(timerId, timerData));

        if (EnableDebugLogging)
        {
            Debug.Log($"Timer '{timerId}' started: Duration = {duration}, CountUp = {countUp}, AutoReset = {autoReset}");
        }
    }

    public void PauseTimer(string timerId)
    {
        if (timers.ContainsKey(timerId))
        {
            timers[timerId].IsPaused = true;

            if (EnableDebugLogging)
            {
                Debug.Log($"Timer '{timerId}' paused.");
            }
        }
    }

    public void ResumeTimer(string timerId)
    {
        if (timers.ContainsKey(timerId))
        {
            timers[timerId].IsPaused = false;

            if (EnableDebugLogging)
            {
                Debug.Log($"Timer '{timerId}' resumed.");
            }
        }
    }

    public void StopTimer(string timerId)
    {
        if (timers.ContainsKey(timerId))
        {
            timers[timerId].IsPaused = true;
            timers.Remove(timerId);

            if (EnableDebugLogging)
            {
                Debug.Log($"Timer '{timerId}' stopped.");
            }
        }
    }

    public float GetElapsedTime(string timerId)
    {
        if (timers.ContainsKey(timerId))
        {
            return timers[timerId].ElapsedTime;
        }
        return 0;
    }

    public bool IsTimerRunning(string timerId)
    {
        return timers.ContainsKey(timerId);
    }

    private IEnumerator TimerCoroutine(string timerId, TimerData timerData)
    {
        while ((timerData.CountUp && timerData.RemainingTime < timerData.Duration) || (!timerData.CountUp && timerData.RemainingTime > 0))
        {
            if (!timerData.IsPaused)
            {
                float deltaTime = Time.deltaTime;
                timerData.ElapsedTime += deltaTime;
                if (timerData.CountUp)
                {
                    timerData.RemainingTime += deltaTime;
                }
                else
                {
                    timerData.RemainingTime -= deltaTime;
                }

                if (Time.time - timerData.LastTickTime >= timerData.TickInterval)
                {
                    timerData.OnTimerTick?.Invoke(timerData.RemainingTime);
                    timerData.LastTickTime = Time.time;

                    if (EnableDebugLogging)
                    {
                        Debug.Log($"Timer '{timerId}' tick: Remaining Time = {timerData.RemainingTime}");
                    }
                }
            }

            yield return null;
        }

        timerData.OnTimerFinished?.Invoke();

        if (EnableDebugLogging)
        {
            Debug.Log($"Timer '{timerId}' finished.");
        }

        if (timerData.AutoReset)
        {
            timerData.RemainingTime = timerData.CountUp ? 0 : timerData.Duration;
            timerData.ElapsedTime = 0;
            StartCoroutine(TimerCoroutine(timerId, timerData));
        }
        else
        {
            timers.Remove(timerId);
        }
    }

    private class TimerData
    {
        public float Duration;
        public float RemainingTime;
        public float ElapsedTime;
        public bool IsPaused;
        public bool CountUp;
        public Action OnTimerFinished;
        public Action<float> OnTimerTick;
        public float TickInterval;
        public float LastTickTime;
        public bool AutoReset;
    }
}
