using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitterManager : MonoBehaviour
{
    string twitter_address = "http://twitter.com/intent/tweet";
    string twitter_language = "en";
    string textToDisplay = "Hey Guys!! New Level Created!! Check it out.";

    public void shareOnTwitter()
    {
        Application.OpenURL(twitter_address = "?text=" + WWW.EscapeURL(textToDisplay) + ";lang=" + WWW.EscapeURL(twitter_language));
    }
}
