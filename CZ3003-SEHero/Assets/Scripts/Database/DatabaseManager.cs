using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class DatabaseManager : MonoBehaviour
{
    private const string URL = "http://3.1.70.5/";

    public IEnumerator SendLogin(string email, string password, Action<bool, string> callback = null)
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

            if(callback != null)
                callback(success, response["username"]);
        }
    }

    public IEnumerator SendRegistration(string username, string password, string email, Action<bool> callback = null)
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

            if(callback != null)
                callback(success);
        }
    }
    
    public IEnumerator SaveLevel(string name, string worldId, string data, Action<bool> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("name", name));
        formData.Add(new MultipartFormDataSection("world_id", worldId));
        formData.Add(new MultipartFormDataSection("data", data));

        UnityWebRequest www = UnityWebRequest.Post(URL + "savelevel.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            bool success = response["success"].AsBool;

            if (success)
            {
                if (callback != null)
                    callback(success);
            }
        }
    }

    public IEnumerator GetLevel(int world, int level, Action<bool, string, string> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("world", world.ToString()));
        formData.Add(new MultipartFormDataSection("level", level.ToString()));

        UnityWebRequest www = UnityWebRequest.Post(URL + "getlevel.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            bool success = response["success"].AsBool;

            Debug.Log(response["data"]);

            if(callback != null)
                callback(success, response["name"], response["data"]);
        }
    }

    public IEnumerator SaveScore(int world, int level, string username, int score, Action<bool, string> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("worldid", world.ToString()));
        formData.Add(new MultipartFormDataSection("levelid", level.ToString()));
        formData.Add(new MultipartFormDataSection("username", username));
        formData.Add(new MultipartFormDataSection("score", score.ToString()));

        UnityWebRequest www = UnityWebRequest.Post(URL + "savescore.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            bool success = response["success"].AsBool;

            if(callback != null)
                callback(success, response["message"]);
        }
    }
}
