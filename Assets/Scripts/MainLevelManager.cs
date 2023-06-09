using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLevelManager : MonoBehaviour
{    
    [SerializeField] private Rigidbody ball;
    [SerializeField] private Brick brickPrefab;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private int lineCount = 6;

    private bool isLevelStarted;
    
    private int currentScore;
    private int bestScore;
    private string bestPlayer;


    void Start()
    {      
        bestPlayer = PlayerPrefs.GetString("bestPlayer", null);
        if (bestPlayer == "" || bestPlayer == null) 
        {
            bestPlayer = MainManager.Instance.CurrentPlayer;
        }
        bestScore = PlayerPrefs.GetInt("bestScore", 0);
        DisplayBestScore();

        SpawnBricks();

        ball.gameObject.SetActive(true);
        isLevelStarted = true;
        MainManager.Instance.GameOver.AddListener(GameOver);    
    }

    void Update()
    {
        if(isLevelStarted & Input.GetKeyDown(KeyCode.Space))
        {
            float randomDirection = Random.Range(-1.0f, 1.0f);
            Vector3 forceDir = new Vector3(randomDirection, 1, 0);
            forceDir.Normalize();

            ball.transform.SetParent(null);
            ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        }
        else if (MainManager.Instance.IsGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void OnDestroy()
    {
        MainManager.Instance.GameOver.RemoveListener(GameOver);
    }


    private void AddPoint(int point)
    {
        currentScore += point;
        scoreText.text = $"Score : {currentScore}";
    }

    private void DisplayBestScore()
    {
        bestScoreText.text = $"Best score: {bestPlayer} - {bestScore}";
    }

    private void SpawnBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        Vector3 brickPosition;
        Transform parent = GameObject.Find("Bricks").GetComponent<Transform>();
        Brick brick;
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                brickPosition = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                brick = Instantiate(brickPrefab, brickPosition, Quaternion.identity, parent);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void GameOver()
    {
        if(bestScore < currentScore)
        {
            bestPlayer = MainManager.Instance.CurrentPlayer;

            PlayerPrefs.SetInt("bestScore", currentScore);
            PlayerPrefs.SetString("bestPlayer", MainManager.Instance.CurrentPlayer);
        }

        gameOverText.SetActive(true);
        isLevelStarted = false;
    }
}
