using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float speed = 400.0f;
    private float speedLimiter = 15f;
    private float jumpForce = 10.0f;
    private float bounceStopper = 1.0f;
    private float lavaBounce = 20.0f;
    private float xBound = 148.5f;
    private float zBound = 148.5f;
    private int countdownTime = 10;

    private const int maxStars = 20;
    private int starCount = 0;

    private Rigidbody playerRigidBody;

    private GameObject focalPoint;
    private GameObject immunityPowerup;
    private GameObject boostJumpPowerup;
    private GameObject starKey;
    private GameObject heart;

    private AudioSource playerAudio;

    public bool onGround = true;
    public bool boostJump = false;
    public bool immunity = false;
    public bool isPlayerAlive = true;
    public bool isInstructionOpened = true;
    public bool gameOver = false;

    public float playerVelocity = 0f;

    public int maxHealth = 100;
    public int enemyDamage;
    public int recoverEneregy;
    public int currentHealth;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI starText;
    public TextMeshProUGUI countdownJumpDisplay;
    public TextMeshProUGUI countdownImmunityDisplay;

    public HealthBar healthBar;

    public Vector3 respawnP = new Vector3(-117f, 5f, -0.73f);

    public ParticleSystem playerDeath;
    public ParticleSystem starExplosion;

    public AudioClip getPowerup;
    public AudioClip getStarKey;
    public AudioClip getHeart;

    public GameObject instructionsBox;
    public GameObject gameOverBox;
    public GameObject youWonBox;
    public GameObject boostJumpIndicator;
    public GameObject immunityIndicator;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerRigidBody = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        playerAudio = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        DeveloperInfo();
        UserInterface();
        ConstrainPlayerMovement();

        if (currentHealth <= 0)
        {
            PlayerDestroyed();
            gameOverBox.SetActive(true);
        }

        if (instructionsBox.activeSelf)
        {
            isInstructionOpened = true;
        } else
        {
            isInstructionOpened = false;
        }

        if (boostJump)
        {
            boostJumpIndicator.SetActive(true);
        }

        if (immunity)
        {
            immunityIndicator.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        } else if (collision.gameObject.CompareTag("Destroy") && !immunity)
        {
            gameOverBox.SetActive(true);
            onGround = true;
            PlayerDestroyed();
        } else if (collision.gameObject.CompareTag("Damage Over Time") &&  !immunity)
        {
            onGround = false;
            playerRigidBody.AddForce(Vector3.up * lavaBounce, ForceMode.Impulse);
            currentHealth -= 15;
            healthBar.SetHealth(currentHealth);
            healthText.text = currentHealth + "/100";
        }

        if (collision.gameObject.CompareTag("Finish") && starCount == maxStars)
        {
            Debug.Log("You Won!");
            youWonBox.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destroy") && immunity)
        {
            onGround = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destroy") && !immunity)
        {
            gameOverBox.SetActive(true);
            onGround = true;
            PlayerDestroyed();
        }
    }

    private void OnTriggerEnter(Collider other) // trigger for each power up
    {
        if (other.CompareTag("Boost Jump"))
        {
            boostJump = true;
            boostJumpPowerup = other.gameObject;
            boostJumpPowerup.SetActive(false);
            playerAudio.PlayOneShot(getPowerup);
            BoostPowerup();
        }
        if (other.CompareTag("Immunity"))
        {
            immunity = true;
            immunityPowerup = other.gameObject;
            immunityPowerup.SetActive(false);
            playerAudio.PlayOneShot(getPowerup);
            ImmunityPowerup();
        }
        if (other.CompareTag("Star"))
        {
            starCount++;
            starText.text = starCount.ToString();
            starKey = other.gameObject;
            Destroy(starKey);
            playerAudio.PlayOneShot(getStarKey);
        }
        if (other.CompareTag("Enemy"))
        {
            enemyDamage = 10;
            TakeDamage(enemyDamage);
        }
        if (other.CompareTag("Heart") && currentHealth != maxHealth)
        {
            recoverEneregy = 10;
            heart = other.gameObject;
            Destroy(heart);
            playerAudio.PlayOneShot(getHeart);
            RecoverHealth(recoverEneregy);
        }
    }

    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isInstructionOpened)
        {
            instructionsBox.SetActive(true);
            isInstructionOpened = true;
            Time.timeScale = 0f;
        }
    }

    void BoostPowerup()
    {
        if (boostJump)
        {
            StartCoroutine(BoostJumpActiveTime());
        }
    }

    void ImmunityPowerup()
    {
        if (immunity)
        {
            StartCoroutine(ImmunityActiveTime());
        }
    }

    IEnumerator BoostJumpActiveTime() // Double jump time limit
    {
        while (countdownTime > 0)
        {

            yield return new WaitForSeconds(1f);
            countdownTime--;

            countdownJumpDisplay.text = countdownTime.ToString();
        }

        boostJumpIndicator.SetActive(false);
        boostJump = false;
        boostJumpPowerup.SetActive(true);
        countdownTime = 10;
        countdownJumpDisplay.text = countdownTime.ToString();
    }

    IEnumerator ImmunityActiveTime()  // Immunity from lava, poison, and poisonus grass time
    {
        while (countdownTime > 0)
        {

            yield return new WaitForSeconds(1f);
            countdownTime--;

            countdownImmunityDisplay.text = countdownTime.ToString();
        }

        immunityIndicator.SetActive(false);
        immunity = false;
        immunityPowerup.SetActive(true);
        countdownTime = 10;
        countdownImmunityDisplay.text = countdownTime.ToString();
    }

    void ConstrainPlayerMovement()
    {
        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
            playerRigidBody.AddForce(Vector3.right * bounceStopper, ForceMode.Impulse);
        }

        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
            playerRigidBody.AddForce(Vector3.left * bounceStopper, ForceMode.Impulse);
        }

        if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
            playerRigidBody.AddForce(Vector3.forward * bounceStopper, ForceMode.Impulse);
        }

        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
            playerRigidBody.AddForce(Vector3.back * bounceStopper, ForceMode.Impulse);
        }

        if (playerVelocity > speedLimiter)
        {
            playerVelocity = speedLimiter;
        }
    }

    void UserInterface()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        
        playerRigidBody.AddForce((focalPoint.transform.forward * verticalMovement * speed * Time.deltaTime));

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            if (!boostJump)
            {
                playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            } else
            {
                playerRigidBody.AddForce(Vector3.up * jumpForce*2, ForceMode.Impulse); // add same impulse to previews one to reach double height
            }
            onGround = false;
        }
    }

    void DeveloperInfo()
    {
        playerVelocity = playerRigidBody.velocity.magnitude;
    }

    void PlayerDestroyed()
    {
        currentHealth = 0;
        healthBar.SetHealth(currentHealth);
        healthText.text = currentHealth + "/100";
        Debug.Log("Game Over");
        gameOver = true;
        isPlayerAlive = false;
        gameObject.SetActive(false);
        playerDeath.Play();
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        healthText.text = currentHealth + "/100";
    }

    void RecoverHealth(int recoverHealth)
    {
        currentHealth += recoverHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
        healthText.text = currentHealth + "/100";
    }
}
