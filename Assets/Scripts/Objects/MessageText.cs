using UnityEngine;
using UnityEngine.UI;

public class MessageText : MonoBehaviour
{
    public int number;
    public Text title;
    public Text from;
    public string text;    
    public GameObject red;
    public GameObject green;
    private MenuManager menuManager;

    private void OnEnable()
    {
        CheckState();
    }
    void CheckState()
    {
        if (PlayerPrefs.GetInt("MessageNumber" + number.ToString()) == 0)
        {
            red.SetActive(true);
            green.SetActive(false);
        }
        else
        {
            red.SetActive(false);
            green.SetActive(true);
        }
    }
    public void TurnTextMessageMenu()
    {
        if (PlayerPrefs.GetInt("MessageNumber" + number.ToString()) == 0)
            PlayerPrefs.SetInt("MessageNumber" + number.ToString(), 1);

        menuManager = GameObject.Find("GameManager").GetComponent<MenuManager>();
        MessageBlock.text = text;
        menuManager.TurnTextMessageMenu(true);
        CheckState();
    }
}
