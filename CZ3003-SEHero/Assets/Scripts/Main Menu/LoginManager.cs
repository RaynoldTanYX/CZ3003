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
    public Text profileWelcomeBackText;
    public Text message;

    public DatabaseManager dbManager;
    public static bool isLoggedIn = false;

    public void Start() 
    {
        if (PlayerPrefs.HasKey("username"))
        {
            isLoggedIn = true;
            welcomeBackText.text = "Welcome back, " + PlayerPrefs.GetString("username");
            profileWelcomeBackText.text = PlayerPrefs.GetString("username");
            mc.ChangeState(1);
        }
    }

    public void Login()
    {
        if (emailField.text.Length == 0 || passwordField.text.Length == 0)
            MessagePanel.GetInstance().ShowMessage("All fields must not be empty.");
        else
            StartCoroutine(dbManager.SendLogin(emailField.text, passwordField.text, LoginCallback));
    }

    public void Logout() {
        PlayerPrefs.DeleteKey("username");
        isLoggedIn = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void LoginCallback(bool success, string username, string userType, int avatar) {
        if (success)
        {
            PlayerPrefs.SetString("username", username);
            PlayerPrefs.SetString("user_type", userType);
            PlayerPrefs.SetInt("avatar", avatar);
            mc.ChangeState(1);
            welcomeBackText.text = "Welcome back, " + username;
            profileWelcomeBackText.text = PlayerPrefs.GetString("username");
        }
        else {
            MessagePanel.GetInstance().ShowMessage("Login failed, please check your credentials.");
        }
    }
}
