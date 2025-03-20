using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PortalCamera : MonoBehaviour
{
    public Transform thisPortal;
    public Transform targetPortal;
    public Transform player;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (HasAllVariables())
        {
            Vector3 Offset = player.transform.position - targetPortal.position;
            transform.position = thisPortal.position + Offset;

            float angle = Quaternion.Angle(thisPortal.rotation, targetPortal.rotation);
            Quaternion angleRotation = Quaternion.AngleAxis(angle, Vector3.up);

            float verticalAngle = playerCamera.transform.eulerAngles.x;
            Quaternion verticalRotation = Quaternion.AngleAxis(-verticalAngle, player.transform.right);

            Quaternion combinedRotation = angleRotation * verticalRotation;

            Vector3 direction = combinedRotation * -player.transform.forward;
            transform.rotation = Quaternion.LookRotation(direction);
        }
       
    }
    bool HasAllVariables()
    {
        if(targetPortal != null && player != null)
        {
            return true;
        }
        return false;
    }
}
