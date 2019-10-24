using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorController : MonoBehaviour
{

    private enum EditorState
    {
        GameSelect,//0
        Editor,
        End
    }

    private EditorState m_state;

    [SerializeField]
    private List<GameObject> m_menuObjects;

    private int m_gameType;

    private void Start()
    {
        m_state = EditorState.GameSelect;
        m_gameType = 0;        
    }

    public void ChangeState(int newState)
    {
        m_menuObjects[(int)m_state].SetActive(false);

        m_state = (EditorState)newState;

        m_menuObjects[(int)m_state].SetActive(true);
    }

    public void setState(int state)
    {
        m_state = (EditorState)state;
    }
}
