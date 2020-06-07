using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResourcesControl : MonoBehaviour
{
    [Header("Ссылка на очки защиту:")]
    public Text shieldText;
    [Header("Ссылка на очки здоровья:")]
    public Image liveStrip;
    public Text liveStripText;
    [Header("Ссылка на очки вируса:")]
    public Image infectionStrip;
    public Text infectionStripText;
    [Header("Ссылка на очки валюты:")]
    public Text tPaperText;
    public int countCurrencyPoints = 0;
    public int countPointsOfShield;
    public GameManager gameManager;
    public AudioSource tpaperPickupSound;
    public AudioSource healPickupSound;
    public AudioSource getDamageSound;
    public AudioSource getInfectionDamageSound;
    [Header("Эффекты")]
    public ParticleSystem getHealEffect;
    public ParticleSystem getInfectionEffect;
    public ParticleSystem getDamageEffect;
    private int soundState;
    private float damage;
    private bool canTakeDamage = false;

   
    private void Start()
    {
        soundState = PlayerPrefs.GetInt("Sounds");
        countCurrencyPoints = 0;
        float tempInfectionPoints = PlayerPrefs.GetFloat("InfectionPoints");
        infectionStripText.text = "ОВ:" + tempInfectionPoints.ToString() + "/100";
        tempInfectionPoints /= 100;
        infectionStrip.fillAmount = tempInfectionPoints;
        countPointsOfShield = PlayerPrefs.GetInt("CountpointsOfShield");
        shieldText.text = countPointsOfShield.ToString();        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (infectionStrip.fillAmount < 1)
        {
            if (collision.gameObject.CompareTag("VirusArea"))
            {               
                damage = 0.05f;
                damage -= (damage / 100) * countPointsOfShield;
                InvokeRepeating("TakeDamageFromVirus", 0f, 0.5f); //запускаем метод через 0 секунд и повторяем каждые 0.5 секунд          
            }
            if (collision.gameObject.name == "CoughingArea")
            {
                damage = 0.1f;
                damage -= (damage / 100) * countPointsOfShield;
                InvokeRepeating("TakeDamageFromVirus", 0f, 0.5f); //запускаем метод через 0 секунд и повторяем каждые 0.5 секунд
            }
        }
        if (collision.gameObject.CompareTag("Currency"))
        {
            PlaySound(tpaperPickupSound);
            countCurrencyPoints += 1;
            tPaperText.text = countCurrencyPoints.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Heal"))
        {
            PlaySound(healPickupSound);
            getHealEffect.Play();
            CancelInvoke("TakeDamage");
            canTakeDamage = true;
            Healing();
            Destroy(collision.gameObject);
        }
 
    }

    //ентер вызывает метод 
    //ексит завершает метод
    //персонаж может войти в зону и одновременно выйти из другой
    //по другому завершать можно метод попробывать
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("VirusArea") || collision.gameObject.name == "CoughingArea")
        {
            CancelDamage();             
        }
    }

    public void CancelDamage()
    {
        damage = 0f;
        CancelInvoke("TakeDamageFromVirus"); //останавливаем конкретный инвок с методом      
    }

    void Update()
    {
        if (infectionStrip.fillAmount == 1)
        {
            if(liveStrip.fillAmount == 1 || liveStrip.fillAmount == 0.6f)//если меняем в RestoreByAds знаечние полоски хп, то тут тоже указать
            {
                canTakeDamage = true;
            }
            else if (liveStrip.fillAmount == 0) //заболели
            {
                CancelInvoke("TakeDamage");
                gameManager.TurnLoseMenu();                
            }
            if (canTakeDamage)
            {
                InvokeRepeating("TakeDamage", 0f, 1f); //запускаем метод через 0 секунд и повторяем каждые 1 секунд
                canTakeDamage = false;
            }
        }
    }

    void TakeDamageFromVirus()
    {
        PlaySound(getInfectionDamageSound);
        getInfectionEffect.Play();
        infectionStrip.fillAmount += damage;
        infectionStripText.text = "ОВ:" + Math.Round(infectionStrip.fillAmount * 100).ToString() + "/100";
    }
    void TakeDamage()
    {
        PlaySound(getDamageSound);
        getDamageEffect.Play();
        liveStrip.fillAmount -= 0.1f;
        liveStripText.text = "ОЗ:" + Math.Round(liveStrip.fillAmount * 100).ToString() + "/100";
    }
    void Healing()
    {
        infectionStrip.fillAmount -= 0.1f;
        infectionStripText.text = "ОВ:" + Math.Round(infectionStrip.fillAmount * 100).ToString() + "/100";
    }
    public void RestorByAds()
    {
        liveStrip.fillAmount = 0.6f;
        liveStripText.text = "ОЗ:" + Math.Round(liveStrip.fillAmount * 100).ToString() + "/100";
        infectionStrip.fillAmount = 0f;
        infectionStripText.text = "ОВ:" + Math.Round(infectionStrip.fillAmount * 100).ToString() + "/100";
    }

    void PlaySound(AudioSource sound)
    {
        if (!sound.isPlaying)
        {
            if (soundState == 0)
                sound.Play();
        }
    }
}

