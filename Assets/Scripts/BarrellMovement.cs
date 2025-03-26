using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BarrellMovement : MonoBehaviour
{
    [SerializeField] private GameObject parentBarrel;
    private float angVel;
    private float angle;
    private Rigidbody rb;
    [SerializeField] private float force;
    bool inAir = true;
    Vector3 intialRotation;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (inAir)
            {
                inAir = false;
                rb.AddForce(force * parentBarrel.transform.right, ForceMode.Impulse);
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        StartCoroutine(SetFlying());
    }

    public void Flying()
    {
        angVel = UnityEngine.Random.Range(1, 10);
        angle = UnityEngine.Random.Range(-10, 10);
        parentBarrel.transform.rotation = Quaternion.Euler(angle, 0f, 0f);
        Vector3 vec = transform.forward * angVel;
        rb.angularVelocity -= vec;
    }

    private IEnumerator SetFlying()
    {
        yield return new WaitForSeconds(0.6f);
        inAir = true;
        rb.linearVelocity = Vector3.zero;
        Flying();
    }
}
