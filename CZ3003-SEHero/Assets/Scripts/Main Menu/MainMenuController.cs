using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void GetLevel(int level)
    {
        //TODO: add world selection
        int world = 1;

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
