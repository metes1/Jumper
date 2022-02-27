using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce;
    private Rigidbody2D rb;
    private bool isGrounded; // Check if player is on the ground
    private bool isDead; // is player dead

    [SerializeField] private AudioSource deathSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        jumpForce = 28.0f;
        rb.gravityScale = 9.0f;
        isGrounded = true;
        isDead = false;
    }

    // Update is called once per frame
    // Only gets the plays inputs, FixedUpdate is what moves the player
    void Update()
    {
        // Get player input from space key or mouse left click
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            // Player can only jump if they are on the ground and are not dead
            if (isGrounded && !isDead)
            {
                if (rb.gravityScale > 0)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                } else
                {
                    rb.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
                }
                
                // PLayer is jumping, they are no longer on the grounded
                isGrounded = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            // Player has collided with a platform, is grounded
            isGrounded = true;
        }
        else if (other.gameObject.tag == "Obstacle")
        {
            // If player collides with an obstacle, they die
            isDead = true;
            isGrounded = true;
            // Play sound effect when player dies
            deathSoundEffect.Play();
            // Call the PlayerDied function from the GameController
            GameController.instance.PlayerDied();
        }
        else if (other.gameObject.tag == "Mixed")
        {
            if (other.collider.gameObject.tag == "Obstacle")
            {
                // If player collides with an obstacle, they die
                isDead = true;
                isGrounded = true;
                // Play sound effect when player dies
                deathSoundEffect.Play();
                // Call the PlayerDied function from the GameController
                GameController.instance.PlayerDied();
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            // Player has exited platform, meaning they are no longer grounded
            isGrounded = false;
        }
    }

    // Reverse gravity when player triggers portal
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Portal")
        {
            rb.gravityScale *= -1.0f;
            GameController.instance.ReverseGravity();
        }
    }
}
