using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGuess : Game
{
    private GuessData m_guessData;
    private int m_currentQuestion;

    [SerializeField]
    private Text m_question;
    [SerializeField]
    private InputField m_answer;
    [SerializeField]
    private Text letters;

    [SerializeField]
    private DatabaseManager db;

    [SerializeField]
    private GameObject m_correctPrefab;
    [SerializeField]
    private GameObject m_wrongPrefab;

    private char[] originalStr;
    private char[] editedStr;
    //private ArrayList hintArr;
    private int count = 0;

    void Start()
    {
        m_currentQuestion = -1;
        m_guessData = JsonUtility.FromJson<GuessData>(PlayerPrefs.GetString("level"));
        //hintArr.Add('S');
        int num = 10;
        score = 0;
        
        m_gameState = GameState.Ready;
        StartGame();
    }

    private void GetLevelCallback(bool success, string name, string data)
    {
        Debug.Log("called");
        if (success)
        {
            Debug.Log("success");
        }
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
                    NextQuestion();
                }
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }

    void generateQuestion()
    {
        originalStr = m_guessData.values[m_currentQuestion].answer.ToCharArray();
        string output = "";
        string temp = "";
        for (int i=0; i< originalStr.Length; i++)
        {
            if (!char.IsWhiteSpace(originalStr[i]))
            {
                output += "_" + "\u00A0";
                temp += originalStr[i];
            }
            else
                output += " ";
        }
        letters.text = output;
        editedStr = temp.ToCharArray();
    }

    public void generateHint()
    {
        /*
        char[] s = m_crosswordData.values[m_currentQuestion].answer.ToCharArray();
        bool check = false;
        while (!check)
        {
            //int randomNum = Random.Range(0, count);
            int randomNum = 0;
            char hint = editedStr[randomNum];
            if (hintArr.Count== 0)
            {
                hintArr[0] = hint;
                check = true;
            }
            else
            {
                for (int i = 0; i < hintArr.Count; i++)
                {
                    if (hint == (char)hintArr[i])
                        break;
                }
                check = true;
                hintArr.Add(hint);
            }
        }
        string output = "";
        for (int i = 0; i < originalStr.Length; i++)
        {
            if (originalStr[i].Equals(hint))
            {
                output += hint + "\u00A0";
            }
            else if (!char.IsWhiteSpace(originalStr[i]))
            {
                output += "_" + "\u00A0";
                editedStr[count] = originalStr[i];
                count++;
            }
            else
                output += " ";
        }
        letters.text = output;
        */
    }

    void NextQuestion()
    {
        m_currentQuestion++;
        if (m_currentQuestion < m_guessData.values.Count)
        {
            QuestionValuesGuess values = m_guessData.values[m_currentQuestion];
            m_question.text = values.question;
            Debug.Log("Question loaded: " + m_currentQuestion);
            generateQuestion();
            //generateHint();
        }
        else
        {
            Debug.Log("All questions completed");
            WinGame(score); //TODO: Set win/lose condition
        }
    }
    
    

    public void CheckAnswer()
    {
        Debug.Log("Check Answer");
        if (m_answer.text == m_guessData.values[m_currentQuestion].answer) //correct
        {
            score += 100;
            m_correctPrefab.SetActive(true);
            Debug.Log("CORRECT");
            m_answer.text = "";
        }
        else
        {
            m_wrongPrefab.SetActive(true);
            m_answer.text = "";
        }
    }

    public void CloseNotif(bool correct)
    {
        if (correct)
            m_correctPrefab.SetActive(false);
        else
            m_wrongPrefab.SetActive(false);
        NextQuestion();
    }
}



/*
    void initLetters(int count)
    {
        
        for (int i=0; i<count; i++)
        {
            Vector3 newPosition;
            newPosition = new Vector3(cen.transform.position.x + ((i - count / 2.0f) * 3* count), cen.transform.position.y, cen.transform.position.z);
            GameObject l = (GameObject)Instantiate(m_letterPrefab, newPosition, Quaternion.identity);
            
            //l.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            l.name = "letter" + (i + 1);
            l.transform.SetParent(GameObject.Find("Content").transform);
            
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


    */
