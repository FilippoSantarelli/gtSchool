using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using ClientSystem;
using LitJson;
using UnityEngine.SceneManagement;

public class UnityLoader : MonoBehaviour
{   
    protected GameClientManager gameClientManager;
    public GameClientManager GameClientManager { get { return gameClientManager; } }
    private UnityWebRequest unityWebRequest;
    private readonly string apiURL = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        InitGame();
    }

    private void InitGame()
    {
        // init manager
        gameClientManager = GameClientManager.Instance;

        // remove any previous listener
        BlockDataManager.OnMoldsParsed -= MoldsParsed;

        // add listener for molds parsed
        BlockDataManager.OnMoldsParsed += MoldsParsed;

        // fetch jsonData
        StartCoroutine(FetchData());
    }

    public void MoldsParsed()
    {
        BlockDataManager.OnMoldsParsed -= MoldsParsed;
        GameClient.SetManager(gameClientManager);
        SceneManager.LoadSceneAsync("GameScene");
    }

    public IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // get json data from request
                JsonData jsonData = JsonMapper.ToObject(request.downloadHandler.text);
                if (jsonData != null && jsonData.IsArray)
                {
                    // parse and set json data
                    gameClientManager.blockDataManager.SetData(jsonData);
                }
                else
                {
                    Debug.LogWarning("Invalid/null json data from web request.");
                }
            }
            else
            {
                // handle error cases
                Debug.Log(request.error);
            }
        }
    }
}
