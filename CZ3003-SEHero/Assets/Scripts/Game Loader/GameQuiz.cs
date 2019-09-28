using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameQuiz : Game
{
    private QuizData m_quizData;
    private int m_currentQuestion;

    [SerializeField]
    private Text m_question;
    [SerializeField]
    private Text m_answer1;
    [SerializeField]
    private Text m_answer2;
    [SerializeField]
    private Text m_answer3;
    [SerializeField]
    private Text m_answer4;

    private int m_correct;

    void Start()
    {
        m_currentQuestion = -1;
        m_quizData = JsonUtility.FromJson<QuizData>(PlayerPrefs.GetString("level"));
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_gameState)
        {
            case GameState.Ready:
                break;
            case GameState.Playing:
                if (m_currentQuestion == -1)
                {
                    NextQuestion();
                }
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }

    void NextQuestion()
    {
        m_currentQuestion++;
        if (m_currentQuestion < m_quizData.values.Count)
        {
            QuestionValues values = m_quizData.values[m_currentQuestion];
            m_question.text = values.question;
            m_answer1.text = values.answer1;
            m_answer2.text = values.answer2;
            m_answer3.text = values.answer3;
            m_answer4.text = values.answer4;
            m_correct = values.correct;
            Debug.Log("Question loaded: " + m_currentQuestion);
        }
        else
        {
            Debug.Log("All questions completed");
            WinGame(); //TODO: Set win/lose condition
        }
    }

    public void ChooseOption(int index)
    {
        if (index + 1 == m_correct)
        {
            NextQuestion();
        }
        else
        {

        }
    }
}
