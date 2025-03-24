using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] private PortalManager portalManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jump;
    [SerializeField] private float gravity;
    [SerializeField] private float mouseSense;


    public Camera mainCam;
    public float rotationSmoothTime = 0.1f;
    private float horizontalLook;
    private float verticalLook;
    float smoothX;
    float smoothY;
    float xSmoothing;
    float ySmoothing;

    private Rigidbody rb;
    private float jumpRay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpRay = transform.localScale.y + 0.05f;
        inputManager.OnMove.AddListener(Move);
        inputManager.OnLook.AddListener(Look);
        inputManager.OnSpacePressed.AddListener(Jump);
        inputManager.OnMouseLeftPressed.AddListener(PortalA);
        inputManager.OnMouseRightPressed.AddListener(PortalB);

        smoothX = horizontalLook;
        smoothY = verticalLook;
    }
    void FixedUpdate()
    {
        Vector3 clampedSpeed = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        rb.linearVelocity = new Vector3(clampedSpeed.x,rb.linearVelocity.y,clampedSpeed.z);

        if (!IsGrounded())
        {
            rb.AddForce(gravity * -transform.up, ForceMode.Acceleration);
        }
    }
 
    private void Move(Vector2 direction)
    {
        Vector3 moveDirection = rb.rotation * new Vector3(direction.x,0f,direction.y);
        rb.AddForce(speed * moveDirection, ForceMode.Impulse);
    }

    private void Look(Vector2 lookInput)
    {
        horizontalLook += lookInput.x * mouseSense;
        verticalLook -= lookInput.y * mouseSense;
        verticalLook = (verticalLook + 180) % 360 - 180;
        verticalLook = Mathf.Clamp(verticalLook, -90f, 90f);
        smoothX = Mathf.SmoothDampAngle(smoothX, horizontalLook, ref xSmoothing, rotationSmoothTime);
        smoothY = Mathf.SmoothDampAngle(smoothY, verticalLook, ref ySmoothing, rotationSmoothTime);
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(Vector3.up * horizontalLook), .6f));
        mainCam.transform.localEulerAngles = Vector3.right * smoothY;
    }

    public void SetRotation(Quaternion newRotation)
    {
        Vector3 targetEuler = newRotation.eulerAngles;
        // Update look variables to prevent the Look() function from immediately overriding it
        horizontalLook = targetEuler.y;
        rb.rotation = Quaternion.Euler(0f, horizontalLook, 0f);

        verticalLook = targetEuler.x;
        mainCam.transform.localRotation = Quaternion.Euler(verticalLook, 0f, 0f);
    }

    public void SetVelocity()
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(speed * transform.forward, ForceMode.Impulse);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(jump * Vector3.up, ForceMode.VelocityChange);
        }
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, jumpRay))
        {
            return true;
        }
        return false;
    }

    private void PortalA()
    {
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject target = hit.collider.gameObject;
            if (target.CompareTag("PortalWall"))
            {
                portalManager.SpawnPortalA(target.transform);
            }
        }
    }

    private void PortalB()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject target = hit.collider.gameObject;
            if (target.CompareTag("PortalWall"))
            {
                portalManager.SpawnPortalB(target.transform);
            }
        }
    }
}
