using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorQuizController : MonoBehaviour
{

    [SerializeField]
    protected GameObject m_questionPrefab;

    [SerializeField]
    protected Transform m_questionParent;
    protected List<GameObject> m_questionList;

    [SerializeField]
    protected RectTransform m_contentTransform;

    [SerializeField]
    protected InputField m_title;
    [SerializeField]
    protected InputField m_description;

    [SerializeField]
    protected QuizData data;

    private void Start()
    {
        m_questionList = new List<GameObject>();
    }
    public virtual void AddQuestion()
    {
        //instantiate object
        GameObject newQuestion = Instantiate(m_questionPrefab);
        //set parent
        newQuestion.transform.SetParent(m_questionParent);
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

    public virtual void UpdateIndexes()
    {
        int index = 0;
        foreach (GameObject go in m_questionList)
        {
            go.GetComponent<QuizPrefabController>().SetIndex(index);
            index++;
        }
    }

    public virtual void Publish()
    {
        data = new QuizData();
        data.gameData = new GameData();
        data.gameData.type = GameData.GameType.Quiz;
        data.gameData.title = m_title.text;
        data.gameData.description = m_description.text;
        List<QuestionValues> list = new List<QuestionValues>();
        foreach(GameObject go in m_questionList)
        {
            list.Add(go.GetComponent<QuizPrefabController>().GetValues());
            //Debug.Log(go.GetComponent<QuizPrefabController>().GetValues().answer1);
        }
        data.values = list;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("level", json);
        Debug.Log(json);
    }
}
