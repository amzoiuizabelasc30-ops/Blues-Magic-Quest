using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

// One GameManager per level scene.
// Handles: restarting the level when the bird dies, going to the next level,
// and showing the congratulations screen at the end of the last level.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Tick this ONLY in the last level")]
    public bool isFinalLevel = false;

    [Header("Win screen (only needed in the last level)")]
    public GameObject winPanel;
    public TMP_Text winText;

    [Header("Seconds to wait before restarting after a crash")]
    public float restartDelay = 0.5f;

    bool levelOver = false;
    public bool IsLevelOver => levelOver;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        if (winPanel != null) winPanel.SetActive(false);
    }

    void Update()
    {
        // After winning the game, press R to play again from level 1
        if (isFinalLevel && levelOver && Time.timeScale == 0f &&
            Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            GameState.Score = 0;
            GameState.ScoreAtLevelStart = 0;
            SceneManager.LoadScene(0);
        }
    }

    // Called by the bird when it hits an obstacle or the ground
    public void PlayerDied()
    {
        if (levelOver) return;
        levelOver = true;
        Invoke(nameof(RestartLevel), restartDelay);
    }

    void RestartLevel()
    {
        // Lose the points collected during this attempt
        GameState.Score = GameState.ScoreAtLevelStart;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Called by the Goal when the bird reaches the end of the level
    public void LevelCompleted()
    {
        if (levelOver) return;
        levelOver = true;

        if (isFinalLevel)
        {
            if (winPanel != null) winPanel.SetActive(true);
            if (winText != null)
                winText.text = "Congratulations!\nScore: " + GameState.Score + "\n\nPress R to play again";
            Time.timeScale = 0f; // freeze the game
        }
        else
        {
            GameState.ScoreAtLevelStart = GameState.Score;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
