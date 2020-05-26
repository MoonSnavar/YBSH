using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Ссылка на очки валюты, запасы еды и заражение")]
    public Text currentCountCurrencyPoints;
    public Text currentHungryPointText;
    public Image currentHungryFill;
    public Text currentInfectionPointText;
    public Image currentInfectionFill;
    [Header("Ссылка на остальные менюшки")]
    public GameObject secondMenu;
    public GameObject settingsMenu;
    public GameObject shopMenu;
    public GameObject messageMenu;
    public GameObject textMessageMenu;    
    public GameObject newMail;    
    public static bool isActiveButtonGetCurrency;
    public AudioSource music;
    public Toggle switchMusic;
    public AudioSource click;
    public GameObject sounds;
    public Toggle switchSounds;
    [Header("Спрайт мусора")]
    public GameObject trash;
    [Header("Ссылка на больную версию")]
    public GameObject sick;
    [Header("Меню отсутствия интернета")]
    public GameObject noConnectionMenu;
    [Header("Текст проверки соединения")]
    public Text textTestConnection;
    [Header("Рекламная часть")]
    public GameObject buttonADSFirst;
    public GameObject buttonAdsSecond;
    public GameObject imageNoConnection;
    public GameObject buttonRefresh;
    public Text textStatus;
    public static bool isEndAds = false;

    void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            music.Stop();
            switchMusic.isOn = false;
        }
        if (PlayerPrefs.GetInt("Sounds") == 1)
        {
            sounds.SetActive(false);
            switchSounds.isOn = false;
        }

        CheckStateNewMails();
        SetCurrencyPoints();
        if (PlayerPrefs.GetInt("isFirstTime") == 0)
        {            
            currentHungryPointText.text = "ЗАПАСЬ ЕДЬ:40/100";
            currentInfectionPointText.text = "ЗАРАЖЕНИЕ:0/100";
            currentHungryFill.fillAmount = 0.4f;
            currentInfectionFill.fillAmount = 0f;
            PlayerPrefs.SetInt("HungryPoints",40);
            PlayerPrefs.SetFloat("InfectionPoints", 0);
            PlayerPrefs.SetInt("isFirstTime", 1);
            isActiveButtonGetCurrency = true;
        }
        else
        {                        
            if (!SceneLoading.loadingFromLevel)
                CalculatingResources();
            SetHungrypoint();
            SetInfectionPoint();
        }                 
        InvokeRepeating("DecreaseFoodAndHealing", 60f, 60f);        
        CheckSick();
        CheckFood();
    }

    private void CheckSick()
    {
        float tempInfectionPoints = PlayerPrefs.GetFloat("InfectionPoints");
        if (tempInfectionPoints == 100)
        {
            sick.SetActive(true);
        }
    }

    private void CheckFood()
    {
        float temp_hp = PlayerPrefs.GetInt("HungryPoints");
        if (temp_hp <= 20)
            trash.SetActive(true);
        else
            trash.SetActive(false);
    }

    public void Update()
    {
        SetCurrencyPoints();       
        if (isEndAds)
        {
            EndAds();
            isEndAds = false;
        }
    }
    public void SetCurrencyPoints()
    {
        currentCountCurrencyPoints.text = PlayerPrefs.GetInt("CurrencyPoints").ToString();
    }
    void CalculatingResources()
    {
        //Часть с очками рекламы
        DateTime lastSaveAdsCurrency = Utils.GetDateTime("AdsCurrencyDate", DateTime.MinValue);
        TimeSpan timePassed = DateTime.UtcNow - lastSaveAdsCurrency; //сколько времени прошло
        int hourPassed = (int)timePassed.TotalHours;
        hourPassed = Mathf.Clamp(hourPassed, 0, 7 * 24);

        if (hourPassed > 2) //каждые 2 часа реклама доступна
            isActiveButtonGetCurrency = true;
        else
            isActiveButtonGetCurrency = false;

        //Шапка для следующих очков
        DateTime lastSaveEnterDate = Utils.GetDateTime("LastEnterDate", DateTime.MinValue);
        timePassed = DateTime.UtcNow - lastSaveEnterDate; //сколько времени прошло
        int minutePassed = (int)timePassed.TotalMinutes;
        minutePassed = Mathf.Clamp(minutePassed, 0, 7 * 24 * 60);

        //Часть с очками еды
        int hungryPoints = minutePassed * 3;  //в минуту 3 единицы вычитаются
        int temp_hp = PlayerPrefs.GetInt("HungryPoints");
        if (temp_hp - hungryPoints <= 0)
        {
            PlayerPrefs.SetInt("HungryPoints", 0);
        }
        else
        {
            PlayerPrefs.SetInt("HungryPoints", temp_hp - hungryPoints);
        }

        //Часть с очками заражения
        int infectionPoints = minutePassed;  //в минуту 1 единица вычитается
        float temp_ip = PlayerPrefs.GetFloat("InfectionPoints");
        if (temp_ip - infectionPoints <= 0)
        {
            PlayerPrefs.SetFloat("InfectionPoints", 0);
        }
        else
        {
            PlayerPrefs.SetFloat("InfectionPoints", temp_ip - infectionPoints);
        }
    }
    void SaveData()
    {
        Utils.SetDateTime("LastEnterDate", DateTime.UtcNow);
        PlayerPrefs.Save();
    }
    private void OnApplicationQuit()
    {
        SaveData();        
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            SaveData();
    }
    void SetHungrypoint()
    {
        float temp_hp = PlayerPrefs.GetInt("HungryPoints");
        currentHungryPointText.text = "ЗАПАСЬ ЕДЬ:" + temp_hp.ToString() + "/100";
        temp_hp /= 100;
        currentHungryFill.fillAmount = temp_hp;
        CheckFood();
    }
    void SetInfectionPoint()
    {
        float temp_ip = PlayerPrefs.GetFloat("InfectionPoints");
        currentInfectionPointText.text = "ЗАРАЖЕНИЕ:" + temp_ip.ToString() + "/100";
        temp_ip /= 100;
        currentInfectionFill.fillAmount = temp_ip;
    }

    void DecreaseFoodAndHealing()
    { 
        //Уменьшение еды
        int temp_hp = PlayerPrefs.GetInt("HungryPoints");
        if (temp_hp > 0)
        {
            PlayerPrefs.SetInt("HungryPoints", temp_hp - 3);
            SetHungrypoint();
        }
        //Уменьшения заражение
        float temp_ip = PlayerPrefs.GetFloat("InfectionPoints");
        if (temp_ip > 0)
        {
            PlayerPrefs.SetFloat("InfectionPoints", temp_ip - 1);
            SetInfectionPoint();
        }
    }
    public void StartGame()
    {
        PlayClick();
        if (PlayerPrefs.GetFloat("InfectionPoints") < 100 && PlayerPrefs.GetInt("HungryPoints") < 100)
        {
            SceneLoading.SceneID = 2;
            SceneManager.LoadScene(0);
        }
    }
    public void ClearPoint()
    {
        PlayerPrefs.SetInt("Progress", 11);
        //PlayClick();
        //PlayerPrefs.DeleteAll();
        //currentCountCurrencyPoints.text = PlayerPrefs.GetInt("CurrencyPoints").ToString();
    }

    public void TurnSecondMenu(bool open)
    {
        PlayClick();
        secondMenu.SetActive(open);        
    }

    public void TurnSettingsMenu(bool open)
    {
        PlayClick();
        settingsMenu.SetActive(open);
    }
    public void TurnShopMenu(bool open)
    {
        PlayClick();
        shopMenu.SetActive(open);
    }
    public void TurnMessageMenu(bool open)
    {
        PlayClick();
        messageMenu.SetActive(open);
    }
    public void TurnTextMessageMenu(bool open)
    {
        PlayClick();
        CheckStateNewMails();
        textMessageMenu.SetActive(open);
    }
    public void CheckStateNewMails()
    {       
        bool isFind = false;
        int progress = PlayerPrefs.GetInt("Progress");

        if (progress <= PlayerPrefs.GetInt("Length")) //чтоб пустые блоки не создавал
        {
            for (int i = 0; i <= progress; i++) //progress - 1 было
            {
                if (PlayerPrefs.GetInt("MessageNumber" + i.ToString()) == 0)
                {
                    newMail.SetActive(true);
                    isFind = true;
                    break;
                }

            }
        }
        if (!isFind)
            newMail.SetActive(false);         
    }

    private void EndAds()
    {
        noConnectionMenu.SetActive(false);
        isActiveButtonGetCurrency = false;        
        Utils.SetDateTime("AdsCurrencyDate", DateTime.UtcNow);
        buttonADSFirst.SetActive(false);
    }
    public void GetCurrency()
    {
        PlayClick();
        if (!noConnectionMenu.activeSelf)
            noConnectionMenu.SetActive(true);  //включаем меню проверки соединения

        buttonAdsSecond.SetActive(false);
        textStatus.text = "Соединение с интернетом \n не установлено";

        textTestConnection.text = "Проверка соединения...";
        print("Проверка...");
        StartCoroutine(CheckInternetConnection((isConnected) => {
            if (isConnected)
            {
                //connected    
                imageNoConnection.SetActive(false);
                buttonRefresh.SetActive(false);
                buttonAdsSecond.SetActive(true);
                GameADS.once = true;
                textStatus.text = "";
                print("connected");
            }
            else
            {
                //not connected
                imageNoConnection.SetActive(true);
                buttonRefresh.SetActive(true);
                print("not connected");
            }
            textTestConnection.text = "";
        }));
        
    }

    public void OffNoConnectionMenu()
    {
        PlayClick();
        noConnectionMenu.SetActive(false);
    }
    public void SwitchMusic()
    {
        PlayClick();
        if (switchMusic.isOn)
        {
            music.Play();
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            music.Stop();
            PlayerPrefs.SetInt("Music", 1);
        }       
    }
    public void SwitchSounds()
    {       
        if (switchSounds.isOn)
        {
            sounds.SetActive(true);
            PlayClick();
            PlayerPrefs.SetInt("Sounds", 0);            
        }
        else
        {
            sounds.SetActive(false);
            PlayerPrefs.SetInt("Sounds", 1);           
        }
    }
    void PlayClick()
    {
        if (sounds.activeSelf)
            click.Play();
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
