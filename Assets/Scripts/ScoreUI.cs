using UnityEngine;
using TMPro;

// Put this on a TextMeshPro text in your Canvas. Shows the current score.
public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public string prefix = "Score: ";

    int lastShown = -1;

    void Awake()
    {
        if (scoreText == null) scoreText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (GameState.Score != lastShown)
        {
            lastShown = GameState.Score;
            scoreText.text = prefix + lastShown;
        }
    }
}
