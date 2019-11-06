﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorQuizController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_questionPrefab;

    [SerializeField]
    private Transform m_questionParent;
    private List<GameObject> m_questionList;

    [SerializeField]
    private RectTransform m_contentTransform;

    [SerializeField]
    private InputField m_title;
    [SerializeField]
    private InputField m_description;

    [SerializeField]
    private QuizData data;

    [SerializeField]
    private DatabaseManager dbManager;

    [SerializeField]
    private EditorController ec;


    private void Start()
    {
        m_questionList = new List<GameObject>();
    }
    public void AddQuestion()
    {
        //instantiate object
        GameObject newQuestion = Instantiate(m_questionPrefab);
        //set parent
        newQuestion.transform.SetParent(m_questionParent, false);
        //set reference to this gameObject
        newQuestion.GetComponent<QuizPrefabController>().SetEditorCrosswordController(this);
        //add to list
        m_questionList.Add(newQuestion);
        //update content size fitter (work-around)
        StartCoroutine(UpdateContentSizeFitter());
        //update indexes of questions
        UpdateIndexes();
    }

    public IEnumerator UpdateContentSizeFitter()
    {
        yield return new WaitForFixedUpdate();
        //Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_contentTransform);
    }

    public void RemoveQuestion(int index)
    {
        if (index >= 0 && index < m_questionList.Count)
        {
            Destroy(m_questionList[index]);
            m_questionList.Remove(m_questionList[index]);
            Debug.Log("Question removed: " + index);
            //update indexes of questions
            UpdateIndexes();
            //update content size fitter (work-around)
            StartCoroutine(UpdateContentSizeFitter());
        }
        else
        {
            Debug.LogError("Index out of range: " + index);
        }
    }

    public void MoveQuestion(int index, bool down)
    {
        if (index >= 0 && index < m_questionList.Count)
        {
            if (down)//move down
            {
                if (index + 1 < m_questionList.Count)//if not last index already
                {
                    m_questionList[index].transform.SetSiblingIndex(m_questionList[index].transform.GetSiblingIndex() + 1);
                    GameObject temp = m_questionList[index + 1];
                    m_questionList[index + 1] = m_questionList[index];
                    m_questionList[index] = temp;
                }
                else
                {
                    Debug.Log("Question is already at last index");
                }
            }
            else//move up
            {
                if (index > 0)//if not first index already
                {
                    m_questionList[index].transform.SetSiblingIndex(m_questionList[index].transform.GetSiblingIndex() - 1);
                    GameObject temp = m_questionList[index - 1];
                    m_questionList[index - 1] = m_questionList[index];
                    m_questionList[index] = temp;
                }
                else
                {
                    Debug.Log("Question is already at first index");
                }
            }
            //update indexes of questions
            UpdateIndexes();
        }
        else
        {
            Debug.LogError("Index out of range: " + index);
        }
    }

    public void UpdateIndexes()
    {
        int index = 0;
        foreach (GameObject go in m_questionList)
        {
            go.GetComponent<QuizPrefabController>().SetIndex(index);
            index++;
        }
    }

    public void Publish()
    {
        data = new QuizData();
        data.gameData = new GameData();
        data.gameData.type = GameData.GameType.Quiz;
        data.gameData.title = m_title.text;
        data.gameData.description = m_description.text;
        List<QuestionValues> list = new List<QuestionValues>();
        foreach (GameObject go in m_questionList)
        {
            list.Add(go.GetComponent<QuizPrefabController>().GetValues());
            //Debug.Log(go.GetComponent<QuizPrefabController>().GetValues().answer1);
        }
        data.values = list;
        //test cases
        string errorMessage = "";
        if (data.gameData.title.Length == 0)
        {
            errorMessage += "Title cannot be empty.\n";
        }
        if (data.gameData.description.Length == 0)
        {
            errorMessage += "Description cannot be empty.\n";
        }
        foreach (QuestionValues value in data.values)
        {
            if (value.question.Length == 0)
            {
                errorMessage += "Question #" + (value.index + 1) + " cannot be empty.\n";
            }
            if (value.answer1.Length == 0)
            {
                errorMessage += "Answer 1 for question #" + (value.index + 1) + " cannot be empty.\n";
            }
            if (value.answer2.Length == 0)
            {
                errorMessage += "Answer 2 for question #" + (value.index + 1) + " cannot be empty.\n";
            }
            if (value.answer3.Length == 0)
            {
                errorMessage += "Answer 3 for question #" + (value.index + 1) + " cannot be empty.\n";
            }
            if (value.answer4.Length == 0)
            {
                errorMessage += "Answer 4 for question #" + (value.index + 1) + " cannot be empty.\n";
            }
            if (value.correct < 1 || value.correct > 4)
            {
                errorMessage += "Correct answer for question #" + (value.index + 1) + " cannot be empty.\n";
            }

        }
        if (errorMessage != "")
        {
            //show message
            MessagePanel.GetInstance().ShowMessage(errorMessage);
            return;//dont send to database
        }

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("level", json);
        StartCoroutine(dbManager.SaveLevel(m_title.text, "0", json, PublishCallback));
        Debug.Log("ran");
        Debug.Log(json);
    }

    protected void PublishCallback(int levelid)
    {
        Debug.Log("Level ID: " + levelid);
        ec.ChangeState(3);
        ec.SetCode(levelid.ToString());
    }
}
