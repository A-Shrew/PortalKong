using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("References")]
    public Camera mainCam;
    [SerializeField] private PortalManager portalManager;
    [SerializeField] private Transform respawn;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private ParticleSystem particlesA;
    [SerializeField] private ParticleSystem particlesB;
   

    [Header("Player Stats")]
    [SerializeField] public int health;
    [SerializeField] private float speed;
    [SerializeField] private float ladderSpeed;
    [SerializeField] private float ladderGrabDistance;
    [SerializeField] private float jump;
    [SerializeField] private float jumpBufferDistance;
    [SerializeField] private float dash;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float portalCooldown;

    [Header("Player Physics")]
    [SerializeField] private float extraGravity;
    [SerializeField] private float airDrag;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float mouseSense;


    //Camera Stuff
    [SerializeField] private float rotationSmoothTime;
    private float horizontalLook;
    private float verticalLook;

    //Player Stuff
    public Rigidbody rb;
    private float jumpRay;
    private float ladderRay;
    private bool isGrounded;
    public bool canDash;
    public float dashTimer;
    private bool canShootPortal;
    private bool hasDoubleJump;
    private bool canLook = true;
    public TMP_Text dashText;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        AddInputs();
        rb = GetComponent<Rigidbody>();
        jumpRay = transform.localScale.y + jumpBufferDistance;
        ladderRay = transform.localScale.x/2 + ladderGrabDistance;
        isGrounded = true;
        canDash = true;
        canShootPortal = true;
    }

    // FixedUpdate is called every fixed framerate frame
    void FixedUpdate()
    {
        AuxiliaryMovement();
        Debug.DrawRay(transform.position, Vector3.down * jumpRay, Color.red);
        Debug.DrawRay(transform.position, Vector3.forward * ladderRay, Color.green);
    }

    // Moves the player by a constant force with mass in a normalized direction given by wasd input
    private void Move(Vector2 direction)
    {
        Vector3 moveDirection;
        if (IsOnLadder() && direction.y>0f)
        {
            moveDirection = rb.rotation * new Vector2(direction.x, direction.y).normalized;
            rb.AddForce(ladderSpeed * moveDirection, ForceMode.Impulse);
        }
        else if (!isGrounded)
        {
            moveDirection = rb.rotation * new Vector3(direction.x, 0f, direction.y).normalized;
            rb.AddForce(speed * moveDirection * airMultiplier, ForceMode.Impulse);
        }
        else 
        {
            moveDirection = rb.rotation * new Vector3(direction.x, 0f, direction.y).normalized;
            rb.AddForce(speed * moveDirection, ForceMode.Impulse);
        }
        
    }

    // Changes camera and player rotation from mouse movements
    private void Look(Vector2 lookInput)
    {
        if (!canLook) return;

        horizontalLook += lookInput.x * mouseSense;
        verticalLook -= lookInput.y * mouseSense;
        verticalLook = (verticalLook + 180) % 360 - 180;
        verticalLook = Mathf.Clamp(verticalLook, -90f, 90f);

        rb.MoveRotation(Quaternion.Euler(Vector3.up * horizontalLook));
        mainCam.transform.localEulerAngles = Vector3.right * verticalLook;
    }

    public void SetCanLook(bool value)
    {
        canLook = value;
    }

    // Moves the player by a constant force ignoring its mass upwards if grounded or if the player has a double jump
    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(jump * Vector3.up, ForceMode.VelocityChange);
            isGrounded = false;
        }
        else if (hasDoubleJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(jump * Vector3.up, ForceMode.VelocityChange);
            hasDoubleJump = false;
        }
    }

    // Moves the player by a constant force ignoring its mass in a normalized direction given by wasd input
    private void Dash()
    {
        if (canDash)
        {
            rb.AddForce(dash * mainCam.transform.forward, ForceMode.VelocityChange);
            StartCoroutine(DashCooldown());
        }
    }

    // Function to start a timer for when a player is able to dash 
    private IEnumerator DashCooldown()
    {
        canDash = false;

        if (dashText != null)
        {
            dashText.color = Color.red;  
            dashText.text = "Dash: Cooldown"; 
        }

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;

        if (dashText != null)
        {
            dashText.color = Color.white; 
            dashText.text = "Dash: Ready";
        }
    }

    // Extra physics calculations for player movement
    private void AuxiliaryMovement()
    {
        // Apply Drag
        rb.linearVelocity = new Vector3(rb.linearVelocity.x / (1 + airDrag), rb.linearVelocity.y, rb.linearVelocity.z / (1 + airDrag));

        // Apply Extra Gravity
        if (!isGrounded)
        {
            rb.AddForce(extraGravity * -transform.up, ForceMode.Acceleration);
        }
        IsGrounded();
    }

    // Updates isGrounded boolean using raycast to determine whether or not the player is touching the ground
    private void IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, jumpRay))
        {
            hasDoubleJump = true;
            isGrounded = true;
        }
        else if (IsOnLadder())
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    // Uses raycast to determine if the player is touching a ladder
    private bool IsOnLadder()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, ladderRay))
        {
            if(hit.collider.CompareTag("Ladder"))
            {
                return true;
            }
        }
        return false;
    }

    // Adds the player input listeners from the InputManager script
    private void AddInputs()
    {
        inputManager.OnMove.AddListener(Move);
        inputManager.OnLook.AddListener(Look);
        inputManager.OnSpacePressed.AddListener(Jump);
        inputManager.OnShiftPressed.AddListener(Dash);
        inputManager.OnMousePressed.AddListener(ShootPortal);
    }

    // Shoots the A portal and calls the PortalManager script SpawnPortalA function if it hits a portal wall
    private void ShootPortal(char portal)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(mainCam.transform.position, mainCam.transform.forward, Mathf.Infinity);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("PortalWall") && canShootPortal)
            {
                if (portal == 'a')
                {
                    portalManager.CreatePortalA(hits[i].collider.gameObject);
                    Instantiate(particlesA, hits[i].transform);
                    StartCoroutine(PortalCooldown());
                }
                else if (portal == 'b')
                {
                    portalManager.CreatePortalB(hits[i].collider.gameObject);
                    Instantiate(particlesB, hits[i].transform);
                    StartCoroutine(PortalCooldown());
                }
                SoundManager.instance.PlayAudioClip("PortalGunSound");
                break;
            }
        }
    }
            
    // Function to start a timer for when a player is able to dash 
    private IEnumerator PortalCooldown()
    {
        canShootPortal = false;
        yield return new WaitForSeconds(portalCooldown);
        canShootPortal = true;
    }

    // Sets the player rotation from a given quaternion
    public void SetRotationAndVelocity(Quaternion newRotation, Vector3 velocity)
    {
        Vector3 targetEuler = newRotation.eulerAngles;
        horizontalLook = targetEuler.y;
        verticalLook = targetEuler.x;
        rb.MoveRotation(Quaternion.Euler(Vector3.up * horizontalLook));
        mainCam.transform.localEulerAngles = Vector3.right * verticalLook;

        rb.linearVelocity = newRotation * (velocity.magnitude * Vector3.forward);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            SoundManager.instance.PlayAudioClip("PlayerDeathSound");
            GameManager.instance.PlayerDies();
        }
    }
    public void TakeKnockback(Vector3 direction,float force)
    {
        rb.AddForce(force * direction,ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.CompareTag("KillFloor"))
        {
            Player playerScript = hit.GetComponent<Player>();
            TakeDamage(1);
            transform.position = respawn.position;
            SoundManager.instance.PlayAudioClip("KillFloorSound");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().name == "TutorialScene" && other.CompareTag("Granny"))
        {
            GameManager.instance.PlayerWins();
        }
        else if (SceneManager.GetActiveScene().name == "LevelOneScene" && other.CompareTag("Granny"))
        {
            GameManager.instance.PlayerWins(); 
        }
        else if (SceneManager.GetActiveScene().name == "LevelThreeScene" && other.CompareTag("Granny"))
        {
            GameManager.instance.PlayerWins();
        }
    }
}