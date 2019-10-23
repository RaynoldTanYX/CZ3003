using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCrossword : Game
{
    private CrosswordData m_crosswordData;
    private int m_currentQuestion;
    private char[][] m_crosswordLetters;

    [SerializeField]
    private Text m_question;
    [SerializeField]
    private Text m_answer;


    [SerializeField]
    private GameObject m_crosswordPrefab;

    void Start()
    {
        m_currentQuestion = -1;
        m_crosswordData = JsonUtility.FromJson<CrosswordData>(PlayerPrefs.GetString("level"));
        Debug.Log("Number of questions: " + m_crosswordData.values.Count);
        //SplitCrossword(m_crosswordData);
        //GenerateBoard(m_crosswordLetters);
    }

    void Update()
    {
        switch (m_gameState)
        {
            case GameState.Ready:
                break;
            case GameState.Playing:
                if (m_currentQuestion == -1)
                {
                    //NextQuestion();
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
        if (m_currentQuestion < m_crosswordData.values.Count)
        {
            QuestionValuesCrossword values = m_crosswordData.values[m_currentQuestion];
            m_question.text = values.question;
            m_answer.text = values.answer;
            Debug.Log("Question loaded: " + m_currentQuestion);
        }
        else
        {
            Debug.Log("All questions completed");
            WinGame(); //TODO: Set win/lose condition
        }
    }

    void SplitCrossword(CrosswordData m_crosswordData)
    {
        for (int i = 0; i < m_crosswordData.values.Count; i++)
        {
            for (int j = 0; j < m_crosswordData.values[i].answer.ToString().Length; j++)
            {
                m_crosswordLetters[i] = m_crosswordData.values[i].answer.ToCharArray();
            }
        }
    }

    void GenerateBoard(char[][] arr)
    {
        GameObject word = Instantiate(m_crosswordPrefab, new Vector3(50, 50, 0), transform.rotation);
    }

}
