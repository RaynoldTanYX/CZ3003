using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class DatabaseManager : MonoBehaviour
{
    public static string URL = "http://3.1.70.5/";

    public IEnumerator SendLogin(string email, string password, Action<bool, string, string, int> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("email", email));
        formData.Add(new MultipartFormDataSection("password", password));

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

            if (callback != null)
                callback(success, response["username"], response["user_type"], response["avatar"]);
        }
    }

    public IEnumerator SendRegistration(string username, string password, string email, int avatarChoice, Action<bool, string> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", username));
        formData.Add(new MultipartFormDataSection("password", password));
        formData.Add(new MultipartFormDataSection("email", email));
        formData.Add(new MultipartFormDataSection("avatar", avatarChoice.ToString()));

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

            if (callback != null)
                callback(success, response["message"]);
        }
    }

    public IEnumerator CheckUsername(string username, Action<bool> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", username));

        UnityWebRequest www = UnityWebRequest.Post(URL + "checkusername.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            bool success = response["success"].AsBool;

            Debug.Log(response["message"]);

            if (callback != null)
                callback(success);
        }
    }

    public IEnumerator SaveLevel(string name, string worldId, string data, Action<int> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("name", name));
        formData.Add(new MultipartFormDataSection("world_id", worldId));
        formData.Add(new MultipartFormDataSection("data", data));

        UnityWebRequest www = UnityWebRequest.Post(URL + "savelevel.php", formData);
        Debug.Log("Sending level data");
        yield return www.SendWebRequest();
        Debug.Log("Sent!");

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            Debug.Log(response);
            //bool success = response["success"].AsBool;
            //if (success)
            {
                if (callback != null)
                {
                    callback(response["level_id"]);
                }
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

            if (callback != null)
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

            if (callback != null)
                callback(success, response["message"]);
        }
    }

    public IEnumerator GetLeaderboard(int world, Action<bool, JSONNode> callback = null)
    {
        Debug.Log("World id: " + world);
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("worldid", world.ToString()));

        UnityWebRequest www = UnityWebRequest.Post(URL + "leaderboard.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            bool success = response["success"].AsBool;

            if (callback != null)
                callback(success, response["scores"]);
        }
    }

    public IEnumerator GetCurrentProgress(string username, Action<bool, int, int> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", username));

        UnityWebRequest www = UnityWebRequest.Post(URL + "getcurrentprogress.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            bool success = response["success"].AsBool;

            if (callback != null)
                callback(success, response["world_id"], response["level_id"]);
        }
    }

    public IEnumerator GetTotalScore(string username, Action<bool, int> callback = null)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", username));

        UnityWebRequest www = UnityWebRequest.Post(URL + "gettotalscore.php", formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var response = JSON.Parse(www.downloadHandler.text);
            bool success = response["success"].AsBool;

            if (callback != null)
                callback(success, response["totalscore"]);
        }
    }
}
