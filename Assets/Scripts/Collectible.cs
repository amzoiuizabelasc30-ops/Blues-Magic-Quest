using UnityEngine;

// Put this on coins / anything that gives points.
// The object needs a Collider2D with "Is Trigger" ticked.
// To make it move too, also add MovingObject.
public class Collectible : MonoBehaviour
{
    public int points = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameState.AddPoints(points);
        Destroy(gameObject);
    }
}
