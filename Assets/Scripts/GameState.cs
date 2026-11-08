using UnityEngine;

// Keeps the score between scenes (levels).
// It's a static class, so it survives when a scene is reloaded or a new level is loaded.
public static class GameState
{
    public static int Score;              // current score
    public static int ScoreAtLevelStart;  // score when the current level began (restored when you die)

    // Resets everything each time you press Play in the editor
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetOnPlay()
    {
        Score = 0;
        ScoreAtLevelStart = 0;
    }

    public static void AddPoints(int amount)
    {
        Score += amount;
    }
}
