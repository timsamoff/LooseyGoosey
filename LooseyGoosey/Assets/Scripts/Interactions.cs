using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interactions : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioClip[] gooseSounds;

    private AudioSource audioSource;

    [Header("Text Prompts")]
    [SerializeField] private Text bestTimeScore;
    [SerializeField] private Text yourTimeScore;

    private bool play = false;
    private bool quit = false;
    private bool restart = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (quit)
            {
                Application.Quit();
            }
            if (restart)
            {
                SceneManager.LoadScene(0);
            }
            if (play)
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    private void PlaySound()
    {
        if (audioSource)
        {
            int randomIndex = UnityEngine.Random.Range(0, gooseSounds.Length);
            audioSource.PlayOneShot(gooseSounds[randomIndex]);
        }
    }

    public void PlayGame()
    {
        // Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlaySound();

        play = true;
    }

    public void RestartGame()
    {
        // Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameSession.instance.DestroyGameSession();

        PlaySound();

        restart = true;
    }

    public void QuitGame()
    {
        PlaySound();

        quit = true;
    }

    public void ResetBestTime()
    {
        PlaySound();

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