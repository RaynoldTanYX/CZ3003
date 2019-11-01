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
    private Image enemyImage;


    [SerializeField]
    private AudioClip correctSound;
    [SerializeField]
    private AudioClip wrongSound;

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
                if (timer >= timeLimit)
                {
                    timer = timeLimit;
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
    public void generateHint()
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
        for (int i=0; i<finishedList.Count; i++)
        {
            Debug.Log(finishedList[i]);
        }
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
                output += " ";
        }
        letters.text = output;
        //---------------------------------------------------
        /*for (int i = 0; i < s.Length; i++)
        {
            for (int j = 0; j < numberOfHint; j++)
            {
                if (i == finishedList[j]) //hint
                {
                    output += s[i] + "\u00A0";
                }
            }
            if (!char.IsWhiteSpace(s[i]))
            {
                output += "_" + "\u00A0";
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
            generateHint();
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
        if (m_answer.text.ToLower().Equals(m_guessData.values[m_currentQuestion].answer.ToLower())) //correct
        {
            score += 100;
            StartCoroutine(HitEnemy());
            audio.PlayOneShot(correctSound);
        }
        else
        {
            audio.PlayOneShot(wrongSound);
        }
        m_answer.text = "";
        NextQuestion();
    }

    IEnumerator HitEnemy()
    {
        //flash red
        enemyImage.color = new Color(1, 0.5f, 0.5f, 0.5f);
        //play hit sound
        //wait
        yield return new WaitForSeconds(0.2f);
        //reset color
        enemyImage.color = new Color(1, 1, 1, 1);
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

    */

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

/*
 int maxCount = 0, count = 0;
    char maxLetter = ' ', temp;
    char[] s = m_guessData.values[m_currentQuestion].answer.ToCharArray();
    string filteredStr = "";
    for (int i=0; i<s.Length; i++)
    {
        if (!char.IsWhiteSpace(s[i]))
            filteredStr += s[i];
    }
    filteredStr.ToLower();
    char[] filteredArr = filteredStr.ToCharArray();

    for (int i=0; i<filteredArr.Length; i++)
    {
        temp = filteredArr[i];
        for (int j = 0; j < filteredArr.Length; j++)
        {
            if (i != j)
            {
                if (filteredArr[j].Equals(temp))
                    count++;
            }
        }
        if (count > maxCount)
        {
            maxCount = count;
            maxLetter = temp;
        }
        count = 0;
    }
 */
