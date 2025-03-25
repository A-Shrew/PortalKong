
using System;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public GameObject prefabA;
    public GameObject prefabB;

    private Portal portalA;
    private Portal portalB;
    private GameObject[] portals = new GameObject[2];

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        portals[0] = null;
        portals[1] = null;
    }
    // Update is called every frame
    void Update()
    {
        PortalConnection(); 
    }

    // Instantiates portal A at a given location
    public void SpawnPortalA(Transform location)
    {
        if (portals[0] != null)
        {
            Destroy(portals[0]);

            if (portals[1] != null && portals[1].transform.position == location.position)
            {
                Destroy(portals[1]);
            }
            portals[0] = Instantiate(prefabA, location.position, location.rotation);
        }
        else
        {
            if (portals[1] != null && portals[1].transform.position == location.position)
            {
                Destroy(portals[1]);
            }
            portals[0] = Instantiate(prefabA, location.position, location.rotation);
        }
        portalA = portals[0].GetComponent<Portal>();
    }


    // Instantiates portal B at a given location
    public void SpawnPortalB(Transform location){
        if (portals[1] != null)
        {
            Destroy(portals[1]);

            if (portals[0] != null && portals[0].transform.position == location.position)
            {
                Destroy(portals[0]);
            }
            portals[1] = Instantiate(prefabB, location.position, location.rotation);
        }
        else
        {
            if (portals[0] != null && portals[0].transform.position == location.position)
            {
                Destroy(portals[0]);
            }
            portals[1] = Instantiate(prefabB, location.position, location.rotation);
        }
        portalB = portals[1].GetComponent<Portal>();
    }

    // Sends transform and camera refereces to portals if two exist and can be connected
    void PortalConnection()
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
        }
    }
}
