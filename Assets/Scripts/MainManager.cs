using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private TMP_InputField playerName;
    public static MainManager Instance;    
    public bool IsGameOver {get; private set; }
    public UnityEvent GameOver {get; set; }


    void Start()
    {
        if (GameOver == null)
        {
            GameOver = new UnityEvent();
        }
        
        GameOver.AddListener(EndGame);
        IsGameOver = false;
        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (IsGameOver & Input.GetKeyDown(KeyCode.Space))
        {
            IsGameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StartLevel()
    {
        if(playerName.text == "")
        {
            playerName.text = "Player";
        }
        Debug.Log(playerName.text);
        gameMenu.SetActive(false);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
  
    void EndGame()
    {
        IsGameOver = true;
    }
}
