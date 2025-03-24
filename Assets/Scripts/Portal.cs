using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Portal : MonoBehaviour
{
    public Player playerScript;
    public Transform player;
    public Transform playerCamera;

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
    void Update()
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
        Vector3 Offset = player.transform.position - targetPortal.position;
        portalCamera.transform.position = transform.position + Offset;

        //// Finds and sets signed horizontal angle between the portals
        //Vector3 vecA = transform.rotation * Vector3.up;
        //Vector3 vecB = targetPortal.rotation * Vector3.up;
        //float angleA = Mathf.Atan2(vecA.x, vecA.z) * Mathf.Rad2Deg;
        //float angleB = Mathf.Atan2(vecB.x, vecB.z) * Mathf.Rad2Deg;
        //var angleDiff = Mathf.DeltaAngle(angleA, angleB);
    
        float horizontalAngle = Quaternion.Angle(transform.rotation, targetPortal.rotation);
        Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);

        float verticalAngle = playerCamera.eulerAngles.x;
        Quaternion verticalRotation = Quaternion.AngleAxis(-verticalAngle, player.transform.right);

        Quaternion combinedRotation = horizontalRotation * verticalRotation;

        Vector3 direction = combinedRotation * -player.transform.forward;
        portalCamera.transform.rotation = Quaternion.LookRotation(direction);
    }

    void OnTeleport()
    {
        if (playerEnterPortal)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
          
            if (dotProduct < 0f)
            {
                Quaternion rotation = targetPortalCamera.rotation;
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