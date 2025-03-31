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
}
