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
        if(emailField.text.IndexOf('@') <= 0)
            StartCoroutine(ShowMessage("Invalid email format entered!", false));
        else if(emailField.text.Length == 0 || usernameField.text.Length == 0 || passwordField.text.Length == 0)
            StartCoroutine(ShowMessage("All fields must be filled up!", false));
        else
            StartCoroutine(dbManager.SendRegistration(usernameField.text, passwordField.text, emailField.text, RegisterCallback));
    }

    IEnumerator ShowMessage(string msg, bool success)
    {
        messagePanel.SetActive(true);
        message.text = msg;
        yield return new WaitForSeconds(3f);
        messagePanel.SetActive(false);

        if(success)
            mc.ChangeState(0);
    }

    private void RegisterCallback(bool success, string msg) {
        if(success)
            StartCoroutine(ShowMessage("Successfully registered!", true));
        else
            StartCoroutine(ShowMessage(msg, false));
    }
}
