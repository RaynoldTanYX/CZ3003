using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System.IO;
using System;
using Facebook.Unity;
using UnityEngine.Networking;

[System.Serializable]
public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    DatabaseManager dbManager;

    public enum MenuState
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

    [SerializeField]
    private Text totalScore;

    [SerializeField]
    private Text currentWorld;

    [SerializeField]
    private Text currentLevel;

    public InputField codeText;

    private int world = 1, numOfWorlds = 5, currentWorldValue = 1, currentLevelValue = 1, totalScoreValue = 0;

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
        if (LoginManager.isLoggedIn)
        {
            StartCoroutine(dbManager.GetCurrentProgress(PlayerPrefs.GetString("username"), GetCurrentProgressCallback));
            StartCoroutine(dbManager.GetTotalScore(PlayerPrefs.GetString("username"), GetTotalScoreCallback));
            m_state = MenuState.Game;
        }
        else
            m_state = MenuState.Login;
		
        if (PlayerPrefs.GetInt("State", 0) == (int)MenuState.Level)
        {
            world = PlayerPrefs.GetInt("worldid", 1) - 1;
            PlayerPrefs.SetInt("State", 0);
            NextWorld();
            ChangeState((int)MenuState.Level);
        }
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

        if (newState == 5) {
            if (totalScoreValue == 0)
            {
                StartCoroutine(dbManager.GetCurrentProgress(PlayerPrefs.GetString("username"), GetCurrentProgressCallback));
                StartCoroutine(dbManager.GetTotalScore(PlayerPrefs.GetString("username"), GetTotalScoreCallback));
            }
            else {
                currentWorld.text = currentWorldValue.ToString();
                currentLevel.text = currentLevelValue.ToString();
                totalScore.text = totalScoreValue.ToString();
            }
        }

        if(newState == 3)//level select
        {
            PlayerPrefs.SetInt("worldid", world);

            int highestworld = PlayerPrefs.GetInt("currentworld", 1);
            int highestlevel = PlayerPrefs.GetInt("currentlevel", 1);
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
        //StartCoroutine(DownloadReport());
        string username = PlayerPrefs.GetString("username");
        string userType = PlayerPrefs.GetString("user_type");
        Application.OpenURL("http://3.1.70.5/pdf.php?username=" + username + "&" + "user_type=" + userType);

    }

    public void Test()
    {
        //Application.OpenURL(Application.persistentDataPath + "/data/report.pdf");

        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject unityContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

            string packageName = unityContext.Call<string>("getPackageName");
            string authority = packageName + ".fileprovider";

            AndroidJavaClass intentObj = new AndroidJavaClass("android.content.Intent");
            string ACTION_VIEW = intentObj.GetStatic<string>("ACTION_VIEW");
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);

            int FLAG_ACTIVITY_NEW_TASK = intentObj.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
            int FLAG_GRANT_READ_URI_PERMISSION = intentObj.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");

            AndroidJavaObject fileObj = new AndroidJavaObject("java.io.File", Application.persistentDataPath + "/data/report.pdf");
            AndroidJavaClass fileProvider = new AndroidJavaClass("android.support.v4.content.FileProvider");
            AndroidJavaObject uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext, authority, fileObj);

            print(uri.Call<string>("toString"));

            intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/pdf");
            intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
            intent.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION);
            currentActivity.Call("startActivity", intent);

            print("Success");
        }
        catch (System.Exception e)
        {
            print("Error: " + e.Message);
        }
    }

    public IEnumerator DownloadReport() {
        string URL = "http://3.1.70.5/";
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", PlayerPrefs.GetString("username")));
        formData.Add(new MultipartFormDataSection("user_type", PlayerPrefs.GetString("user_type")));

        string filePath = Path.Combine(Application.persistentDataPath, "data");
        filePath = Path.Combine(filePath, "report1.pdf");
        UnityWebRequest www = UnityWebRequest.Post(URL + "pdf.php", formData);
        www.downloadHandler = new DownloadHandlerBuffer();
        var dh = new DownloadHandlerFile(filePath);
        dh.removeFileOnAbort = true;
        www.downloadHandler = dh;
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else {

            Debug.Log(filePath);

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            Debug.Log("OpenURL");
            Application.OpenURL("file:///storage/emulated/0/Android/data/com.DefaultCompany.SEHero/files/data/report.pdf");

            //System.IO.File.WriteAllText(filePath, www.downloadHandler.text);
        }
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
        if (!String.IsNullOrEmpty(result.Url))
        {
            var start = result.Url.IndexOf("level/") + 6;
            var end = result.Url.IndexOf("?");

            if ((start - 6) != -1)
            {
                codeText.text = result.Url.Substring(start, end - start);
                ChangeState(10);
            }
        }
    }

    private void GetCurrentProgressCallback(bool success, int worldId, int levelId)
    {
        if (success) {
            PlayerPrefs.SetInt("currentworld", worldId);
            PlayerPrefs.SetInt("currentlevel", levelId);


            currentWorld.text = worldId.ToString();
            currentLevel.text = levelId.ToString();
        }
    }

    private void GetTotalScoreCallback(bool success, int score)
    {
        if (success)
        {
            totalScoreValue = score;
            totalScore.text = score.ToString();
        }
    }
}
