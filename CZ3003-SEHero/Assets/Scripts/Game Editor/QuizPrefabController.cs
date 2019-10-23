using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class QuizPrefabController : MonoBehaviour
{
    private EditorQuizController m_editorQuizController;
    [SerializeField]
    protected Text m_text;

    protected QuestionValues m_qnValues;

    [SerializeField]
    protected InputField question;
    [SerializeField]
    protected InputField answer1;
    [SerializeField]
    protected InputField answer2;
    [SerializeField]
    protected InputField answer3;
    [SerializeField]
    protected InputField answer4;
    [SerializeField]
    protected Dropdown correct;

    public void SetEditorCrosswordController(EditorQuizController editorQuizController)
    {
        m_editorQuizController = editorQuizController;
    }

    public void SetIndex(int index)
    {
        m_qnValues.index = index;
        gameObject.name = "Question #" + (index + 1);
        m_text.text = gameObject.name;
    }
    public void Remove()
    {
        m_editorQuizController.RemoveQuestion(m_qnValues.index);
    }

    public void Move(bool down)
    {
        m_editorQuizController.MoveQuestion(m_qnValues.index, down);
    }

    public QuestionValues GetValues()
    {
        UpdateValues();
        return m_qnValues;
    }

    public void UpdateValues()
    {
        m_qnValues.question = question.text;
        m_qnValues.answer1 = answer1.text;
        m_qnValues.answer2 = answer2.text;
        m_qnValues.answer3 = answer3.text;
        m_qnValues.answer4 = answer4.text;
        m_qnValues.correct = correct.value;
    }
}
