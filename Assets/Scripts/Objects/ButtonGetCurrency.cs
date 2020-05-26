using UnityEngine;

public class ButtonGetCurrency : MonoBehaviour
{
    private void OnEnable()
    {
        if (!MenuManager.isActiveButtonGetCurrency)
            gameObject.SetActive(false);
    }
}
