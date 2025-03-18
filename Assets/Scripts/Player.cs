using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Camera cam;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jump;

    private float jumpRay;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpRay = transform.localScale.y + 0.05f;

        inputManager.OnMove.AddListener(Move);
        inputManager.OnSpacePressed.AddListener(Jump);
        inputManager.OnMouseLeftPressed.AddListener(ShootPortal1);
        inputManager.OnMouseRightPressed.AddListener(ShootPortal2);
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); 
        Debug.DrawRay(cam.transform.position, cam.transform.forward*5f,Color.yellow);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }

    // Update is called once per frame
    private void Move(Vector2 direction)
    {
        Vector3 moveDirection = rb.rotation * new Vector3(direction.x,0f,direction.y);
        rb.AddForce(speed * moveDirection, ForceMode.Impulse);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(jump * Vector3.up, ForceMode.Impulse);
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

    private void ShootPortal1()
    {

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject target = hit.collider.gameObject;
            if (target.CompareTag("PortalWall"))
            {

            }
        }
    }

    private void ShootPortal2()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject target = hit.collider.gameObject;
            if (target.CompareTag("PortalWall"))
            {

            }
        }
    }
}
