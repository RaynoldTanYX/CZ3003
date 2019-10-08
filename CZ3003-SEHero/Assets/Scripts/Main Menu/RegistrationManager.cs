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

    public void Register()
    {
        StartCoroutine(SendRegistration(usernameField.text, passwordField.text, emailField.text));
    }

    IEnumerator ShowMessage(string msg)
    {
        messagePanel.SetActive(true);
        message.text = msg;
        yield return new WaitForSeconds(3f);
        messagePanel.SetActive(false);

        mc.ChangeState(0);
    }

    IEnumerator SendRegistration(string username, string password, string email)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", username));
        formData.Add(new MultipartFormDataSection("password", password));
        formData.Add(new MultipartFormDataSection("email", email));

        UnityWebRequest www = UnityWebRequest.Post("http://3.1.70.5/register.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            bool success = response["success"].AsBool;
            Debug.Log(response);

            if (success)
            {
                StartCoroutine(ShowMessage("Successfully registered!"));
            }
        }
    }
}
