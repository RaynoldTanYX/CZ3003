using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct QuestionValuesCrossword
{
    public int index;
    public string question;
    public string answer;
}

[System.Serializable]
public class CrosswordData
{
    public GameData gameData;
    public List<QuestionValuesCrossword> values;
}
