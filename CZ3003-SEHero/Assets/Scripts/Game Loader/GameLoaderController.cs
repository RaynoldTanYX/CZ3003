using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class GameLoaderController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gameQuizPrefab;
    [SerializeField]
    private GameObject m_gameCrosswordPrefab;
    [SerializeField]
    private GameObject m_gameSpotPrefab;

    private Game m_game;
    void Start()
    {
        LoadGame(PlayerPrefs.GetString("level"));
    }
    void LoadGame(string json)
    {
        Debug.Log(json);
        JSONNode node = JSON.Parse(json);
        //get game type
        int value = node["gameData"]["type"].AsInt;
        GameObject go = null;
        switch (value)
        {
            case (int)GameData.GameType.Quiz:
                Debug.Log("Loading Quiz");
                go = Instantiate(m_gameQuizPrefab);
                Debug.Log("Quiz Loaded");
                break;
            case (int)GameData.GameType.Crossword:
                Debug.Log("Loading Spot");
                go = Instantiate(m_gameCrosswordPrefab);
                Debug.Log("Spot Loaded");
                break;
            case (int)GameData.GameType.Spot:
                Debug.Log("Loading Spot");
                go = Instantiate(m_gameSpotPrefab);
                Debug.Log("Spot Loaded");
                break;
            default:
                Debug.LogError("Invalid game type: " + value);
                break;
        }
        if (go != null)
        {
            go.transform.SetParent(transform);
            m_game = go.GetComponent<Game>();
        }
        else
        {
            Debug.LogError("Failed to load game");
        }
    }

}
