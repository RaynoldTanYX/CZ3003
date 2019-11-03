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
    public Text message;
    public GameObject messagePanel;

    public DatabaseManager dbManager;
    public static bool isLoggedIn = false;

    public void Start() 
    {
        if (PlayerPrefs.HasKey("username"))
        {
            isLoggedIn = true;
            welcomeBackText.text = "Welcome back, " + PlayerPrefs.GetString("username");
            mc.ChangeState(1);
        }
    }

    public void Login()
    {
        if (emailField.text.Length == 0 || passwordField.text.Length == 0)
            StartCoroutine(ShowMessage("All fields must be filled up!"));
        else
            StartCoroutine(dbManager.SendLogin(emailField.text, passwordField.text, LoginCallback));
    }

    public void Logout() {
        PlayerPrefs.DeleteKey("username");
        isLoggedIn = false;
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator ShowMessage(string msg)
    {
        messagePanel.SetActive(true);
        message.text = msg;
        yield return new WaitForSeconds(3f);
        messagePanel.SetActive(false);
    }

    private void LoginCallback(bool success, string username) {
        if (success)
        {
            PlayerPrefs.SetString("username", username);
            mc.ChangeState(1);
            welcomeBackText.text = "Welcome back, " + username;
        }
        else {
            StartCoroutine(ShowMessage("Login failed, please check your credentials."));
        }
    }
}
