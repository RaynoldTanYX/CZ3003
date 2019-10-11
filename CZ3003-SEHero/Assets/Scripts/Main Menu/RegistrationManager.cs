using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class RegistrationManager : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public InputField emailField;
    public GameObject messagePanel;
    public Text message;
    public MainMenuController mc;

    public DatabaseManager dbManager;

    public void Register()
    {
        StartCoroutine(dbManager.SendRegistration(usernameField.text, passwordField.text, emailField.text, RegisterCallback));

    
    }

    IEnumerator ShowMessage(string msg)
    {
        messagePanel.SetActive(true);
        message.text = msg;
        yield return new WaitForSeconds(3f);
        messagePanel.SetActive(false);

        mc.ChangeState(0);
    }

    private void RegisterCallback(bool success) {
        if(success)
            StartCoroutine(ShowMessage("Successfully registered!"));
    }
}
