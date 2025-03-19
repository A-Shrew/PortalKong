using System.Collections;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Camera camera;
    [SerializeField] private Transform portalLocation;
    [SerializeField] private Transform portalSpawnLocation;
    private bool canTeleport;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canTeleport = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.CompareTag("Player"))
        {
            if (canTeleport)
            {
                hit.transform.position = portalSpawnLocation.position;
            }
        }

    }

    private void OnCollisionLeave(Collision collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.CompareTag("Player"))
        {
            canTeleport = true;
        }
    }


}
