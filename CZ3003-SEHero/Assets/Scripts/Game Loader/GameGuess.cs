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

    private float timer;

    private float timeLimit = 30;

    [SerializeField]
    private List<GameObject> bombs;


    [SerializeField]
    private AudioClip correctSound;
    [SerializeField]
    private AudioClip wrongSound;
    [SerializeField]
    private AudioClip explodeSound;

    private AudioSource audio;

    [SerializeField]
    private Text timerText;

    private List<int> uniqueNumbers;
    private List<int> finishedList;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        m_currentQuestion = -1;
        m_guessData = JsonUtility.FromJson<GuessData>(PlayerPrefs.GetString("level"));
        uniqueNumbers = new List<int>();
        finishedList = new List<int>();

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
                timer += Time.deltaTime;
                timerText.text = "Time left\n" + (int)(timeLimit - timer);

                if ((int)(timeLimit - timer) == 20)
                    bombs[1].SetActive(true);
                else if ((int)(timeLimit - timer) == 5)
                    bombs[2].SetActive(true);
                
                if (timer >= timeLimit)
                {
                    audio.PlayOneShot(explodeSound);
                    timer = timeLimit;
                    finishedList.Clear();
                    uniqueNumbers.Clear();
                    bombs[3].SetActive(true);
                    NextQuestion();
                }
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }

    public void GenerateRandomList(int [] arr, int length, int numberOfHint)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            uniqueNumbers.Add(arr[i]);
        }
        for (int i = 0; i < numberOfHint; i++)
        {
            int ranNum = uniqueNumbers[Random.Range(0, uniqueNumbers.Count)];
            while (!uniqueNumbers.Contains(ranNum))
            {
                ranNum = uniqueNumbers[Random.Range(0, uniqueNumbers.Count)];
            }
            finishedList.Add(ranNum);
            uniqueNumbers.Remove(ranNum);
        }
    }
    public void GenerateHint()
    {

        string answer = m_guessData.values[m_currentQuestion].answer.ToUpper();     
        char[] s = answer.ToCharArray();                                                    // char [] s = SOFTWARE ENGINEERING
        char[] outputArr = new char[s.Length];
        int j = 0;
        for (int i=0; i <s.Length; i++)
        {
            if (!char.IsWhiteSpace(s[i]))
                j++;
            else
                outputArr[i] = ' ';
        }
        int[] indexArr = new int [j];
        j = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (!char.IsWhiteSpace(s[i]))
            {
                indexArr[j] = i;
                j++;
            }
        }
        int numberOfHint = (int)(0.4 * s.Length);

        GenerateRandomList(indexArr, s.Length, numberOfHint);
        Debug.Log("RANDOM INDEXES");
        string output = "";
        
        for (int i = 0; i < numberOfHint; i++)
        {
            outputArr[finishedList[i]] = s[finishedList[i]];
        }
        Debug.Log("OUTPUT STRING");
        for (int i = 0; i < s.Length; i++)
        {
            if (outputArr[i].Equals('\u0000'))
            {
                Debug.Log("null");
            }
            else
                Debug.Log(outputArr[i]);
            
        }
        //---------------------------------------------------
        for (int i = 0; i < outputArr.Length; i++)
        {
            if (outputArr[i].Equals('\u0000'))
                output += "_" + "\u00A0";
            else if (!char.IsWhiteSpace(outputArr[i]))
            {
                output += outputArr[i];
            }
            else
                output += "      ";
        }
        letters.text = output;
    }

    void NextQuestion()
    {
        m_currentQuestion++;
        timer = 0;
        if (m_currentQuestion < m_guessData.values.Count)
        {
            QuestionValuesGuess values = m_guessData.values[m_currentQuestion];
            m_question.text = values.question;
            Debug.Log("Question loaded: " + m_currentQuestion);
            for (int i = 1; i < 4; i++)
                bombs[i].SetActive(false);
            GenerateHint();
        }
        else
        {
            Debug.Log("All questions completed");
            for (int i = 0; i < 4; i++)
                bombs[i].SetActive(false);
            WinGame(score); //TODO: Set win/lose condition
        }
    }


    public void CheckAnswer()
    {
        Debug.Log("Check Answer");
        if (m_answer.text.ToLower().Equals(m_guessData.values[m_currentQuestion].answer.ToLower())) //correct
        {
            score += 100;
            audio.PlayOneShot(correctSound);
            finishedList.Clear();
            uniqueNumbers.Clear();
            NextQuestion();
        }
        else
            audio.PlayOneShot(wrongSound);
        m_answer.text = "";
    }
}