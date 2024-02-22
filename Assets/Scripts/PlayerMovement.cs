using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

public class PlayerMovement : MonoBehaviour
{
    [System.Serializable]
    public struct Stats
    {
        public float speed;
        public float jumpForce;
        public bool canMove;
        public bool canJump;
    }

    public Stats playerStats;
    public SoundManager soundManager;
    public LayerMask groundLayer;
    public Transform groundCheckL, groundCheckR;
    public Transform mainCamera;
    public Text healthText; // Reference to the UI Text component for displaying health

    private float moveX, moveY;
    private float facing;
    private Rigidbody rb;
    private int health = 1000; // Initial health value

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateHealthText();
    }

    private void Update()
    {
        if (playerStats.canMove == true)
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");

            bool hitL = Physics.Linecast(new Vector3(groundCheckL.position.x, transform.position.y + 1, transform.position.z), groundCheckL.position, groundLayer);
            bool hitR = Physics.Linecast(new Vector3(groundCheckR.position.x, transform.position.y + 1, transform.position.z), groundCheckR.position, groundLayer);

            Debug.DrawLine(new Vector3(groundCheckL.position.x, transform.position.y + 1, transform.position.z), groundCheckL.position, Color.red);
            Debug.DrawLine(new Vector3(groundCheckR.position.x, transform.position.y + 1, transform.position.z), groundCheckR.position, Color.red);

            if (hitL || hitR)
            {
                playerStats.canJump = true;
            }
            else
            {
                playerStats.canJump = false;
            }

            if (playerStats.canJump)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerStats.canMove == true)
        {
            Vector3 movement = ((mainCamera.right * moveX) * playerStats.speed) + ((mainCamera.forward * moveY) * playerStats.speed);
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

            if (movement.x != 0 && movement.z != 0)
            {
                facing = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            }
            rb.rotation = Quaternion.Euler(0, facing, 0);
        }
    }

    private void Jump()
    {
        playerStats.canJump = false;
        soundManager.PlayJumpSound();
        rb.AddForce(Vector3.up * playerStats.jumpForce);
    }

    public void TakeDamage(int damageAmount)
    {
        if (!playerStats.canMove)
        {
            return;
        }

        health -= damageAmount;

        if (health <= 0)
        {
            health = 0;
            Die(); // Call the Die method when health is zero
        }

        UpdateHealthText();
    }

    private void Die()
    {
        // Perform actions when the player dies
        Debug.Log("Player has died!");

        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health;
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        UpdateHealthText();
    }
}

