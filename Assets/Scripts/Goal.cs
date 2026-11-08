using UnityEngine;

// Put this at the end of each level. Needs a Collider2D with "Is Trigger" ticked.
// Level 1: loads the next level. Last level: shows the congratulations screen.
[RequireComponent(typeof(Collider2D))]
public class Goal : MonoBehaviour
{
    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameManager.Instance != null)
            GameManager.Instance.LevelCompleted();
    }
}
