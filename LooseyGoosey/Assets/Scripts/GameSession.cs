using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession instance;

    private GameTimer gameTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            gameTimer = GetComponent<GameTimer>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameTimer GetGameTimer()
    {
        return gameTimer;
    }

    public void DestroyGameSession()
    {
        Destroy(gameObject);
    }
}