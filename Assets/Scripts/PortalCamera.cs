using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField] private Transform thisPortal;
    [SerializeField] private Transform targetPortal;
    [SerializeField] private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Offset = player.transform.position - targetPortal.position;
        transform.position = thisPortal.position + Offset;

        float angle = Quaternion.Angle(thisPortal.rotation, targetPortal.rotation);
        Quaternion angleRotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 direction = angleRotation * -player.transform.forward;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
