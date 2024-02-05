using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("Settings Stuff")]
    [SerializeField] private string highScoreMessage = "WINN-Y GINN-Y!";
    [SerializeField] private string lowScoreMessage = "LOSE-Y GOOZE-Y!";
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text bestTimeScore;
    [SerializeField] private Text yourTimeScore;
    [SerializeField] private Image winImage;
    [SerializeField] private Image loseImage;

    private void Start()
    {
        // Find the GameSession object
        GameSession gameSession = FindObjectOfType<GameSession>();

        if (gameSession != null)
        {
            // GameTimer from GameSession
            GameTimer gameTimer = gameSession.GetGameTimer();

            if (gameTimer != null)
            {
                float yourTime = gameTimer.GetLastTimeValue();

                // PlayerPrefs (persistent storage)
                float bestTime = PlayerPrefs.GetFloat("BestTime", Mathf.Infinity);

                // Convert times to milliseconds for comparison
                int yourTimeInMilliseconds = Mathf.FloorToInt(yourTime * 1000);
                int bestTimeInMilliseconds = Mathf.FloorToInt(bestTime * 1000);

                Debug.Log("Your Time In Milliseconds: " + yourTimeInMilliseconds);
                Debug.Log("Best Time In Milliseconds: " + bestTimeInMilliseconds);

                // Compare times in milliseconds
                if (yourTimeInMilliseconds > bestTimeInMilliseconds)
                {
                    // If the player achieves or matches the best time
                    bestTime = yourTime;
                    PlayerPrefs.SetFloat("BestTime", bestTime);
                    PlayerPrefs.Save(); // Save PlayerPrefs changes

                    gameOverText.text = highScoreMessage;

                    // Win
                    winImage.gameObject.SetActive(true);
                    loseImage.gameObject.SetActive(false);
                }
                else
                {
                    // If the player doesn't achieve the best time
                    gameOverText.text = lowScoreMessage;

                    // Lose
                    loseImage.gameObject.SetActive(true);
                    winImage.gameObject.SetActive(false);
                }

                // Display best times with milliseconds
                bestTimeScore.text = FormatTime(bestTime);
                yourTimeScore.text = FormatTime(yourTime);
            }
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int hours = Mathf.FloorToInt(timeInSeconds / 3600);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000) % 100);

        hours = Mathf.Clamp(hours, 0, 99);
        minutes = Mathf.Clamp(minutes, 0, 99);
        seconds = Mathf.Clamp(seconds, 0, 99);
        milliseconds = Mathf.Clamp(milliseconds, 0, 99);

        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hours, minutes, seconds, milliseconds);
    }
}