using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class GuessPrefabController : MonoBehaviour
{
    private EditorGuessController m_editorGuessController;
    [SerializeField]
    private Text m_text;

    private QuestionValuesGuess m_qnValues;

    [SerializeField]
    private InputField question;
    [SerializeField]
    private InputField answer;

    public void SetEditorGuessController(EditorGuessController editorGuessController)
    {
        m_editorGuessController = editorGuessController;
    }

    public void SetIndex(int index)
    {
        m_qnValues.index = index;
        gameObject.name = "Question #" + (index + 1);
        m_text.text = gameObject.name;
    }
    public void Remove()
    {
        m_editorGuessController.RemoveQuestion(m_qnValues.index);
    }

    public void Move(bool down)
    {
        m_editorGuessController.MoveQuestion(m_qnValues.index, down);
    }

    public QuestionValuesGuess GetValues()
    {
        UpdateValues();
        return m_qnValues;
    }

    public void UpdateValues()
    {
        m_qnValues.question = question.text;
        m_qnValues.answer = answer.text;
    }
}
