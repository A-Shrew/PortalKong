using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PortalWall : MonoBehaviour
{
    [SerializeField] private GameObject[] portals = new GameObject[2];
    [SerializeField] private int maxInception;
    public Camera portalCamera;
    public Camera mainCamera;


    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        UpdateCamera();
    }
    void UpdateCamera()
    {
        if (portals[0] != null || portals[1] != null)
        {
            return;
        }
        else
        {
            portalCamera = portals[0].GetComponent<Camera>();
            for (int i = maxInception - 1; i >= 0; --i)
            {
                RenderCamera(portals[0], portals[1], i);
            }
            portalCamera = portals[1].GetComponent<Camera>();
            for (int i = maxInception-1; i >= 0; --i)
            {
                RenderCamera(portals[1], portals[0], i);
            }
        
        }

  
    }

    void RenderCamera(GameObject inPortal, GameObject outPortal, int iteration)
    {
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        Transform cameraTransform = portalCamera.transform;
        cameraTransform.position = transform.position;
        cameraTransform.rotation = transform.rotation;

        for (int i = 0;i <= iteration; ++i)
        {
            var relativePosition = inTransform.InverseTransformPoint(cameraTransform.position);
            relativePosition = Quaternion.Euler(0f,180f,0f) *  relativePosition;
            cameraTransform.position = outTransform.TransformPoint(relativePosition);

            Quaternion relativeRotation = Quaternion.Inverse(inTransform.rotation) * cameraTransform.rotation;
            relativeRotation = Quaternion.Euler(0f, 180f, 0f) * relativeRotation;
            cameraTransform.rotation = outTransform.rotation * relativeRotation;
        }

        Plane p = new Plane(-outTransform.forward, outTransform.position);
        Vector4 clipPlaneWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;
        var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = newMatrix;
    }
}
