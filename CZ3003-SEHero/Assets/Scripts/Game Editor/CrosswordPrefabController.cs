using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class CrosswordPrefabController : MonoBehaviour
{
    private EditorCrosswordController m_editorCrosswordController;
    [SerializeField]
    private Text m_text;

    private QuestionValuesCrossword m_qnValues;

    [SerializeField]
    private InputField question;
    [SerializeField]
    private InputField answer;

    public void SetEditorCrosswordController(EditorCrosswordController editorCrosswordController)
    {
        m_editorCrosswordController = editorCrosswordController;
    }

    public void SetIndex(int index)
    {
        m_qnValues.index = index;
        gameObject.name = "Question #" + (index + 1);
        m_text.text = gameObject.name;
    }
    public void Remove()
    {
        m_editorCrosswordController.RemoveQuestion(m_qnValues.index);
    }

    public void Move(bool down)
    {
        m_editorCrosswordController.MoveQuestion(m_qnValues.index, down);
    }

    public QuestionValuesCrossword GetValues()
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
