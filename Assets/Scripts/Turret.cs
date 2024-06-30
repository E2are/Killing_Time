using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject TurretBullet;
    public float Dmg;
    public float Turret_AttackSpeed;
    public float bullet_speed;
    public float Turret_length;
    public float lastTime = 20f;
    Vector3 direction;
    void Start()
    {
        StartCoroutine(TurretOperate());
    }

    void Update()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, Turret_length, LayerMask.GetMask("Enemy"));
        lastTime -= Time.deltaTime;
        if (lastTime < 0)
        {
            StopCoroutine(TurretOperate());
            Destroy(this.gameObject);
        }
        if (collider.Length > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(collider[collider.Length - 1].transform.position - transform.position).normalized, Time.deltaTime * 5f);
        }
    }
    IEnumerator TurretOperate()
    {
        while (true)
        {
            Collider[] collider = Physics.OverlapSphere(transform.position, Turret_length, LayerMask.GetMask("Enemy"));

            if (collider.Length > 0)
            {
                direction = (collider[0].transform.position - transform.position + collider[0].transform.forward).normalized;
                GameObject bullet = Instantiate(TurretBullet, transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().isAlly = true;
                bullet.GetComponent<Bullet>().dir = direction;
                bullet.GetComponent<Bullet>().dmg = Dmg;
                bullet.GetComponent<Bullet>().speed = bullet_speed;
                yield return new WaitForSeconds(Turret_AttackSpeed);
            }
            yield return new WaitForSeconds(0f);
        }
    }
}
