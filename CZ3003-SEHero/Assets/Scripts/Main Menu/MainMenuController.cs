using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

	private MenuState m_state;

    [SerializeField]
    private List<GameObject> m_menuObjects;

    [SerializeField]
    private Text worldTitleText;

    [SerializeField]
    private Text worldNumberText;

    private int world = 1, numOfWorlds = 5;

    private string[] worldDesc = {"Requirement Elicitation", "World 2 Desc", "World 3 Desc", "World 4 Desc", "World 5 Desc"};

	void Start()
	{
		m_state = MenuState.Login;
	}

	public void ChangeState(int newState)
	{
        m_menuObjects[(int)m_state].SetActive(false);

        m_state = (MenuState)newState;

        m_menuObjects[(int)m_state].SetActive(true);
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
}
