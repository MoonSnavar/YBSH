using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController playerController;
    [Header("Ссылка на подменю")]
    public GameObject submenu;
    [Header("Ссылка на джойстик")]
    public GameObject joystick;
    [Header("Ссылка на контроль ресурсов")]
    public ResourcesControl resourcesControl;
    [Header("Ссылка на менюшку после проигрыша")]
    public GameObject loseMenu;
    [Header("Ссылка на звуки")]
    public AudioSource music;
    public GameObject sounds;
    public AudioSource click;
    public static bool isADS;
    [Header("Иконка и кнопка обновить")]
    public GameObject iconNoConnection;
    public GameObject buttonRefresh;
    [Header("Кнопка рекламы")]
    public GameObject buttonADS;
    [Header("Текст проверки")]
    public Text textTestConnection;
    [Header("Экран паузы перед началом")]
    public GameObject continueScreen;
    public static bool canCheckConnection;
    private void Start()
    {
        Time.timeScale = 1f;
        canCheckConnection = true;
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            music.Stop();            
        }
        if (PlayerPrefs.GetInt("Sounds") == 1)
        {
            sounds.SetActive(false);           
        }        
        isADS = false;
    }
    public void BackToMenu()
    {
        PlayClick();

        double tempInfectionPoints = Math.Round(resourcesControl.infectionStrip.fillAmount * 100);
        PlayerPrefs.SetFloat("InfectionPoints", (float)tempInfectionPoints);

        SceneLoading.loadingFromLevel = true;
        SceneLoading.SceneID = 1;
        SceneManager.LoadScene(0);
    }
    public void TurnSubmenu(bool open)
    {
        PlayClick();
        submenu.SetActive(open);
        joystick.SetActive(!open);
        if (open)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
 
    public void TurnLoseMenu()
    {
        playerController.enabled = false;        
        loseMenu.SetActive(true);
        joystick.SetActive(false);
        Time.timeScale = 0f;    
        if (canCheckConnection)
        {
            CheckConnection();
            canCheckConnection = false;
        }

    }
    private void Update()
    {
        if (isADS)
        {
            BackToGame();
            isADS = false;
        }
    }
    public void BackToGame()
    {
        //обнулить заражение и восстановить здоровье
        buttonADS.SetActive(false);
        resourcesControl.RestorByAds();
        loseMenu.SetActive(false);
        continueScreen.SetActive(true);        
        Utils.SetDateTime("LastSaveAds", DateTime.UtcNow);
    }
    public void Continue()
    {
        playerController.enabled = true;
        continueScreen.SetActive(false);
        joystick.SetActive(true);
        Time.timeScale = 1f;
    }

    void PlayClick()
    {
        if (sounds.activeSelf)
            click.Play();
    }


    public void CheckConnection() 
    {        
        PlayClick();
        print("Проверка...");
        textTestConnection.text = "Проверка соединения...";
        StartCoroutine(CheckInternetConnection((isConnected) => {
            if (isConnected)
            {
                //connected
                iconNoConnection.SetActive(false);
                buttonRefresh.SetActive(false);
                buttonADS.SetActive(true);                
                print("connected");
            }
            else
            {
                //not connected
                iconNoConnection.SetActive(true);
                buttonRefresh.SetActive(true);
                buttonADS.SetActive(false);                
                print("not connected");
            }
            textTestConnection.text = "";
        }));
    }

    private IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://google.com");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }
}
