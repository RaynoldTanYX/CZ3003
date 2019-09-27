using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainMenuController : MonoBehaviour
{

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
}
