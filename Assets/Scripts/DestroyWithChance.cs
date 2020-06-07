using UnityEngine;

public class DestroyWithChance : MonoBehaviour
{
    //[Range(0, 1)]
    //public float chanceOfStaying = 0.5f;
    [Header("Object type")]
    public bool isCoughtintEnemy;    
    public bool isAIEnemy;
    public bool isHorWalkingEnemy;
    public bool isStandingEnemy;
    public bool isBonus;    

    public static float chanceOfStayingCE = 0.35f;
    public static float chanceOfStayingAI = 0.2f;
    public static float chanceOfStayingHor = 0.25f;
    public static float chanceOfStayingSt = 0.45f;
    public static float chanceOfStayingBonus = 0.8f;    

    public static float difficulty;
    private void Start()
    {
        float chanceOfStaying = 0.5f;

        if (!isBonus)
        {
            if (isCoughtintEnemy)
                chanceOfStaying = chanceOfStayingCE;
            else if (isAIEnemy)
                chanceOfStaying = chanceOfStayingAI;
            else if (isHorWalkingEnemy)
                chanceOfStaying = chanceOfStayingHor;
            else if (isStandingEnemy)
                chanceOfStaying = chanceOfStayingSt;

            chanceOfStaying *= difficulty;
            if (chanceOfStaying > 1)
                chanceOfStaying = 1;
        }
        else
        {
            chanceOfStaying = chanceOfStayingBonus;
            chanceOfStaying -= (chanceOfStaying * difficulty) - chanceOfStaying;
            if (chanceOfStaying <= 0)
                chanceOfStaying = 0.18f;
        }        
        if (Random.value > chanceOfStaying) Destroy(gameObject);
    }
}
