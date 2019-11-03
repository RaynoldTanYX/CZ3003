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

    private float timer;

    private float timeLimit = 10;

    [SerializeField]
    private Image enemyImage;

    [SerializeField]
    private AudioClip correctSound;
    [SerializeField]
    private AudioClip wrongSound;

    private AudioSource audio;

    [SerializeField]
    private Text timerText;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        m_currentQuestion = -1;
        m_quizData = JsonUtility.FromJson<QuizData>(PlayerPrefs.GetString("level"));
        score = 0;
        m_gameState = GameState.Ready;
        StartGame();
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
                timer += Time.deltaTime;
                Debug.Log("Time left " + (int)(timeLimit - timer));
                timerText.text = "Time left\n" + (int)(timeLimit - timer);
                if (timer >= timeLimit)
                {
                    timer = timeLimit;
                    NextQuestion();
                    Debug.Log("Times up!");
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
        timer = 0;
        m_currentQuestion++;
        Debug.Log("quizData count: " + m_quizData.values.Count);
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
            WinGame(score); //TODO: Set win/lose condition
        }
    }

    public void ChooseOption(int index)
    {
        if (index == m_correct)
        {
            score += 100;
            StartCoroutine(HitEnemy());
            audio.PlayOneShot(correctSound);
            Debug.Log("Correct!");
        }
        else
        { 
            audio.PlayOneShot(wrongSound);
            Debug.Log("Wrong!");
        }
        
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
