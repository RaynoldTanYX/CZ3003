using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class LetterPrefabController : MonoBehaviour
{
    [SerializeField]
    protected Text m_text;

    public void SetLetter(char letter)
    {
        m_text.text = letter.ToString();
    }
}
