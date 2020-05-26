using UnityEngine;

public class Cough : MonoBehaviour
{
    [Header("Ссылка на зону кашля:")]
    public GameObject coughingArea;
    [Header("Ссылка на звуки:")]
    public AudioSource cought;
    private int soundState;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        soundState = PlayerPrefs.GetInt("Sounds");
        animator = GetComponent<Animator>();
        float randomRepeatRate = Random.Range(4, 8);
        InvokeRepeating("MakeCoughingArea", 1f, randomRepeatRate);
    }

    // Update is called once per frame

    void MakeCoughingArea()
    {
        PlayCought();
        animator.SetInteger("canCought", 1);       
        Invoke("CancelCoughingArea", 3f);
        coughingArea.SetActive(true);
    }
    void CancelCoughingArea()
    {
        animator.SetInteger("canCought", 0);
        coughingArea.SetActive(false);
        CancelInvoke("CancelCoughingArea");
    }
    void PlayCought()
    {
        if (soundState == 0)
            cought.Play();
    }
}
