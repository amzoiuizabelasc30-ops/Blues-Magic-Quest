using UnityEngine;

// Moves an object back and forth forever (up/down, left/right or diagonal).
// Use it for moving pipes AND moving coins.
public class MovingObject : MonoBehaviour
{
    [Tooltip("How far it moves from its starting point, in each direction. (0, 2) = 2 units up and down.")]
    public Vector2 moveDistance = new Vector2(0f, 2f);

    [Tooltip("How fast it moves.")]
    public float speed = 1f;

    [Tooltip("0 to 1. Use different values so objects don't all move in sync.")]
    [Range(0f, 1f)] public float startOffset = 0f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float t = Mathf.Sin(Time.time * speed + startOffset * Mathf.PI * 2f); // goes -1 .. 1
        transform.localPosition = startPos + (Vector3)(moveDistance * t);
    }
}
