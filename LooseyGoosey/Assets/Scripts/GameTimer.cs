using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Stuff")]
    [Tooltip("The Text object that will display the hours")]
    [SerializeField] private Text hoursText;
    [Tooltip("The Text object that will display the minutes")]
    [SerializeField] private Text minutesText;
    [Tooltip("The Text object that will display the seconds")]
    [SerializeField] private Text secondsText;
    [Tooltip("The Text object that will display the milliseconds")]
    [SerializeField] private Text millisecondsText;

    private float timer;
    private bool isGameRunning = true;
    private float lastTimeValue;

    void Update()
    {
        if (isGameRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        // Calculate hours, minutes, seconds, and milliseconds
        int hours = Mathf.FloorToInt(timer / 3600);
        int minutes = Mathf.FloorToInt((timer % 3600) / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = (int)(timer * 1000) % 100;

        // Limit milliseconds to two digits
        milliseconds = Mathf.Clamp(milliseconds, 0, 99);

        // Update the Text objects for each time value
        hoursText.text = hours.ToString("D2");
        minutesText.text = minutes.ToString("D2");
        secondsText.text = seconds.ToString("D2");
        millisecondsText.text = milliseconds.ToString("D2");
    }

    public void EndGame()
    {
        // Game has ended, store the last time value
        lastTimeValue = timer;
        isGameRunning = false;
    }

    public float GetLastTimeValue()
    {
        return lastTimeValue;
    }
}
