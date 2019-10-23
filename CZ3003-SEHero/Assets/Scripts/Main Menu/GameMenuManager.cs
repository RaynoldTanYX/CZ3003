using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public Text welcomeBackText;

    void Start()
    {
        welcomeBackText.text = "Welcome back, " + GlobalVariables.username;
    }
}
