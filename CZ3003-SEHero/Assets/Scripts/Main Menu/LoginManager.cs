using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{

    public InputField emailField;
    public InputField passwordField;
    public MainMenuController mc;
    public Text welcomeBackText;

    public DatabaseManager dbManager;

    public void Login()
    {
        StartCoroutine(dbManager.SendLogin(emailField.text, passwordField.text, LoginCallback));
    }

    private void LoginCallback(bool success, string username) {
        if(success) {
            GlobalVariables.username = username;
            mc.ChangeState(1);
        }
    }
}
