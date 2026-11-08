using UnityEngine;

// Put this on the Main Camera. Drag the Bird into "Target".
// The camera follows the bird left/right only (not up/down), like Flappy Bird.
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float xOffset = 2f; // positive = bird sits a bit to the left of the screen centre

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 pos = transform.position;
        pos.x = target.position.x + xOffset;
        transform.position = pos;
    }
}
