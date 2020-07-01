using System;
using UnityEngine;
using UnityEngine.UI;

public class CheckLosemenu : MonoBehaviour
{
    public GameObject buttonADS;
    public Text textADS;
    private void OnEnable()
    {
        DateTime lastsaveAds = Utils.GetDateTime("LastSaveAds", DateTime.MinValue);
        TimeSpan timePassed = DateTime.UtcNow - lastsaveAds; //сколько времени прошло
        int minutePassed = (int)timePassed.TotalMinutes;
        minutePassed = Mathf.Clamp(minutePassed, 0, 7 * 24 * 60 * 60);
        print(minutePassed);
        if (minutePassed >= 20)
        {
            GameManager.canCheckConnection = true;
            textADS.text = "Можно вернуться в игру, \n посмотрев рекламу";            
        }
        else
        {
            GameManager.canCheckConnection = false;
            textADS.text = "Ваша попытка выздоровления была истрачена";
        }
    }
}
