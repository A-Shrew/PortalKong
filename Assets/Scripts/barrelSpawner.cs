using UnityEngine;

public class barrelSpawner : MonoBehaviour
{
    public GameObject barrel;
    public float spawnTime;
    public float destroyTime;
    Quaternion spawnDirection;

    private void Start()
    {
        InvokeRepeating("spawnBarrel", 1f, spawnTime);
    }
    public void spawnBarrel()
    {
        GameObject brl = Instantiate(barrel, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(brl, destroyTime);
    }
}
