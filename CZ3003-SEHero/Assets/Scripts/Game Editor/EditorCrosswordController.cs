using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorCrosswordController : MonoBehaviour
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

    [SerializeField]
    private GameObject gameSelectPanel;
    [SerializeField]
    private GameObject gamePanel;


    private void Start()
    {
        m_questionList = new List<GameObject>();
        gameSelectPanel.SetActive(false);
    }

    public void AddQuestion()
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
            go.GetComponent<CrosswordPrefabController>().SetIndex(index);
            index++;
        }
    }

    public void Publish()
    {
        CrosswordData data = new CrosswordData();
        data.gameData = new GameData();
        data.gameData.type = GameData.GameType.Crossword;
        data.gameData.title = m_title.text;
        data.gameData.description = m_description.text;
        List<QuestionValuesCrossword> list = new List<QuestionValuesCrossword>();
        foreach (GameObject go in m_questionList)
        {
            list.Add(go.GetComponent<CrosswordPrefabController>().GetValues());
            //Debug.Log(go.GetComponent<QuizPrefabController>().GetValues().answer1);
        }
        data.values = list;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("level", json);
        StartCoroutine(dbManager.SaveLevel(m_title.text, "0", json, PublishCallback));
        Debug.Log("ran");
        Debug.Log(json);
    }
    private void PublishCallback(bool success)
    {
        Debug.Log(success);
        ec.ChangeState(0);
    }

    public void Back()
    {
        gamePanel.SetActive(false);
        gameSelectPanel.SetActive(true);
        ec.setState(0);
    }
}
