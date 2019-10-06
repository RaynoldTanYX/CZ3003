using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class CrosswordPrefabController : QuizPrefabController
{
    private EditorCrosswordController m_editorCrosswordController;
    

    public void SetEditorCrosswordController(EditorCrosswordController editorCrosswordController)
    {
        m_editorCrosswordController = editorCrosswordController;
    }

    public override void Remove()
    {
        m_editorCrosswordController.RemoveQuestion(m_qnValues.index);
    }

    public override void Move(bool down)
    {
        m_editorCrosswordController.MoveQuestion(m_qnValues.index, down);
    }

    public override void UpdateValues()
    {
        m_qnValues.question = question.text;
        m_qnValues.answer1 = answer1.text;
    }
}
