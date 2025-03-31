using UnityEngine;
/* 
 * Portal script is placed on portal prefab
 * Each portal has its own portal script
 * Each portal has references to many different transforms and cameras.
 * Portal script also has a reference to the Player script to update player on teleport
 * Portal gets its refernces from PortalManager script
 * Portals can only be entered from its forward facing direction
*/
public class Portal : MonoBehaviour
{
    // Player References
    public Player playerScript; 
    public Transform player; 
    public Camera playerCamera; 

    // Target Portal (Other Portal) Refrences
    public Transform targetPortal;
    public Transform targetPortalSpawn;
    public Transform targetPortalCamera;

    // This Portal Camera Reference (What will be displayed on target portal)
    public Camera portalCamera;
    public Transform portalScreen;
    public bool hasAllVariables;
    public bool playerEnterPortal;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        portalCamera = GetComponentInChildren<Camera>();
        hasAllVariables = false;
        playerEnterPortal = false;
    }

    // FixedUpdate is called every fixed framerate frame

    void LateUpdate()
    {
        if (hasAllVariables)
        {
            PreventClipping();
            PositionCamera();
            OnTeleport();
        }     
    }

    // Orients portal camera with respect to the player portals relative position and rotation
    private void PositionCamera()
    {
        // Camera offset
        Vector3 offset = player.transform.position - targetPortal.position;
        offset = transform.rotation * Quaternion.Inverse(targetPortal.rotation) * offset;
        portalCamera.transform.position = transform.position - offset;

        // Camera rotation
        float horizontalAngle = Vector3.SignedAngle(transform.forward,targetPortal.forward,Vector3.up);
        Quaternion horizontalRotation = Quaternion.AngleAxis(-horizontalAngle, Vector3.up);

        float verticalAngle = playerCamera.transform.eulerAngles.x;
        Quaternion verticalRotation = Quaternion.AngleAxis(-verticalAngle, player.transform.right);

        Quaternion combinedRotation = horizontalRotation * verticalRotation;
        Vector3 direction = combinedRotation * -player.transform.forward;
        portalCamera.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        // Compute the clipping plane 
        Plane portalPlane = new Plane(transform.forward, transform.position);
        Vector4 clipPlaneCameraSpace = CameraSpacePlane(portalCamera, portalPlane.normal, portalPlane.ClosestPointOnPlane(portalCamera.transform.position));

        // Apply the new projection matrix
        portalCamera.projectionMatrix = playerCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
}
    // Clipping plane utility function
    private Vector4 CameraSpacePlane(Camera cam, Vector3 normal, Vector3 position)
    {
        // Convert the portal plane from world space to camera space
        Matrix4x4 matrix = cam.worldToCameraMatrix;
        Vector3 cameraSpaceNormal = matrix.MultiplyVector(normal).normalized;
        Vector3 cameraSpacePosition = matrix.MultiplyPoint(position);
        float distance = -Vector3.Dot(cameraSpaceNormal, cameraSpacePosition);

        return new Vector4(cameraSpaceNormal.x, cameraSpaceNormal.y, cameraSpaceNormal.z, distance);
    }

    private void PreventClipping()
    {
        float halfHeight = playerCamera.nearClipPlane * Mathf.Tan(playerCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * playerCamera.aspect;
        float distanceToNearClipPlane = new Vector3(halfWidth, halfHeight, playerCamera.nearClipPlane).magnitude;

        bool camFacingPortal = Vector3.Dot(transform.forward, transform.position - playerCamera.transform.position) < 0f;
        portalScreen.localScale = new Vector3(portalScreen.localScale.x, portalScreen.localScale.y, distanceToNearClipPlane);
        portalScreen.localPosition = Vector3.forward * distanceToNearClipPlane * ((camFacingPortal) ? 0.5f : -0.5f);
    }

    // Teleports the player and updates the player's rotation and velocity
    private void OnTeleport()
    {
        if (playerEnterPortal)
        {
            Quaternion rotation = targetPortalCamera.rotation;
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);

            if (dotProduct < 0.1f)
            {
                playerScript = player.GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.SetRotationAndVelocity(rotation, playerScript.rb.linearVelocity);
                }

                player.position = targetPortalSpawn.position;
                playerEnterPortal = false;
            }
        }
    }

    // Modifies boolean to be true if player enters portal collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerEnterPortal = true;
        }
    }

    // Modifies boolean to be false if player leaves the portal collider
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerEnterPortal = false;
        }
    }
}