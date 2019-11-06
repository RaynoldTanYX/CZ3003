using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    static MessagePanel instance;
    [SerializeField]
    Text text;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        if (text == null)
            Debug.Log("MessagePanel.text is not set.");
        instance.HideMessage();
    }


    public static MessagePanel GetInstance()
    {
        return instance;
    }

    public void ShowMessage(string message)
    {
        instance.text.text = message;
        instance.gameObject.SetActive(true);
    }

    public void HideMessage()
    {
        instance.text.text = "";
        instance.gameObject.SetActive(false);
    }
}
