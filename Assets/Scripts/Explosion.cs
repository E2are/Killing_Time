using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float bTime = 0.5f;
    public bool EBomb;
    public float Damage;
    public float Range = 3.0f;
    bool hit = true;
    private void Start()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, Range);
        foreach (Collider i in colls)
        {
            if (i.gameObject.tag == "Player")
            {
                if (EBomb)
                {
                    i.gameObject.GetComponent<Player>().Damaged(Damage);
                    hit = false;
                }
                else
                {
                    i.gameObject.GetComponent<PlayerMove>().yVelocity += Damage/1.5f;
                }
            }
            if (i.gameObject.tag == "Base")
            {
                if (EBomb)
                {
                    i.gameObject.GetComponent<Base>().Damaged(Damage);
                    hit = false;
                }
            }
            if (i.gameObject.tag == "Enemy" && !EBomb)
            {
                if (!i.gameObject.name.Contains("Boss"))
                {
                    i.GetComponent<Enemy>().Hp -= Damage;
                    hit = false;
                }
                else
                {
                    i.GetComponent<Boss>().hp -= Damage;
                    hit = false;
                }
            }
        }
    }
    void Update()
    {
        if (bTime > 0)
        {
            bTime -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
