using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorController : MonoBehaviour
{


    private int m_state;
    private string m_code;

    [SerializeField]
    private List<GameObject> m_menuObjects;
    [SerializeField]
    private Text m_codeText;

    private int m_gameType;

    private void Start()
    {
        m_state = 0;
        m_gameType = 0;        
    }

    public void ChangeState(int newState)
    {
        if (newState == -1)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }
        m_menuObjects[(int)m_state].SetActive(false);

        m_state = newState;

        m_menuObjects[(int)m_state].SetActive(true);
    }

    public void SetCode(string code)
    {
        PlayerPrefs.SetString("Code", code);
        m_codeText.text = code;
    }

    public void CopyCodeToClipboard()
    {
        TextEditor te = new TextEditor();
        te.text = PlayerPrefs.GetString("Code", "error");
        te.SelectAll();
        te.Copy();
    }
}
