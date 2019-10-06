using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorCrosswordController : EditorQuizController
{
    
    private void Start()
    {
        //m1_questionList = new List<GameObject>();
        m_questionList = new List<GameObject>();
    }

    public override void AddQuestion()
    {
        //instantiate object
        GameObject newQuestion = Instantiate(m_questionPrefab);
        //set parent
        newQuestion.transform.SetParent(m_questionParent);
        //set reference to this gameObject
        newQuestion.GetComponent<CrosswordPrefabController>().SetEditorCrosswordController(this);
        //add to list
        m_questionList.Add(newQuestion);
        //update content size fitter (work-around)
        StartCoroutine(UpdateContentSizeFitter());
        //update indexes of questions
        UpdateIndexes();
    }

    public override void UpdateIndexes()
    {
        int index = 0;
        foreach (GameObject go in m_questionList)
        {
            go.GetComponent<CrosswordPrefabController>().SetIndex(index);
            index++;
        }
    }

    public override void Publish()
    {
        data = new QuizData();
        data.gameData = new GameData();
        data.gameData.type = GameData.GameType.Crossword;
        data.gameData.title = m_title.text;
        data.gameData.description = m_description.text;
        List<QuestionValues> list = new List<QuestionValues>();
        foreach (GameObject go in m_questionList)
        {
            list.Add(go.GetComponent<CrosswordPrefabController>().GetValues());
            //Debug.Log(go.GetComponent<CrosswordPrefabController>().GetValues().answer1);
        }
        data.values = list;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("level", json);
        Debug.Log(json);
    }
}
