using UnityEngine;

public class Base : MonoBehaviour
{
    public GameManager Gamemanager;
    public void Damaged(float dmg)
    {
        Gamemanager.Damaged(dmg, false);
    }
}
