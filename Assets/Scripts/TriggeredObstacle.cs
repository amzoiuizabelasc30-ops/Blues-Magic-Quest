using UnityEngine;

// An obstacle that stays still until the player enters a TriggerZone,
// then moves by "moveBy" (e.g. a pipe that drops down or slides in).
public class TriggeredObstacle : MonoBehaviour
{
    public Vector2 moveBy = new Vector2(0f, -3f); // where it moves to, relative to where it starts
    public float speed = 5f;

    bool activated = false;
    Vector3 targetPos;

    void Start()
    {
        targetPos = transform.localPosition + (Vector3)moveBy;
    }

    public void Activate()
    {
        activated = true;
    }

    void Update()
    {
        if (!activated) return;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
    }
}
