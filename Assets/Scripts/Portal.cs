using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Portal : MonoBehaviour
{
    public Player playerScript;
    public Transform player;
    public Camera playerCamera;

    public Transform targetPortal;
    public Transform targetPortalSpawn;
    public Transform targetPortalCamera;

    public Camera portalCamera;

    public bool hasAllVariables;
    public bool playerEnterPortal;

    void Awake()
    {
        portalCamera = GetComponentInChildren<Camera>();
        hasAllVariables = false;
        playerEnterPortal = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasAllVariables)
        {
            PositionCamera();
            OnTeleport();
        }
        else
        {
            CheckAllVariables();
        }
    }

    void PositionCamera()
    {
        Vector3 offset = player.transform.position - targetPortal.position;

        offset = transform.rotation * Quaternion.Inverse(targetPortal.rotation) * offset;

        portalCamera.transform.position = transform.position - offset;

        float horizontalAngle = Vector3.SignedAngle(transform.forward,targetPortal.forward,Vector3.up);
        Quaternion horizontalRotation = Quaternion.AngleAxis(-horizontalAngle, Vector3.up);

        float verticalAngle = playerCamera.transform.eulerAngles.x;
        Quaternion verticalRotation = Quaternion.AngleAxis(-verticalAngle, player.transform.right);

        Quaternion combinedRotation = horizontalRotation * verticalRotation;

        Vector3 direction = combinedRotation * -player.transform.forward;
        portalCamera.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    void OnTeleport()
    {
        if (playerEnterPortal)
        {
            Quaternion rotation = targetPortalCamera.rotation;
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);
          
            if (dotProduct < 0f)
            {
               
                player.position = new Vector3(targetPortalSpawn.position.x, player.position.y, targetPortalSpawn.position.z);

                playerScript = player.GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.SetRotation(rotation);
                    playerScript.SetVelocity();// Pass the target rotation
                }

                playerEnterPortal = false;
            }
        }
    }

    void CheckAllVariables()
    {
        if (targetPortal != null && player != null)
        {
            hasAllVariables = true;
        }
        else
        {
            hasAllVariables = false;
        } 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerEnterPortal = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerEnterPortal = false;
        }
    }
}