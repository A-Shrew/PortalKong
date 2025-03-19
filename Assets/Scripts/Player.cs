using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Camera mainCam;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jump;
    [SerializeField] private float gravity;
    [SerializeField] private float mouseSense;

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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpRay = transform.localScale.y + 0.05f;
        inputManager.OnMove.AddListener(Move);
        inputManager.OnLook.AddListener(Look);
        inputManager.OnSpacePressed.AddListener(Jump);

        smoothX = horizontalLook;
        smoothY = verticalLook;
    }
    void LateUpdate()
    {
        
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
        verticalLook = Mathf.Clamp(verticalLook, -90f, 90f);
        smoothX = Mathf.SmoothDampAngle(smoothX, horizontalLook, ref xSmoothing, rotationSmoothTime);
        smoothY = Mathf.SmoothDampAngle(smoothY, verticalLook, ref ySmoothing, rotationSmoothTime);
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(Vector3.up * horizontalLook),.6f));
        mainCam.transform.localEulerAngles = Vector3.right * smoothY;
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
}
