using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoading : MonoBehaviour
{
    public static bool loadingFromLevel=false;
    public static int SceneID = 1;
    public Text progressText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AsyncLoad());
    }

    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneID);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            progressText.text = string.Format("{0:0}%", progress * 100);
            yield return null;
        }

    }
}
