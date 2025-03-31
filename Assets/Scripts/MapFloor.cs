using UnityEngine;

public class MapFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.CompareTag("Player"))
        {
            Player playerScript = hit.GetComponent<Player>();
            playerScript.TakeDamage(1);
            playerScript.Respawn();
        }
    }
}
