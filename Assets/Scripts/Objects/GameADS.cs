using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine;

public class GameADS : MonoBehaviour, IUnityAdsListener
{
    public Text textTestConnection;
    public static bool once;
    public bool isInfection = false;
    public Button myButton; // кнопка, которая будет показывать ролик
    public string myPlacementId = "rewardedVideo"; // идентификатор видео, по умолчанию 'rewardedVideo'
    private string gameId = "3547178"; // идентификатор приложения
   
    
    void Start()
    {
        myButton.interactable = Advertisement.IsReady(myPlacementId);
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, false);
        if (!myButton.interactable)
            textTestConnection.text = "Загрузка...";
    }

    void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementId);
    }

    public void OnUnityAdsReady(string placementId)
    {     
        if (placementId == myPlacementId)
        {
            myButton.interactable = true;
            textTestConnection.text = "";
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // ошибка
        print("Не удалось подключиться к сервису");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // дополнительные действия, которые необходимо предпринять, когда конечные пользователи запускают объявление.       
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            if (isInfection)
            {
                GameManager.isADS = true;
            }
            else
            {
                if (once)
                {
                    int currentPoints = PlayerPrefs.GetInt("CurrencyPoints");
                    PlayerPrefs.SetInt("CurrencyPoints", currentPoints + 50);                    
                    once = false;
                    ItemBlock.canUpdate = true;
                    MenuManager.isEndAds = true;
                }
            }           
        }
        else if (showResult == ShowResult.Skipped)
        {
            // не вознаграждайте пользователя за пропуск объявления.
        }
        else if (showResult == ShowResult.Failed)
        {
            // объявление не было завершено из-за ошибки.
        }
    }

}
