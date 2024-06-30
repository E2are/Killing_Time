using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    Animator anim;
    AudioSource gunsounds;
    public int weaponType = 1;
    [SerializeField] GameObject[] Weapons;
    public enum WeaponType
    {
        bullet = 1,
        bazuka,
        drop
    }
    public GameObject Bullet;
    public GameObject Bazuka;
    public GameObject TurretDrop;
    public Transform BulletPoint;
    float ShootCooltime = 0f;
    public float ShootRate = 0.1f;
    float VazuShootCooltime = 0f;
    public float VazuShootRate = 4f;
    public int requireScore = 100;
    public List<GameObject> MaxBullets = new List<GameObject>();
    public List<GameObject> MaxBazukas = new List<GameObject>();
    public List<GameObject> MaxTurretsign = new List<GameObject>();
    public int MaxBulletsCount = 20;
    public GameObject Shoot;
    GameObject ShootPrefab;
    void Start()
    {
        anim = GetComponent<Animator>();
        for (int i = 0; i < MaxBulletsCount * 2; i++)
        {
            GameObject bullet = Instantiate(Bullet);

            MaxBullets.Add(bullet);

            bullet.SetActive(false);
        }
        for (int i = 0; i < MaxBulletsCount; i++)
        {
            GameObject bullet = Instantiate(Bazuka);

            MaxBazukas.Add(bullet);

            bullet.SetActive(false);
        }
        for (int i = 0; i < MaxBulletsCount; i++)
        {
            GameObject bullet = Instantiate(TurretDrop);

            MaxTurretsign.Add(bullet);

            bullet.SetActive(false);
        }
    }

    public void Function()
    {
        if (Input.GetKeyDown("1"))
        {
            weaponType = (int)WeaponType.bullet;
            anim.SetTrigger("Swap");
        }
        if (Input.GetKeyDown("2"))
        {
            weaponType = (int)WeaponType.bazuka;
            anim.SetTrigger("Swap");
        }
        if (Input.GetKeyDown("3"))
        {
            if (GameManager.Instance.Score >= requireScore)
            {
                weaponType = (int)WeaponType.drop;
                anim.SetTrigger("Swap");
            }
            else
            {
                weaponType = 1;
                anim.SetTrigger("Swap");
            }
        }
        BulletPoint = Weapons[weaponType - 1].transform.GetChild(0).GetComponent<Transform>();
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (weaponType - 1 == i)
            {
                Weapons[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                Weapons[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        if (ShootCooltime < 0)
        {
            if (Input.GetMouseButton(0) && weaponType == (int)WeaponType.bullet && MaxBullets.Count > 0)
            {
                gunsounds = Weapons[weaponType - 1].GetComponent<AudioSource>();
                gunsounds.Play();
                Vector3 shootDirection = Camera.main.transform.forward;
                Quaternion bulletRotation = Quaternion.LookRotation(shootDirection);

                ShootPrefab = MaxBullets[0];
                MaxBullets.RemoveAt(0);
                ShootPrefab = Instantiate(Shoot, BulletPoint.position, bulletRotation);
                ShootPrefab.transform.parent = gameObject.transform;

                GameObject Pbullet = Instantiate(Bullet, BulletPoint.position, bulletRotation);
                anim.SetBool("shooting", true);

                ShootCooltime = ShootRate;
            }
            if (Input.GetMouseButton(0) && weaponType == 3 && MaxTurretsign.Count > 0)
            {
                gunsounds = Weapons[weaponType - 1].GetComponent<AudioSource>();
                gunsounds.Play();
                Vector3 shootDirection = Camera.main.transform.forward;
                Quaternion bulletRotation = Quaternion.LookRotation(shootDirection);

                ShootPrefab = MaxTurretsign[0];
                MaxTurretsign.RemoveAt(0);

                ShootPrefab = Instantiate(Shoot, BulletPoint.position, bulletRotation);
                ShootPrefab.transform.parent = gameObject.transform;

                GameObject Pbullet = Instantiate(TurretDrop, BulletPoint.position, bulletRotation);
                anim.SetBool("shooting", true);

                weaponType = 1;
                requireScore += 50 * GameManager.Instance.wave;
                anim.SetTrigger("Swap");
                ShootCooltime = ShootRate;
            }
        }
        else
        {
            ShootCooltime -= Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("shooting", false);
        }

        if (VazuShootCooltime < 0 && weaponType == 2)
        {
            if (Input.GetMouseButtonDown(0) && MaxBazukas.Count > 0)
            {
                gunsounds = Weapons[weaponType - 1].GetComponent<AudioSource>();
                gunsounds.Play();
                Vector3 shootDirection = Camera.main.transform.forward;
                Quaternion bulletRotation = Quaternion.LookRotation(shootDirection);
                ShootPrefab = MaxBazukas[0];
                MaxBazukas.RemoveAt(0);

                ShootPrefab = Instantiate(Shoot, BulletPoint.position, bulletRotation);
                ShootPrefab.transform.parent = gameObject.transform;

                GameObject Pbullet = Instantiate(Bazuka, BulletPoint.position, bulletRotation);

                VazuShootCooltime = VazuShootRate;
                weaponType = 1;

                GameManager.Instance.setVazukaColtime(VazuShootCooltime);
            }
        }
        else
        {
            VazuShootCooltime -= Time.deltaTime;
        }
    }
}