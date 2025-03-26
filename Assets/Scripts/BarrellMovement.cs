using UnityEngine;

public class BarrellMovement : MonoBehaviour
{
    private float angVel;
    private float angle;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angVel = Random.Range(1,10);
        angle = Random.Range(1,15);
        transform.Rotate(angle, 0f, 0f);
        Vector3 vec = new Vector3(0f, 0f, angVel);
        rb.angularVelocity -= vec;
    }
}
