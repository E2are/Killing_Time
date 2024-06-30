using UnityEngine;

public class fireEffect : MonoBehaviour
{
    public float dTime = 0.2f;
    float ctime = 0;
    // Update is called once per frame
    void Update()
    {
        ctime += Time.deltaTime;
        if (ctime > dTime)
        {
            Destroy(this.gameObject);
        }
    }
}
