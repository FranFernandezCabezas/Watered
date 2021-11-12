using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float health, speed, jumpForce;
    public bool isGrounded;

    public Transform groundCheck;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    Animator anim;

    // List of player's sounds
    public AudioClip[] playerSounds;
    AudioSource audioSource;
    public AudioSource runningAudioSource;

    public GameController gameController;

    // Used to determine the size of the sprite
    public float fullHealthSize, currentSize, sizeSubstracter;
    bool isLookingRight;
    

    // Start is called before the first frame update
    void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fullHealthSize = transform.localScale.x;
        currentSize = fullHealthSize;
        sizeSubstracter = fullHealthSize / 10;
        isLookingRight = true;
        audioSource = gameObject.GetComponent<AudioSource>();
        health = 10;

        // To make the player lose life every second
        InvokeRepeating("ReducePlayerHealth", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            anim.SetBool("isGoingUp", false);
            anim.SetBool("isGoingDown", false);
        }
        else
        {
            CheckIfFalling();
        }
    }

    private void CheckIfFalling()
    {
        // Checks if the player is going up or down, and changes the animation depending on it
        if (rb.velocity.y > 0.1)
        {
            anim.SetBool("isGoingUp", true);
            anim.SetBool("isGoingDown", false);
        } else if (rb.velocity.y < 0.1)
        {
            anim.SetBool("isGoingUp", false);
            anim.SetBool("isGoingDown", true);
        } else
        {
            anim.SetBool("isGoingUp", false);
            anim.SetBool("isGoingDown", false);
            MakeSoundPlay(1);
        }
    }

    private void FixedUpdate()
    {
        if (gameController.isGameStarted) {
            checkJump();
            checkMovement();
        }

        if(health == 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void checkMovement()
    {
        float velX = Input.GetAxisRaw("Horizontal");
        float velY = rb.velocity.y;

        rb.velocity = new Vector2(velX * speed, velY);

        if (rb.velocity.x != 0)
        {
            anim.SetBool("isRunning", true);
            //runningAudioSource.Play();
        }
        else
        {
            anim.SetBool("isRunning", false);
            //runningAudioSource.Stop();
        }

        // Flips the sprite when running left or right
        //Flip
        if (rb.velocity.x > 0)
        {
            isLookingRight = true;
            transform.localScale = new Vector3(currentSize, currentSize, currentSize);
        }

        if (rb.velocity.x < 0)
        {
            isLookingRight = false;
            transform.localScale = new Vector3(-currentSize, currentSize, currentSize);
        }
}

    void checkJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.05f, whatIsGround);

        if (Input.GetButton("Jump") && isGrounded)
        {
            MakeSoundPlay(0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            gameController.CollectCoin(collision.gameObject);
        }

        if (collision.CompareTag("Waterbank"))
        {
            gameController.waterBanksTouched += 1;
            collision.tag = "UsedWaterbank";
        }

        if (collision.CompareTag("Triggerable"))
        {
            collision.GetComponent<Triggerable>().ActivateElement();
        }

        if (collision.CompareTag("Spikes"))
        {
            health = 0;
        }

        if (collision.CompareTag("SecretZone"))
        {
            gameController.ActivateZoom(true);
            gameController.superForeground.SetActive(false);
        }

        if (collision.CompareTag("CameraLimit"))
        {
            gameController.cameraController.isNearWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SecretZone"))
        {
            gameController.ActivateZoom(false);
            gameController.superForeground.SetActive(true);

        }

        if (collision.CompareTag("CameraLimit"))
        {
            gameController.cameraController.isNearWall = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            if (!collision.gameObject.GetComponent<Animator>().GetBool("isOnGround"))
            {
                if (collision.gameObject.GetComponent<Traps>().isActive) health = 0;
            }
        }

        if (collision.gameObject.CompareTag("Leak"))
        {
            Destroy(collision.gameObject);
            if (health + 5 > 10)
            {
                health = 10;
                currentSize = fullHealthSize;
            } else
            {
                health += 5;
                currentSize += sizeSubstracter * 5;
            }
            CheckSpriteScale();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("UsedWaterbank"))
        {
            health = 10;
            currentSize = fullHealthSize;
            CheckSpriteScale();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            if (collision.gameObject.GetComponent<Animator>().GetBool("isOnGround"))
            {
                isGrounded = true;
            }
        }
    }

    void ReducePlayerHealth()
    {
        health -= 1;
        currentSize -= sizeSubstracter;
        CheckSpriteScale();
    }

    public void CheckSpriteScale()
    {
        if (isLookingRight) 
        {
            transform.localScale = new Vector3(currentSize, currentSize, currentSize);
        } 
        else
        {
            transform.localScale = new Vector3(-currentSize, currentSize, currentSize);
        }
        MakeSoundPlay(1);
    }

    void MakeSoundPlay(int clipPosition)
    {
        audioSource.Stop();
        audioSource.clip = playerSounds[clipPosition];
        audioSource.Play();
    }
}
