using System;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public ChunkPlacer chunkPlacer;
    public static bool isGetProducts;
    public PlayableDirector director;
    public PlayableAsset timelineShop;
    private bool canUdate;
    private void Start()
    {
        canUdate = false;
        isGetProducts = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "CheckTimeline")
        {
            if (isGetProducts && canUdate)
            {
                canUdate = false;            
                chunkPlacer.ClearLevel();
                chunkPlacer.SpawnChunk();
            }
        }
        if (collision.name == "GameManager")
        {
            if (!isGetProducts)
            {
                director.Play(timelineShop);
                canUdate = true;
                isGetProducts = true;                
            }
        }
        else if(collision.name == "Exit")
        {
            if (isGetProducts)
            {                                
                int temp = PlayerPrefs.GetInt("HungryPoints");
                PlayerPrefs.SetInt("HungryPoints", temp + 20);
                temp = PlayerPrefs.GetInt("Progress");

                if (temp <= PlayerPrefs.GetInt("Length")) //чтоб пустые блоки не создавал
                    PlayerPrefs.SetInt("Progress", temp + 1);

                temp = PlayerPrefs.GetInt("LevelProgress");
                PlayerPrefs.SetInt("LevelProgress", temp + 1);

                Saveinfection();
                int currentCountCurrencyPoints = PlayerPrefs.GetInt("CurrencyPoints");
                PlayerPrefs.SetInt("CurrencyPoints", currentCountCurrencyPoints + GetComponent<ResourcesControl>().countCurrencyPoints);
                SceneLoading.loadingFromLevel = true;
                SceneLoading.SceneID = 1;
                SceneManager.LoadScene(0);
            }
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            Saveinfection();
    }
    private void OnApplicationQuit()
    {
        Saveinfection();
    }
    void Saveinfection()
    {
        double tempInfectionPoints = Math.Round(GetComponent<ResourcesControl>().infectionStrip.fillAmount * 100);
        PlayerPrefs.SetFloat("InfectionPoints", (float)tempInfectionPoints);
    }
}
