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

    private float chanceOfStaying = 0.5f;
    private void Start()
    {
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
            int progress = PlayerPrefs.GetInt("LevelProgress");

            if (progress < 2)
            {
                //не спавнить бумагу и мыло
                chanceOfStaying = 0;
            }
            else if (progress < 3)
            {
                //спавнить бумагу, но не спавнить мыло
                if (gameObject.CompareTag("Heal"))
                    chanceOfStaying = 0;
                else
                    CalculateChanceForBonus();
            }
            else
                CalculateChanceForBonus();
        }        
        if (Random.value > chanceOfStaying) Destroy(gameObject);
    }
    private void CalculateChanceForBonus()
    {
        chanceOfStaying = chanceOfStayingBonus;
        chanceOfStaying -= (chanceOfStaying * difficulty) - chanceOfStaying;
        if (chanceOfStaying <= 0)
            chanceOfStaying = 0.18f;
    }
}
