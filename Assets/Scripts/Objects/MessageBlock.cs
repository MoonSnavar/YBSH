using UnityEngine;

public class MessageBlock : MonoBehaviour
{
    public TextAsset ta;
    public Message message;
    public GameObject messageBlock;
    public GameObject parentContent;
    private GameObject lastObject;
    private MessageText messageText;

    public static string text;

    void Start()
    {              
        message = Message.Load(ta);
        SpawnMessageBlocks();
    }

    void SpawnMessageBlocks()
    {
        int progress = PlayerPrefs.GetInt("Progress");
        if (progress == 0)
        {
            lastObject = Instantiate(messageBlock, parentContent.transform);                
            SetText(0);
        }
        else
        {
            for (int i = 0; i <= progress; i++)
            {
                if (i <= message.nodes.Length - 1)
                {
                    lastObject = Instantiate(messageBlock, parentContent.transform);
                    SetText(i);
                }
            }
        }
        PlayerPrefs.SetInt("Length", message.nodes.Length);
    }
    void SetText(int i)
    {
        messageText = lastObject.GetComponent<MessageText>();
        messageText.title.text = message.nodes[i].titleText;
        messageText.text = message.nodes[i].text;
        messageText.number = i;
        messageText.from.text = message.nodes[i].from;
    }
}
