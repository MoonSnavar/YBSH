using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerObjects : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyThisAnimation", 2f);
    }

    void DestroyThisAnimation()
    {
        animator.SetTrigger("Destroy");
        Invoke("DestroyThis", 2f);        
    }
    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
