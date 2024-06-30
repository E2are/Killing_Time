using UnityEngine;

public class PBullet : MonoBehaviour
{
    public float bullet_speed;
    public float destroyCool = 0f;
    public int dmg = 1;

    public Vector3 dir;
    public int BulletType = 1;
    public float splashLength;
    [System.Serializable]
    public struct objects
    {
        public GameObject hitParticlePrefab;
        public GameObject TurretPoint;
    }
    public objects objs;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, splashLength);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * bullet_speed * Time.deltaTime);
        destroyCool -= Time.deltaTime;

        if (destroyCool < 0)
        {
            switch (BulletType)
            {
                case 1:
                    GameManager.Instance.player.PS.MaxBullets.Add(gameObject);
                    gameObject.SetActive(false);
                    break;
                case 2:
                    GameManager.Instance.player.PS.MaxBazukas.Add(gameObject);
                    gameObject.SetActive(false);
                    break;
                case 3:
                    GameManager.Instance.player.PS.MaxTurretsign.Add(gameObject);
                    gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (BulletType == 1)
            {
                GameManager.Instance.Attackhit();
                if (collision.name.Contains("Boss"))
                {
                    if (collision.gameObject.GetComponent<Boss>().hp > 0)
                    {
                        collision.gameObject.GetComponent<Boss>().hp -= dmg;
                    }
                    else if(collision.gameObject.GetComponent<Boss>().hp <= 0)
                    {
                        collision.gameObject.GetComponent<Boss>().DeathStart();
                    }
                }
                else
                {
                    collision.gameObject.GetComponent<Enemy>().Hp -= dmg;
                }

                dir = (collision.gameObject.transform.position - transform.position).normalized;
                Ray ray = new Ray(transform.position + (-dir), dir);
                RaycastHit HitInfo = new RaycastHit();
                if (Physics.Raycast(ray, out HitInfo))
                {
                    GameObject hitEffect = Instantiate(objs.hitParticlePrefab);
                    hitEffect.transform.position = HitInfo.point;
                    hitEffect.transform.forward = HitInfo.normal;
                }
                GameManager.Instance.player.PS.MaxBullets.Add(gameObject);
                gameObject.SetActive(false);
            }
            if (BulletType == 2)
            {
                GameManager.Instance.Attackhit();
                Collider[] colliders = Physics.OverlapSphere(transform.position, splashLength);
                foreach (Collider i in colliders)
                {
                    if (i.gameObject.tag == "Enemy")
                    {
                        if (collision.name.Contains("Boss"))
                        {
                            if (collision.gameObject.GetComponent<Boss>().hp > 0)
                            {
                                collision.gameObject.GetComponent<Boss>().hp -= dmg;
                            }
                            else if (collision.gameObject.GetComponent<Boss>().hp <= 0)
                            {
                                collision.gameObject.GetComponent<Boss>().DeathStart();
                            }
                        }
                        else
                        {
                            i.gameObject.GetComponent<Enemy>().Hp -= dmg;
                        }
                    }
                }
                Instantiate(objs.hitParticlePrefab, transform.position, transform.rotation);
                GameManager.Instance.player.PS.MaxBazukas.Add(gameObject);
                gameObject.SetActive(false);
            }
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (BulletType == 1)
            {
                GameManager.Instance.player.PS.MaxBullets.Add(gameObject);
                gameObject.SetActive(false);
            }
            if (BulletType == 2)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, splashLength);
                foreach (Collider i in colliders)
                {
                    if (i.gameObject.tag == "Enemy")
                    {
                        if (i.gameObject.name.Contains("Boss"))
                        {
                            i.gameObject.GetComponent<Boss>().hp -= dmg;
                        }
                        else
                        {
                            i.gameObject.GetComponent<Enemy>().Hp -= dmg;
                        }
                    }
                }
                GameObject bullet = Instantiate(objs.hitParticlePrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Explosion>().Range = splashLength;
                bullet.GetComponent<Explosion>().Damage = dmg;
                GameManager.Instance.player.PS.MaxBazukas.Add(gameObject);
                gameObject.SetActive(false);
            }
            if (BulletType == 3)
            {
                GameObject Drop_Turret = Instantiate(objs.TurretPoint, new Vector3(transform.position.x + UnityEngine.Random.Range(-3f, 3), transform.position.y + 100f, transform.position.z + UnityEngine.Random.Range(-3f, 3f)), Quaternion.identity);
                Vector3 dropDir = (transform.position - Drop_Turret.transform.position).normalized;
                Drop_Turret.GetComponent<Drop>().dir = dropDir;
                Drop_Turret.GetComponent<Drop>().type = 1;
                Ray hit = new Ray(transform.position, Vector3.down);
                RaycastHit raycastHit = new RaycastHit();
                if (Physics.Raycast(hit, out raycastHit))
                {
                    GameObject DropEft = Instantiate(objs.hitParticlePrefab, raycastHit.point, Quaternion.Euler(0, 0, 0));
                }
                GameManager.Instance.player.PS.MaxTurretsign.Add(gameObject);
                gameObject.SetActive(false);
            }
        }

    }
}