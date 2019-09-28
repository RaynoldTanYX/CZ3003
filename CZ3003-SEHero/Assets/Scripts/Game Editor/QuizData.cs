using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct QuestionValues
{
    public int index;
    public string question;
    public string answer1;
    public string answer2;
    public string answer3;
    public string answer4;
    public int correct;
}


[System.Serializable]
public class QuizData
{
    public GameData gameData;
    public List<QuestionValues> values;
}
