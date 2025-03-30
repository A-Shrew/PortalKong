using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BarrellMovement : MonoBehaviour
{
    [SerializeField] private GameObject parentBarrel;
    [SerializeField] private float force;
    [SerializeField] private float kbForce;
    [SerializeField] private int damage;

    private float angVel;
    private float angle;
    private bool inAir;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inAir = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.CompareTag("Ground"))
        {
            if (inAir)
            {
                inAir = false;
                rb.AddForce(force * parentBarrel.transform.right, ForceMode.VelocityChange);
            }
        }

        if (hit.CompareTag("Player"))
        {
            Player playerScript = hit.GetComponent<Player>();
            Vector3 kbDirection = hit.transform.position - transform.position;
            playerScript.TakeDamage(damage);
            playerScript.TakeKnockback(kbDirection, kbForce);
            Destroy(gameObject);
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
