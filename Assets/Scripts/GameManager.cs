using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider hpState;
    public GameObject[] BaseState;
    public GameObject hit;
    public Text ScoreBoard;
    public Text WaveBoard;
    public Image Vazuka;
    public GameObject atkhit;
    public GameObject menu;
    public bool menued = false;

    public static GameManager Instance;
    public Player player;
    public Base bs;
    public WaveManager wm;
    public float Hp = 10;
    public float RespawnSpeed = 10f;
    public float BaseHp = 10;
    public GameObject GameOverUI;
    Animator GameOverAnim;

    public int Score = 0;
    public int wave = 0;
    public bool started = false;
    public AudioClip startbgm;
    [HideInInspector]
    public float MHp;
    [HideInInspector]
    public float BHp;
    float VazukaCooltime;
    bool over = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        MHp = Hp;
        BHp = BaseHp;
        GameOverAnim = GameOverUI.GetComponent<Animator>();
        WaveBoard.gameObject.SetActive(false);
        menu.SetActive(false);
    }
    void Update()
    {
        if (!over)
        {
            ScoreBoard.text = "Score : " + Score + "\nMaxScore : " + PlayerPrefs.GetInt("MaxScore");
            WaveBoard.text = "Wave : " + wave;
            hpState.value = Hp / MHp;
            Vazuka.fillAmount = VazukaCooltime / player.gameObject.GetComponent<PlayerShoot>().VazuShootRate;

            for (int i = 1; i <= BHp; i++)
            {
                if (BaseHp < i)
                {
                    BaseState[i - 1].GetComponent<Image>().color = new Color(190, 190, 190, 1f);
                }
                else
                {
                    BaseState[i - 1].GetComponent<Image>().color = new Color(0, 246, 255, 1f);
                }
            }

            if (Hp <= 0)
            {
                GameOver();
                Hp = 0;
            }
            if (BaseHp <= 0)
            {
                GameOverAnim.SetBool("GameOver", true);
                Hp = 0;
                Cursor.lockState = CursorLockMode.None;
            }

            if (player.Dead == true)
            {
                Hp += Time.deltaTime * RespawnSpeed;
            }
            if (Hp >= MHp)
            {
                player.Dead = false;
                Hp = MHp;
            }

            if (VazukaCooltime > 0)
            {
                VazukaCooltime -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menued = !menued;
                if (menued)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                menu.SetActive(menued);
            }
        }
    }
    public void Damaged(float dmg, bool isPlayer)
    {
        if (isPlayer)
        {
            if (!player.Dead)
            {
                Hp -= dmg;
                StartCoroutine(Hit());
            }
        }
        else
        {
            BaseHp -= dmg;
        }
    }
    public void setVazukaColtime(float vazuka)
    {
        VazukaCooltime = vazuka;
    }
    void GameOver()
    {
        player.Dead = true;
    }

    IEnumerator Hit()
    {
        hit.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hit.SetActive(false);
    }

    public void Attackhit()
    {
        atkhit.GetComponent<Animator>().SetTrigger("hit");
    }
}
