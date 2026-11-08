using UnityEngine;

// An invisible area. When the bird flies into it, the linked obstacles start moving.
// Needs a Collider2D with "Is Trigger" ticked (ticked automatically when you add this script).
[RequireComponent(typeof(Collider2D))]
public class TriggerZone : MonoBehaviour
{
    public TriggeredObstacle[] obstaclesToActivate;

    bool used = false;

    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used || !other.CompareTag("Player")) return;
        used = true;
        foreach (TriggeredObstacle obstacle in obstaclesToActivate)
            if (obstacle != null) obstacle.Activate();
    }
}
