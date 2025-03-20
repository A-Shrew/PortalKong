using System.Collections;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform player;
    public Transform targetPortal;
    private bool playerOverlapping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerOverlapping = false;
    }

    void Update()
    {
        if (HasAllVariables())
        {
            if (playerOverlapping)
            {
                Vector3 portalToPlayer = player.position - transform.position;
                float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

                if (dotProduct < 0f)
                {
                    float rotationDiff = Quaternion.Angle(transform.rotation, targetPortal.rotation);
                    rotationDiff += 180;
                    player.Rotate(Vector3.up, rotationDiff);

                    Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                    player.position = targetPortal.position + positionOffset;

                    playerOverlapping = false;
                }
            }
        }
    }

    bool HasAllVariables()
    {
        if (targetPortal != null && player != null)
        {
            return true;
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerOverlapping = false;
        }
    }
}
