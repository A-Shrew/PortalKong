using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] List<GameObject> checkPoints;
    [SerializeField] Vector3 vectorPoint;
    [SerializeField] float dead = 5f; // Ensure default value

    void Start()
    {
        vectorPoint = player.transform.position; // Save initial position
    }

    void Update()
    {
        if (player.transform.position.y < -dead)
        {
            player.transform.position = vectorPoint; // Respawn at last checkpoint
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) 
        {
            vectorPoint = transform.position; // Save checkpoint
            Destroy(gameObject); // Destroy the cube when the player reaches it
        }
    }
}
