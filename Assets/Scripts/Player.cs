using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [Header("References")]
    public Camera mainCam;
    [SerializeField] private PortalManager portalManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private ParticleSystem particlesA;
    [SerializeField] private ParticleSystem particlesB;

    [Header("Player Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float ladderSpeed;
    [SerializeField] private float jump;
    [SerializeField] private float dash;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float portalCooldown;

    [Header("Player Physics")]
    [SerializeField] private float extraGravity;
    [SerializeField] private float airDrag;
    [SerializeField] private float mouseSense;


    //Camera Stuff
    private float rotationSmoothTime;
    private float horizontalLook;
    private float verticalLook;
    private float horizontalSmoothing;
    private float verticalSmoothing;
    private float xSmoothReference;
    private float ySmoothReference;

    //Player Stuff
    public Rigidbody rb;
    private float jumpRay;
    private float ladderRay;
    private bool isGrounded;
    private bool canDash;
    private bool canShootPortal;
    private bool hasDoubleJump;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        AddInputs();
        rb = GetComponent<Rigidbody>();
        jumpRay = transform.localScale.y + 0.05f;
        ladderRay = transform.localScale.x + 0.05f;
        rotationSmoothTime = 0.1f;
        horizontalSmoothing = horizontalLook;
        verticalSmoothing = verticalLook;
        isGrounded = true;
        canDash = true;
        canShootPortal = true;
    }

    // FixedUpdate is called every fixed framerate frame
    void FixedUpdate()
    {
        AuxiliaryMovement();
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
        else
        {
            moveDirection = rb.rotation * new Vector3(direction.x, 0f, direction.y).normalized;
            rb.AddForce(speed * moveDirection, ForceMode.Impulse);
        }
        
    }

    // Changes camera and player rotation from mouse movements
    private void Look(Vector2 lookInput)
    {
        horizontalLook += lookInput.x * mouseSense;
        verticalLook -= lookInput.y * mouseSense;
        verticalLook = (verticalLook + 180) % 360 - 180;
        verticalLook = Mathf.Clamp(verticalLook, -90f, 90f);
        horizontalSmoothing = Mathf.SmoothDampAngle(horizontalSmoothing, horizontalLook, ref xSmoothReference, rotationSmoothTime);
        verticalSmoothing = Mathf.SmoothDampAngle(verticalSmoothing, verticalLook, ref ySmoothReference, rotationSmoothTime);
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(Vector3.up * horizontalLook), .6f));
        mainCam.transform.localEulerAngles = Vector3.right * verticalSmoothing;
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
    private void Dash(Vector2 direction)
    {
        if (canDash)
        {
            Vector3 moveDirection = rb.rotation * new Vector3(direction.x, 0f, direction.y).normalized;
            rb.AddForce(dash * moveDirection, ForceMode.VelocityChange);
            rb.AddForce(dash / 10 * Vector3.up, ForceMode.VelocityChange);
            StartCoroutine(DashCooldown());
        }
    }

    // Function to start a timer for when a player is able to dash 
    private IEnumerator DashCooldown()
    {
        canDash = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
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

    // Boolean using raycast to determing whether or not the player is touching the ground
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
        rb.rotation = Quaternion.Euler(0f, horizontalLook, 0f);

        verticalLook = targetEuler.x;
        mainCam.transform.localRotation = Quaternion.Euler(verticalLook, 0f, 0f);

        rb.linearVelocity = newRotation * (velocity.magnitude * Vector3.forward);
    }
}