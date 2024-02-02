using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interactions : MonoBehaviour
{
    [SerializeField] private Text bestTimeScore;
    [SerializeField] private Text yourTimeScore;

    void Start()
    {
        // Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        // Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(1);
    }

    public void RestartGame()
    {
        // Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameSession.instance.DestroyGameSession();

        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetBestTime()
    {
        // Reset the BestTime save slot in PlayerPrefs
        PlayerPrefs.DeleteKey("BestTime");
        Debug.Log("BestTime reset!");

        UpdateTimeScores(TimeSpan.Zero);
    }

    private void UpdateTimeScores(TimeSpan timeSpan)
    {
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);

        if (bestTimeScore != null)
        {
            bestTimeScore.text = formattedTime;
        }
        if (yourTimeScore != null)
        {
            yourTimeScore.text = formattedTime;
        }
    }
}