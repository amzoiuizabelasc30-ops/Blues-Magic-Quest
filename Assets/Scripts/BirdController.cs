using UnityEngine;
using UnityEngine.InputSystem;

// Put this on the Bird. Needs a Rigidbody2D and a Collider2D.
// The bird flies to the right at a constant speed and flaps with Space / left click / tap.
// It floats in place until the first flap.
[RequireComponent(typeof(Rigidbody2D))]
public class BirdController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 3f;   // how fast the bird moves right (make it higher in level 2)
    public float flapForce = 6f;      // how strong each flap is
    public float gravityScale = 2f;   // how fast it falls

    [Header("Tilt (visual only)")]
    public float maxUpAngle = 30f;
    public float maxDownAngle = -60f;
    public float tiltSpeed = 6f;

    Rigidbody2D rb;
    Animator anim;      // optional: plays the "Flap" animation trigger
    bool started = false;
    bool dead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.gravityScale = 0f;      // no gravity until the player flaps
        rb.freezeRotation = true;  // we rotate the bird ourselves
    }

    void Update()
    {
        if (dead) return;

        if (FlapPressed())
        {
            if (!started)
            {
                started = true;
                rb.gravityScale = gravityScale;
            }
            rb.linearVelocity = new Vector2(forwardSpeed, flapForce);

            // Play the jump animation (needs a Trigger parameter called "Flap" in the Animator)
            if (anim != null) anim.SetTrigger("Flap");
        }

        // Tilt the bird up when rising, down when falling
        float targetAngle = 0f;
        if (started)
        {
            float t = Mathf.InverseLerp(-flapForce, flapForce, rb.linearVelocity.y);
            targetAngle = Mathf.Lerp(maxDownAngle, maxUpAngle, t);
        }
        Quaternion target = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, tiltSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // Keep moving right at a constant speed
        if (started && !dead)
            rb.linearVelocity = new Vector2(forwardSpeed, rb.linearVelocity.y);
    }

    bool FlapPressed()
    {
        bool key = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool mouse = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool touch = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
        return key || mouse || touch;
    }

    // Solid obstacles and the ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Ground"))
            Die();
    }

    // Obstacles whose collider is set to "Is Trigger" (e.g. spinning saws)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
            Die();
    }

    void Die()
    {
        if (dead) return;
        dead = true;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        if (GameManager.Instance != null)
            GameManager.Instance.PlayerDied();
    }
}
