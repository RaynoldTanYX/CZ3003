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
            FB.Init();
        }
        else
        {
            FB.ActivateApp();
        }
    }

    public void LogIn()
    {
        FB.LogInWithReadPermissions(callback:OnLogIn);
    }

    private void OnLogIn(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            AccessToken token = AccessToken.CurrentAccessToken;
            userIDText.text = token.UserId;
        }
        else
        {
            Debug.Log("Canceled Login");
        }
    }

    public void Share()
    {
        // Note: Currently, we do not have a link for this. Will update once we have the link.
        FB.ShareLink(
            contentTitle: "SEHero Game Creation", 
            contentURL: null,
            contentDescription: "Check this level out on SEHero!\n" + PlayerPrefs.GetString("Code", "error"),
            callback:OnShare
            );
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

    public void ShareOnFacebook()
    {
        string app_id = "713430059179583";
        string facebookshare = "https://www.facebook.com/sharer/sharer.php?u=" + Uri.EscapeUriString("Check this level out on SEHero!\n" + PlayerPrefs.GetString("Code", "error"));
        Application.OpenURL(facebookshare);
    }
}
