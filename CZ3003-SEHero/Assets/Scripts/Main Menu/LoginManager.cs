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

    public void Login()
    {
        StartCoroutine(SendLogin(emailField.text, passwordField.text));
    }

    public void Register()
    {
        SceneManager.LoadScene("RegisterScene");
    }

    IEnumerator SendLogin(string email, string password)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("email", email));
        formData.Add(new MultipartFormDataSection("password", password));

        Debug.Log("email " + email);
        Debug.Log("password " + password);

        UnityWebRequest www = UnityWebRequest.Post("http://3.1.70.5/login.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            bool success = response["success"].AsBool;

            if (success) {
                GlobalVariables.username = response["username"];
                mc.ChangeState(1);
            }

        }
    }
}
