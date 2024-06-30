using UnityEngine;

public class Drop : MonoBehaviour
{
    Rigidbody rigid;
    public Vector3 dir;
    public float drop_Damage;
    public float dropDelay = 1;
    int dspeed = 1;
    [SerializeField] private int Maxspeed = 65;
    public GameObject DropEffect;

    public enum DropType
    {
        Turret = 1,
        Fix,
        Boss,
        e1,
        e2,
        e3
    }
    public int type;
    public GameObject[] DropObject;


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Invoke(nameof(destroy), 13f);
    }
    private void destroy()
    {
        if (type > 3)
        {
            GameManager.Instance.wm.enemy_Cnt -= 1;
        }
        Destroy(gameObject);
    }
    void Update()
    {
        if (dspeed > Maxspeed)
        {
            transform.Translate(dir * dspeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(dir * dspeed++ * Time.deltaTime);
        }
        if (Physics.Raycast(rigid.position, dir, 5f, LayerMask.GetMask("Platform")))
        {
            Ray ray = new Ray(rigid.position, dir);
            RaycastHit HitInfo = new RaycastHit();
            if (Physics.Raycast(ray, out HitInfo))
            {
                GameObject DEffct = Instantiate(DropEffect, HitInfo.point, Quaternion.Euler(0, 0, 0));
                if(type > 2)
                {
                    DEffct.GetComponent<Explosion>().EBomb = true; 
                }
                if(type == (int)DropType.Boss)
                {
                    DEffct.transform.localScale *= 2;
                }
                DEffct.GetComponent<Explosion>().Damage = drop_Damage;
                Instantiate(DropObject[type - 1], new Vector3(transform.position.x, HitInfo.point.y + 1f, transform.position.z), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                Destroy(this.gameObject);
            }
        }
    }
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.tag == "Platform")
    //     {

    //     }
    // }
}
