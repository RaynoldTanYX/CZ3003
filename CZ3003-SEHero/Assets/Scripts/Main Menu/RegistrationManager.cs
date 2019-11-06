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

    private int avatarChoice = 0;

    public void Register()
    {
        if (emailField.text.IndexOf('@') <= 0)
            MessagePanel.GetInstance().ShowMessage("Invalid email format entered!");
        else if (emailField.text.Length == 0 || usernameField.text.Length == 0 || passwordField.text.Length == 0)
            MessagePanel.GetInstance().ShowMessage("All fields must be filled up!");
        else
            StartCoroutine(dbManager.SendRegistration(usernameField.text, passwordField.text, emailField.text, avatarChoice, RegisterCallback));
    }

    public void SelectAvatar(int choice) {
        avatarChoice = choice;
    }

    private void RegisterCallback(bool success, string msg) {
        if(success)
            mc.ChangeState(0);
        else
            MessagePanel.GetInstance().ShowMessage(msg);
    }
}
