using UnityEngine;

public class barrelSpawner : MonoBehaviour
{
    public GameObject spawner;
    public GameObject barrel;
    public float spawnTime;
    public float destroyTime;

    private void Start()
    {
        InvokeRepeating("spawnBarrel", 1f, spawnTime);
    }
    public void spawnBarrel()
    {
        GameObject brl = Instantiate(barrel);
        brl.transform.position = spawner.transform.position;
        Destroy(brl, destroyTime);
    }
}
