using UnityEngine;

public class TimerExample : MonoBehaviour
{
    private void Start()
    {
        // Enable debug logging
        //Timer.EnableDebugLogging(false);

        // Start a simple countdown timer for 5 seconds
        Timer.StartTimer("SimpleCountdown", 5.0f, TimerFinished);

        // Start a countdown timer for 5 seconds with a tick interval
        Timer.StartTimer("CountdownWithTick", 5.0f, TimerFinished, TimerTick, 1.0f);

        // Start a count-up timer for 5 seconds with full customization
        Timer.StartTimer("FullCustomTimer", 5.0f, TimerFinished, true, TimerTick, 0.5f, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Timer.PauseTimer("SimpleCountdown");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Timer.ResumeTimer("SimpleCountdown");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Timer.StopTimer("SimpleCountdown");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            float elapsedTime = Timer.GetElapsedTime("FullCustomTimer");
            Debug.Log("Elapsed Time: " + elapsedTime);
        }
    }

    private void TimerFinished()
    {
        Debug.Log("Timer Finished!");
    }

    private void TimerTick(float remainingTime)
    {
        Debug.Log("Remaining Time: " + remainingTime);
    }
}
