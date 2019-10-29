using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct QuestionValuesGuess
{
    public int index;
    public string question;
    public string answer;
}

[System.Serializable]
public class GuessData
{
    public GameData gameData;
    public List<QuestionValuesGuess> values;
}
