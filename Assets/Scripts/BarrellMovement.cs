using Unity.Mathematics;
using UnityEngine;

public class BarrellMovement : MonoBehaviour
{
    private float angVel;
    private float angle;
    private Rigidbody rb;
    [SerializeField] private float force;
    bool inAir = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angVel = UnityEngine.Random.Range(1, 10);
        angle = UnityEngine.Random.Range(1, 15);
        transform.Rotate(angle, 0f, 0f);
        Vector3 vec = new Vector3(0f, 0f, angVel);
        rb.angularVelocity -= vec;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (inAir)
            {
                inAir = false;
                transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                Vector3 right = Vector3.right;
                rb.AddForce(force * right, ForceMode.VelocityChange);
            }
        }
    }
}
