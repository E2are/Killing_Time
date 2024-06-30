using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    Animator anim;

    public GameObject target;
    public float hp;
    float MaxHp;
    public float damage;
    public float speed;
    public float atkDelay;
    float delay;
    Vector3 dir;
    [Space(10f)]
    [SerializeField]
    private float Patern_one_required_Distance;
    public float Patern_one_cooltime;
    float Patern_one_coltime;
    public int Patern_one_duration;
    public GameObject MisileObject;
    Slider hpSlider;
    bool dead = false;

    public GameObject DestroyEffect;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        target = GameObject.Find("Player");
        hpSlider = GetComponentInChildren<Slider>();
        MaxHp = hp;
    }

    void Update()
    {
        if (!dead)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > 2f)
            {
                move();
            }
            else
            {
                Attack();
            }
            if (Patern_one_coltime > Patern_one_cooltime && Vector3.Distance(target.transform.position, transform.position) < Patern_one_required_Distance)
            {
                Debug.Log("patern_one operated");
                Patern_one_coltime = 0;
                StartCoroutine(Patern1());
            }
            else
            {
                Patern_one_coltime += Time.deltaTime;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 6f);
            dir = (target.transform.position - transform.position).normalized;
        }
        hpSlider.value = hp/MaxHp;
        hpSlider.transform.forward = Camera.main.transform.forward;
    }

    void move()
    {
        anim.SetTrigger("walking");
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        delay += Time.deltaTime;
        if (delay > atkDelay)
        {
            delay = 0;
            anim.SetTrigger("Attack");
            StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(0.3f);
        Collider[] colliders = Physics.OverlapBox(transform.position + dir, new Vector3(5, 3, 6));

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Player")
            {
                Debug.Log("attaked");
                GameManager.Instance.player.Damaged(damage);
            }
        }
    }

    IEnumerator Patern1()
    {
        for (int i = 0; i < Patern_one_duration; i++)
        {
            Ray ray = new Ray(target.transform.position, Vector3.down);
            RaycastHit hit = new RaycastHit();
            GameObject Misile = Instantiate(MisileObject, transform.position, Quaternion.Euler(dir * -1));
            Misile.GetComponent<Boss_Misile>().Initialize(transform, target.transform);

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
    }
    public void DeathStart()
    {
        anim.applyRootMotion = true;
        StartCoroutine(DeathSequence());
    }
    public IEnumerator DeathSequence()
    {
        speed = 0;
        StopCoroutine(Patern1());
        yield return null;
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(2f);
        Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
