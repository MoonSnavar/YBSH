using System.IO;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("dialogue")]
public class Message
{
    [XmlElement("node")]
    public Node[] nodes;
    public static Message Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Message));
        StringReader reader = new StringReader(_xml.text);
        Message dial = serializer.Deserialize(reader) as Message;
        return dial;
    }
}

[System.Serializable]
public class Node
{
    [XmlElement("title")]
    public string titleText;

    [XmlElement("text")]
    public string text;

    [XmlElement("from")]
    public string from;
}
