using UnityEngine;

public class Boss_Misile : MonoBehaviour
{
    public float m_time = 0;
    float m_timer = 0;
    float m_speed;
    Vector3[] points = new Vector3[4];

    public GameObject HitEffect;

    public void Initialize(Transform startPoint, Transform endPoint)
    {
        m_speed = Random.Range(1.5f, 3f);
        m_time = Random.Range(0.5f, 3f);
        points[0] = startPoint.position;
        points[1] = startPoint.position + (Random.Range(3f, 6f) * Random.Range(-1f, 1f) * startPoint.right) + (Random.Range(3f, 6f) * Random.Range(0, 1f) * startPoint.up) + (Random.Range(3f, 6f) * Random.Range(-1f, 1f) * startPoint.forward);
        points[2] = endPoint.position + (Random.Range(0f, 3f) * Random.Range(-1f, 1f) * startPoint.right) + (Random.Range(0f, 3f) * Random.Range(0, 1f) * startPoint.up) + (Random.Range(0f, 3f) * Random.Range(-1f, 1f) * startPoint.forward);
        points[3] = endPoint.position;
        transform.position = points[0];
        Invoke("timer_Bomb", m_time - m_time / 3);
    }
    void Update()
    {
        m_timer += Time.deltaTime * m_speed;

        transform.position = new Vector3(
            Bezier(points[0].x, points[1].x, points[2].x, points[3].x),
            Bezier(points[0].y, points[1].y, points[2].y, points[3].y),
            Bezier(points[0].z, points[1].z, points[2].z, points[3].z)
        );
    }

    public float Bezier(float a, float b, float c, float d)
    {
        float t = m_timer / m_time;
        float ab = Mathf.Lerp(a, b, t);
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Platform"))
        {
            GameObject Bomb = Instantiate(HitEffect, transform.position, Quaternion.identity);
            Bomb.GetComponent<Explosion>().Damage = 1;
            Bomb.GetComponent<Explosion>().Range = 3f;
            Destroy(gameObject);
        }
    }

    void timer_Bomb()
    {
        GameObject Bomb = Instantiate(HitEffect, transform.position, Quaternion.identity);
        Bomb.GetComponent<Explosion>().Damage = 1;
        Bomb.GetComponent<Explosion>().Range = 3f;
        Destroy(gameObject);
    }
}
