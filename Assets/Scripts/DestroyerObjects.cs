using UnityEngine;

public class DestroyerObjects : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        Invoke("DestroyThisAnimation", 2f);
    }

    void DestroyThisAnimation()
    {
        animator.SetTrigger("Destroy");
        Destroy(gameObject,2f);      
    }
}
