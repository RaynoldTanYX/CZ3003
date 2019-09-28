using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    [System.Serializable]
    public enum GameType
    {
        Quiz,//0
        Crossword,
        Spot,
        Total
    }
    public GameType type;
    public string title;
    public string description;
}
