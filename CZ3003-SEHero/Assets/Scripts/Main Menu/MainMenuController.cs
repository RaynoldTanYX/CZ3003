using System.Collections;
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
        End
    }

	private static MenuState m_state;

    [SerializeField]
    private List<GameObject> m_menuObjects;

    [SerializeField]
    private Text worldTitleText;

    [SerializeField]
    private Text worldNumberText;

    private int world = 1, numOfWorlds = 5;

    private string[] worldDesc = {"Requirement Elicitation", "UseCase Diagram/Description", "UML Design Model (1)", "UML Design Model (2)", "SRS"};

	void Start()
	{
        if(LoginManager.isLoggedIn)
		    m_state = MenuState.Game;
        else
            m_state = MenuState.Login;
    }

	public void ChangeState(int newState)
    {
        Debug.Log(m_state + " set to inactive");
        m_menuObjects[(int)m_state].SetActive(false);

        m_state = (MenuState)newState;

        Debug.Log(m_state + " set to active");

        m_menuObjects[(int)m_state].SetActive(true);

        if (newState == 7)
        {
            SceneManager.LoadScene("Editor");
        }
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

    private void GetLevelCallback(bool success, string name, string data) {
        Debug.Log("called");
        if(success) {
            Debug.Log("success");
            data = data.Replace("\\", "");
            Debug.Log(data);
            PlayerPrefs.SetString("level", data);
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

    public void setMState(int newState) {
        m_state = (MenuState) newState;
    }
}
