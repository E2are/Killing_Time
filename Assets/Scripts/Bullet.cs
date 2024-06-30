using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rigid;
    public Vector3 dir;
    public bool isAlly;
    public float speed = 5f;
    public float D_Time = 3f;
    public float dmg;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Invoke("Disappear", D_Time);
    }
    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isAlly)
        {
            Disappear();
            Player player = other.gameObject.GetComponent<Player>();
            player.Damaged(dmg);
        }
        if (other.gameObject.tag == "Enemy" && isAlly)
        {
            Disappear();
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.Hp -= dmg;
        }
        if (other.gameObject.tag == "Base" && !isAlly)
        {
            Base bs = other.gameObject.GetComponent<Base>();
            bs.Damaged(dmg);
            Disappear();
        }
        if (other.gameObject.tag == "Platform")
        {
            Disappear();
        }
    }
    void Disappear()
    {
        Destroy(this.gameObject);
    }
}
