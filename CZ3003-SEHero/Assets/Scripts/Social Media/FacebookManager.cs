using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using System;

public class FacebookManager : MonoBehaviour
{
    public Text userIDText;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void Login()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, OnLogin);
    }

    private void OnLogin(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var token = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(token.UserId);
            userIDText.text = token.UserId;
            //FB.ShareLink(new Uri("http://3.1.70.5/sharelevel.php?levelid=5"), callback: ShareCallback);
        }
        else
        {
            Debug.Log("Canceled Login");
        }
    }

    public void Share()
    {
        //Login();
        FB.ShareLink(new Uri("http://3.1.70.5/sharelevel.php?levelid=" + PlayerPrefs.GetString("Code", "0")), callback: ShareCallback);

    }

    public void OnShare(IShareResult result)
    {
        if(result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!string.IsNullOrEmpty(result.PostId))
        {
            Debug.Log(result.PostId);
        }
        else
        {
            Debug.Log("Share succeed");
        }
    }

    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!String.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        }
        else
        {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");
        }
    }

    public void ShareOnFacebook()
    {
        string app_id = "713430059179583";
        string facebookshare = "https://www.facebook.com/sharer/sharer.php?u=" + Uri.EscapeUriString("Check this level out on SEHero!\n" + PlayerPrefs.GetString("Code", "error"));
        Application.OpenURL(facebookshare);
    }
}
