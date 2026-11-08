using UnityEngine;

// Spins an object forever. Good for saw blades / spinning bars.
public class Rotator : MonoBehaviour
{
    public float degreesPerSecond = 180f; // negative = spins the other way

    void Update()
    {
        transform.Rotate(0f, 0f, degreesPerSecond * Time.deltaTime);
    }
}
