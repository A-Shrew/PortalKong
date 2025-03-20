
using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class PortalManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;
    public GameObject portalA;
    public GameObject portalB;
    public GameObject[] portals = new GameObject[2];

    public bool isConnected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        portals[0] = null;
        portals[1] = null;
        isConnected = false;
    }

    void Update()
    {
        PortalConnection(); 
    }

    public void SpawnPortalA(Transform location)
    {
        if (portals[0] != null)
        {
            Destroy(portals[0]);

            if (portals[1] != null && portals[1].transform.position == location.position)
            {
                Destroy(portals[1]);
            }
            portals[0] = Instantiate(portalA, location.position, Quaternion.Euler(location.right * -90f));
        }
        else
        {
            if (portals[1] != null && portals[1].transform.position == location.position)
            {
                Destroy(portals[1]);
            }
            portals[0] = Instantiate(portalA, location.position, Quaternion.Euler(location.right * -90f));
        }
    }

    public void SpawnPortalB(Transform location){
        if (portals[1] != null)
        {
            Destroy(portals[1]);

            if (portals[0] != null && portals[0].transform.position == location.position)
            {
                Destroy(portals[0]);
            }
            portals[1] = Instantiate(portalB, location.position, Quaternion.Euler(location.right * -90f));
        }
        else
        {
            if (portals[0] != null && portals[0].transform.position == location.position)
            {
                Destroy(portals[0]);
            }
            portals[1] = Instantiate(portalB, location.position, Quaternion.Euler(location.right * -90f));
        }
    }

    void PortalConnection()
    {
        if (portals[0] != null && portals[1] != null)
        {
            Debug.Log("TWO PORTALS");
            PortalCamera portalCamA = portals[0].GetComponentInChildren<PortalCamera>();
            PortalTeleport portalTeleA = portals[0].GetComponentInChildren<PortalTeleport>();

            PortalCamera portalCamB = portals[1].GetComponentInChildren<PortalCamera>();
            PortalTeleport portalTeleB = portals[1].GetComponentInChildren<PortalTeleport>();

            gameManager.cameraA = portals[0].GetComponentInChildren<Camera>();
            gameManager.cameraB = portals[1].GetComponentInChildren<Camera>();

            gameManager.LoadTextures();

            portalCamA.player = player.GetComponent<Transform>();
            portalCamB.player = player.GetComponent<Transform>();

            portalCamA.targetPortal = portals[1].GetComponent<Transform>();
            portalCamB.targetPortal = portals[0].GetComponent<Transform>();

            portalTeleA.player = player.GetComponent<Transform>();
            portalTeleB.player = player.GetComponent<Transform>();

            portalTeleA.targetPortal = portals[1].GetComponent<Transform>();
            portalTeleB.targetPortal = portals[0].GetComponent<Transform>();

        }
    }
}
