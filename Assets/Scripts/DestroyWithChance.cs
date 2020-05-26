using UnityEngine;

public class DestroyWithChance : MonoBehaviour
{
    //[Range(0, 1)]
    //public float chanceOfStaying = 0.5f;
    [Header("Characters settings")]
    public bool isCoughtintEnemy;    
    public bool isAIEnemy;
    public bool isHorWalkingEnemy;
    public bool isStandingEnemy;
    public bool isHeal;
    public bool isCurrencyPoints;

    public static float chanceOfStayingCE;
    public static float chanceOfStayingAI;
    public static float chanceOfStayingHor;
    public static float chanceOfStayingSt;
    public static float chanceOfStayingHeal;
    public static float chanceOfStayingCP;
    private void Start()
    {
        float chanceOfStaying = 0.5f;

        if (isCoughtintEnemy)
            chanceOfStaying = chanceOfStayingCE;
        else if (isAIEnemy)
            chanceOfStaying = chanceOfStayingAI;
        else if (isAIEnemy)
            chanceOfStaying = chanceOfStayingHor;
        else if (isAIEnemy)
            chanceOfStaying = chanceOfStayingSt;
        else if (isAIEnemy)
            chanceOfStaying = chanceOfStayingHeal;
        else if (isAIEnemy)
            chanceOfStaying = chanceOfStayingCP;

        if (Random.value > chanceOfStaying) Destroy(gameObject);
    }
}
