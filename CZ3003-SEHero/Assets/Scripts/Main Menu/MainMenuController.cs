using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System;
using Facebook.Unity;

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

    [SerializeField]
    private Button[] levelButtons;

    public InputField codeText;

    private int world = 1, numOfWorlds = 5;

    private string[] worldDesc = {"Requirement Elicitation", "Use Case Diagram/Description", "UML Design Model (1)", "UML Design Model (2)", "SRS"};
    
    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(FBInitCallback);
        }
        else
        {
            FB.ActivateApp();
        }
    }

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

        if(newState == 3)//level select
        {
            PlayerPrefs.SetInt("worldid", world);

            //TODO: HY, help me set these two variables
            //maybe set playerprefs in Start() then get playerprefs here
            //so don't have to wait here for database call
            int highestworld = 1;
            int highestlevel = 2;
            //if user picks highest world unlocked
            if (world == highestworld)
            {
                for (int i = 0; i < levelButtons.Length; i++)
                {
                    //lock levels that are higher than highest level and unlock those lower or equal
                    if (i > highestlevel)
                        levelButtons[i].interactable = false;
                    else
                        levelButtons[i].interactable = true;
                }
            }
            //else if user has completed previous world
            else if (world == highestworld + 1 && highestlevel == levelButtons.Length - 1)
            {
                for (int i = 0; i < levelButtons.Length; i++)
                {
                    //unlock first level
                    if (i == 0)
                        levelButtons[i].interactable = true;
                    else
                        levelButtons[i].interactable = false;
                }
            }
            else if (world > highestworld)
            {
                //lock all levels
                for (int i = 0; i < levelButtons.Length; i++)
                {
                    levelButtons[i].interactable = false;
                }
            }
            else if (world < highestworld)
            {
                //unlock all levels
                for (int i = 0; i < levelButtons.Length; i++)
                {
                    levelButtons[i].interactable = true;
                }
            }
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
        else
        {
            MessagePanel.GetInstance().ShowMessage("Challenge code must not be empty.");
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
        else
        {
            MessagePanel.GetInstance().ShowMessage("Invalid challenge code.");
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

    private void FBInitCallback() 
    {
        if (FB.IsInitialized)
        {
            FB.GetAppLink(DeepLinkCallback);
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void DeepLinkCallback(IAppLinkResult result)
    {
        Debug.Log("Deeplink called");

        if (!String.IsNullOrEmpty(result.Url))
        {
            var start = result.Url.IndexOf("level/") + 6;
            var end = result.Url.IndexOf("?");

            if (start != -1)
            {
                Debug.Log(result.Url.Substring(start, end - start));

                codeText.text = result.Url.Substring(start, end - start);
                ChangeState(10);
            }
        }
    }
}
