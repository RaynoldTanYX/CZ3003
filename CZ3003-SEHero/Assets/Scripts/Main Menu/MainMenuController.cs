﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System;

[System.Serializable]
public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    DatabaseManager dbManager;

    private enum MenuState
    {
        Login,//0
        Game,
        World,
        Level,
        UserCreations,
        Profile,
        Leaderboard
    }

	private static MenuState m_state;

    [SerializeField]
    private List<GameObject> m_menuObjects;

    [SerializeField]
    private Text worldTitleText;

    [SerializeField]
    private Text worldNumberText;

    public InputField codeText;

    private int world = 1, numOfWorlds = 5;

    private string[] worldDesc = {"Requirement Elicitation", "Use Case Diagram/Description", "UML Design Model (1)", "UML Design Model (2)", "SRS"};

	void Start()
	{
        if(LoginManager.isLoggedIn)
		    m_state = MenuState.Game;
        else
            m_state = MenuState.Login;
    }

	public void ChangeState(int newState)
    {
        m_menuObjects[(int)m_state].SetActive(false);

        m_state = (MenuState)newState;

        m_menuObjects[(int)m_state].SetActive(true);

        if (newState == 7)
        {
            m_state = MenuState.Login;
            SceneManager.LoadScene("Editor");
        }

        if(newState == 3)
            PlayerPrefs.SetInt("worldid", world);
    }

    public void NextWorld() 
    {
        world++;

        if (world > numOfWorlds)
            world = 1;

        worldTitleText.text = worldDesc[world - 1];
        worldNumberText.text = "World # " + world;
    }

    public void PreviousWorld()
    {
        world--;

        if (world < 1)
            world = numOfWorlds;

        worldTitleText.text = worldDesc[world - 1];
        worldNumberText.text = "World # " + world;
    }

    public void GetLevel(int level)
    {
        //TODO: add world selection
        //int world = 1;

        PlayerPrefs.SetInt("worldid", world);
        PlayerPrefs.SetInt("levelid", level);

        StartCoroutine(dbManager.GetLevel(world, level, GetLevelCallback));
    }

    public void GetChallenge()
    {
        int level = -1;
        try
        {
            level = Int32.Parse(codeText.text);
        }
        catch { };
        if (level >= 0)
        {
            StartCoroutine(dbManager.GetLevel(0, level, GetLevelCallback));
        }
    }

    private void GetLevelCallback(bool success, string name, string data) {
        if(success) {
            data = data.Replace("\\", "");
            Debug.Log("GetLevel: " + data);
            PlayerPrefs.SetString("level", data);
            m_state = MenuState.Login;
            SceneManager.LoadScene("GameLoader");
        }
    }

    public void GenerateReport() {
        Application.OpenURL("http://3.1.70.5/pdf.php");
    }

    public void DownloadReport() {
        Debug.Log("DownloadReport called");
        System.Net.WebClient client = new WebClient();
        client.DownloadFileAsync(new Uri("http://3.1.70.5/pdf.php"), Application.persistentDataPath + "report.pdf"); //This shit doesn't work TODO
    }
}
