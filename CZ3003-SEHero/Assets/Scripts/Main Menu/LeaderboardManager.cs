using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System;
using SimpleJSON;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private Text top1;

    [SerializeField]
    private Text top2;

    [SerializeField]
    private Text top3;

    [SerializeField]
    private Text top4;

    [SerializeField]
    private Text top5;

    [SerializeField]
    private Text leadershipText;

    public DatabaseManager dbManager;
    
    public void UpdateLeaderboard()
    {
        leadershipText.text = "Leaderboard of World " + PlayerPrefs.GetInt("worldid", 0);

        StartCoroutine(dbManager.GetLeaderboard(PlayerPrefs.GetInt("worldid", 0), LeaderboardCallback));
    }

    private void LeaderboardCallback(bool success, JSONNode scores) {
        if (success) {
            top1.text = scores[0]["score"] + " - " + scores[0]["username"];
            top2.text = scores[1]["score"] + " - " + scores[1]["username"];
            top3.text = scores[2]["score"] + " - " + scores[2]["username"];
            top4.text = scores[3]["score"] + " - " + scores[3]["username"];
            top5.text = scores[4]["score"] + " - " + scores[4]["username"];
        }
    }
}
