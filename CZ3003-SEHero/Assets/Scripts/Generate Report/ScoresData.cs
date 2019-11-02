using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoresData
{
    public List<ScoreList> scores;
}

[System.Serializable]
public class ScoreList
{
    public int level_id;
    public int world_id;
    public string username;
    public int score;
}