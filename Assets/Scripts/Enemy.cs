using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    Rigidbody rigid;
    Animator anim;

    bool operated = false;
    public int e_id;
    GameObject target;
    public bool basic;
    TrailRenderer trailRenderer;
    public bool bomb;
    public bool ranger;
    public bool targetisPlayer;
    public bool P_alive = true;
    public float Speed;
    float N_Speed;
    public float Hp;
    float MaxHP;
    public int Score;
    public float Dmg = 1f;
    public float Length;
    public GameObject Broken;
    public GameObject Boom_E;
    public GameObject Bullet;
    public bool Bash;
    public GameObject Dash_Effect;
    bool approaching = true;
    public Transform shotpoint;
    Vector3 dir;
    Slider Ehp;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        if (ranger)
        {
            anim = GetComponentInChildren<Animator>();
        }
        N_Speed = Speed;
        if (ranger)
        {
            StartCoroutine(type_ranger());
        }
        Ehp = GetComponentInChildren<Slider>();
        MaxHP = Hp;
        Invoke(nameof(operate), 1f);
        if (basic)
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }
        
    }
    void Update()
    {
        Ehp.value = (float)Hp / (float)MaxHP;
        Ehp.gameObject.transform.forward = Camera.main.transform.forward;
        if (operated)
        {
            Collider[] collider = Physics.OverlapSphere(transform.position, Length, LayerMask.GetMask("Player"));

            if (targetisPlayer && !GameManager.Instance.player.Dead)
            {
                if (collider.Length > 0 && basic)
                {
                    Bash = true;
                }
                else
                {
                    Bash = false;
                }
                target = GameManager.Instance.player.gameObject;
                dir = (target.transform.position - transform.position).normalized;
            }
            else
            {
                target = GameManager.Instance.bs.gameObject;
                dir = (target.transform.position - transform.position).normalized;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 3f);
            if (approaching && !Bash)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Speed * Time.deltaTime);
                
            }
            else if (basic && Bash)
            {
                trailRenderer.enabled = true;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Speed * 3 * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 12f);
            }
            else if(basic && !Bash)
            {
                trailRenderer.enabled = false;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Speed * Time.deltaTime);
            }

            if (Hp < 1)
            {
                if (bomb)
                {
                    GameObject Boom = Instantiate(Boom_E, transform.position, Quaternion.identity);
                    Boom.GetComponent<Explosion>().Damage = Dmg;
                    Boom.GetComponent<Explosion>().Range = Length;
                }
                else
                {
                    GameObject Boom = Instantiate(Broken, transform.position, Quaternion.identity);
                }
                GameManager.Instance.Score += Score;
                GameManager.Instance.wm.enemy_Cnt--;
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (basic)
            {
                Debug.Log($"boom");
                player.Damaged(Dmg);
                Destroy(this.gameObject);
            }
            if (bomb)
            {
                GameObject Boom = Instantiate(Boom_E, transform.position, Quaternion.identity);
                Boom.GetComponent<Explosion>().Damage = Dmg;
                Boom.GetComponent<Explosion>().Range = Length;
                Destroy(this.gameObject);
            }
            GameManager.Instance.wm.enemy_Cnt--;
        }
        if (other.gameObject.tag == "Base")
        {
            Base bs = other.gameObject.GetComponent<Base>();

            if (basic)
            {
                Debug.Log($"hit");
                bs.Damaged(Dmg);
                Destroy(this.gameObject);
            }

            if (bomb)
            {
                GameObject Boom = Instantiate(Boom_E, transform.position, Quaternion.identity);
                Boom.GetComponent<Explosion>().Damage = Dmg;
                Boom.GetComponent<Explosion>().Range = Length;
                Destroy(this.gameObject);
            }
            GameManager.Instance.wm.enemy_Cnt--;
        }
        
    }

    IEnumerator type_ranger()
    {
        while (true)
        {
            Collider[] collider = Physics.OverlapSphere(transform.position, Length, LayerMask.GetMask("Player"));
            if (collider.Length > 0)
            {
                approaching = false;
                anim.SetTrigger("shot");
                Bullet.GetComponent<Bullet>().dir = (dir).normalized;
                Bullet.GetComponent<Bullet>().dmg = Dmg;
                GameObject bullet = Instantiate(Bullet, shotpoint.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                approaching = true;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    void operate()
    {
        operated = true;
    }
}
