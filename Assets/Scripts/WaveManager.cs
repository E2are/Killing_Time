using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public EnemySpawner[] ESpawners;
    public int Spawntimes = 10;
    public int wave = 1;
    int spawntime = 10;
    public int enemy_Cnt = 10;
    public GameObject Drop;
    [SerializeField] private int DropSpawnTime = 3;
    public GameObject waveBoard;
    public void Start()
    {
        enemy_Cnt = spawntime;
        StartCoroutine(Spawning());
        GameManager.Instance.wave = wave;
    }
    IEnumerator Spawning()
    {
        while (true)
        {
            if (GameManager.Instance.started)
            {
                if (spawntime <= 0 && enemy_Cnt <= 0)
                {
                    waveBoard.GetComponent<Animator>().SetBool("changing", true);
                    yield return new WaitForSeconds(2f);
                    spawntime = Spawntimes + wave++;
                    enemy_Cnt = spawntime;
                    GameManager.Instance.wave = wave;
                    SpawnItem();
                    yield return new WaitForSeconds(3f);
                    waveBoard.GetComponent<Animator>().SetBool("changing", false);
                }
                else
                {
                    foreach (EnemySpawner enemySpawner in ESpawners)
                    {
                        if (spawntime > 0)
                        {
                            enemySpawner.SpawnStart();

                            spawntime--;
                        }
                        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
                    }
                }
                yield return null;
            }
            else
            {
                yield return null;
            }
        }
    }
    void SpawnItem()
    {
        if (wave % DropSpawnTime == 0)
        {
            GameObject drop = Instantiate(Drop, new Vector3(transform.position.x + Random.Range(10f, 50f), transform.position.y + 100f, transform.position.z + Random.Range(-3f, 3f)), Quaternion.identity);
            drop.GetComponent<Drop>().type = 2;
            drop.GetComponent<Drop>().dir = (new Vector3(GameManager.Instance.bs.transform.position.x - Random.Range(30f, 50f), 0, GameManager.Instance.bs.transform.position.z + Random.Range(-10f, 10f)) - drop.transform.position).normalized;
        }
        if (wave % 5 == 0)
        {
            GameObject drop = Instantiate(Drop, new Vector3(transform.position.x + Random.Range(-30f, 30f), transform.position.y + 100f, transform.position.z + Random.Range(-30f, 30f)), Quaternion.identity);
            drop.GetComponent<Drop>().type = 3;
            drop.GetComponent<Drop>().dir = (new Vector3(transform.position.x + Random.Range(-50f, 50f), 0, GameManager.Instance.bs.transform.position.z + Random.Range(-50f, 50f)) - drop.transform.position).normalized;
        }
    }
}
