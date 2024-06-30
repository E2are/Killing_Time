using UnityEngine;

public class Fix : MonoBehaviour
{
    public GameObject FixParticle;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.Hp = GameManager.Instance.MHp;
            GameManager.Instance.BaseHp = GameManager.Instance.BHp;
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject FixEffect = Instantiate(FixParticle, hit.point + new Vector3(0, 0.01f, 0), Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
