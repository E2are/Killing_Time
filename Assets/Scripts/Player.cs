using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager Gamemanager;
    public bool Dead;
    public PlayerMove PM;
    public PlayerShoot PS;
    void Start()
    {
        PM = GetComponent<PlayerMove>();
        PS = GetComponent<PlayerShoot>();
    }
    public void Damaged(float dmg)
    {
        Gamemanager.Damaged(dmg, true);
    }

    void Update()
    {
        if (!Dead && !GameManager.Instance.menued)
        {
            PM.Function();
            PS.Function();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "startGate") { 
            StartCoroutine(startWave());
            other.GetComponent<AudioSource>().Play();
            GameManager.Instance.GetComponent<AudioSource>().clip = GameManager.Instance.startbgm;
            
            Destroy(other.gameObject,2f);
            Instantiate(other.GetComponent<start_gate>().wall, other.transform.position + Vector3.forward * 2, Quaternion.Euler(0, 0, 0));
        }
    }

    IEnumerator startWave()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.WaveBoard.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.started = true;
        GameManager.Instance.GetComponent<AudioSource>().Play();
    }
}

