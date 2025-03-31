using System;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject prefabA;
    [SerializeField] private GameObject prefabB;

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
        if(CheckConnection(portalA) && CheckConnection(portalB))
        {
            portalA.hasAllVariables = true;
            portalB.hasAllVariables = true;
            portalWallA.isTrigger = true;
            portalWallB.isTrigger = true;
        }
        else
        {
            PortalConnection();
        }
    }

    // Instantiates portal A on wall
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
                portalWallB.isTrigger = false;
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
        portalA.hasAllVariables = false;
    }

    // Instantiates portal B on wall
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
                portalWallA.isTrigger = false;
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
        portalB.hasAllVariables = false;
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

            portalA.portalScreen = portals[1].GetComponent<Transform>().GetChild(1).transform;
            portalB.portalScreen = portals[0].GetComponent<Transform>().GetChild(1).transform;
        }
    }

    // Checks if all portal variables are connected
    private bool CheckConnection(Portal portal)
    {
        try
        {
            if (portal.playerScript == null || portal.player == null || portal.playerCamera == null || portal.targetPortal == null || portal.targetPortalSpawn == null || portal.targetPortalCamera == null || portal.portalScreen == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
}
