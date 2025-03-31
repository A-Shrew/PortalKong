using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public GameObject prefabA;
    public GameObject prefabB;

    private Portal portalA;
    private Portal portalB;
    public Collider portalWallA;
    public Collider portalWallB;
    private GameObject[] portals = new GameObject[2];

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        portals[0] = null;
        portals[1] = null;
    }

    // Update is called every frame
    void LateUpdate()
    {
        PortalConnection(); 
    }

    // Pain
    public void CreatePortalA(GameObject wall)
    {
        Collider wallCollider = wall.GetComponent<Collider>();
        if (portals[0] == null && portals[1] == null)
        {
            portals[0] = Instantiate(prefabA, wall.transform.position, wall.transform.rotation);
            portalWallA = wallCollider;
        }
        else if (portals[0] != null && portals[1] != null)
        {
            if (portals[0].transform.position != wall.transform.position && portals[1].transform.position != wall.transform.position)
            {
                Destroy(portals[0]);
                portalWallA.isTrigger = false;
                portals[0] = Instantiate(prefabA, wall.transform.position, wall.transform.rotation);
                portalWallA = wallCollider;
            }
            else if (portals[1].transform.position == wall.transform.position)
            {
                Destroy(portals[0]);
                Destroy(portals[1]);
                portalWallA.isTrigger = false;
                portals[0] = Instantiate(prefabA, wall.transform.position, wall.transform.rotation);
                portalWallA = wallCollider;
                portalWallA.isTrigger = false;
            }
        }
        else if (portals[0] != null)
        {
            if (portals[0].transform.position != wall.transform.position)
            {
                Destroy(portals[0]);
                portalWallA.isTrigger = false;
                portals[0] = Instantiate(prefabA, wall.transform.position, wall.transform.rotation);
                portalWallA = wallCollider;
            }
        }
        else if (portals[1] != null)
        {
            if (portals[1].transform.position == wall.transform.position)
            {
                Destroy(portals[1]);
                portalWallB.isTrigger = false;
                portals[0] = Instantiate(prefabA, wall.transform.position, wall.transform.rotation);
                portalWallA = wallCollider;
            }
            else
            {
                portals[0] = Instantiate(prefabA, wall.transform.position, wall.transform.rotation);
                portalWallA = wallCollider;
            }
        }
        portalA = portals[0].GetComponent<Portal>();
    }

    // More Pain
    public void CreatePortalB(GameObject wall)
    {
        Collider wallCollider = wall.GetComponent<Collider>();
        if (portals[0] == null && portals[1] == null)
        {
            portals[1] = Instantiate(prefabB, wall.transform.position, wall.transform.rotation);
            portalWallB = wallCollider;
        }
        else if (portals[0] != null && portals[1] != null)
        {
            if (portals[1].transform.position != wall.transform.position && portals[0].transform.position != wall.transform.position)
            {
                Destroy(portals[1]);
                portalWallB.isTrigger = false;
                portals[1] = Instantiate(prefabB, wall.transform.position, wall.transform.rotation);
                portalWallB = wallCollider;
            }
            else if (portals[0].transform.position == wall.transform.position)
            {
                Destroy(portals[0]);
                Destroy(portals[1]);
                portalWallB.isTrigger = false;
                portals[1] = Instantiate(prefabB, wall.transform.position, wall.transform.rotation);
                portalWallB = wallCollider;
                portalWallB.isTrigger = false;
            }
        }
        else if (portals[1] != null)
        {
            if (portals[1].transform.position != wall.transform.position)
            {
                Destroy(portals[1]);
                portalWallB.isTrigger = false;
                portals[1] = Instantiate(prefabB, wall.transform.position, wall.transform.rotation);
                portalWallB = wallCollider;
            }
        }
        else if (portals[0] != null)
        {
            if (portals[0].transform.position == wall.transform.position)
            {
                Destroy(portals[0]);
                portalWallA.isTrigger = false;
                portals[1] = Instantiate(prefabB, wall.transform.position, wall.transform.rotation);
                portalWallB = wallCollider;
            }
            else
            {
                portals[1] = Instantiate(prefabB, wall.transform.position, wall.transform.rotation);
                portalWallB = wallCollider;
            }
        }
        portalB = portals[1].GetComponent<Portal>();
    }

    // Public script to destroy portal A
    public void DestroyPortalA()
    {
        Destroy(portals[0]);
    }

    // Public script to destroy portal B
    public void DestroyPortalB()
    {
        Destroy(portals[1]);
    }

    // Sends transform and camera refereces to portals if two exist and can be connected
    private void PortalConnection()
    {
        if (portals[0] != null && portals[1] != null)
        {
            gameManager.cameraA = portals[0].GetComponentInChildren<Camera>();
            gameManager.cameraB = portals[1].GetComponentInChildren<Camera>();
            gameManager.LoadTextures();

            portalA.playerScript = player.GetComponent<Player>();
            portalB.playerScript = player.GetComponent<Player>();

            portalA.player = player.GetComponent<Transform>();
            portalB.player = player.GetComponent<Transform>();

            portalA.playerCamera = player.GetComponentInChildren<Camera>();
            portalB.playerCamera = player.GetComponentInChildren<Camera>();

            portalA.targetPortal = portals[1].GetComponent<Transform>();
            portalB.targetPortal = portals[0].GetComponent<Transform>();

            portalA.targetPortalSpawn = portals[1].GetComponent<Transform>().GetChild(0).transform;
            portalB.targetPortalSpawn = portals[0].GetComponent<Transform>().GetChild(0).transform;

            portalA.targetPortalCamera = portals[1].GetComponentInChildren<Camera>().transform;
            portalB.targetPortalCamera = portals[0].GetComponentInChildren<Camera>().transform;

            portalWallA.isTrigger = true;
            portalWallB.isTrigger = true;
        }
    }
}
