using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Drop;
    public float spawnLength;
    public void SpawnStart()
    {
        int i = Random.Range(0, 3);
        GameObject enemy = Instantiate(Drop, new Vector3(Random.Range(transform.position.x - spawnLength, transform.position.x + spawnLength), 100 + Random.Range(transform.position.y + 1f, transform.position.y + spawnLength), Random.Range(transform.position.z - spawnLength, transform.position.z + spawnLength)), Quaternion.identity);
        enemy.GetComponent<Drop>().type = i+4;
        enemy.GetComponent<Drop>().dir = Vector3.down;
    }
}
