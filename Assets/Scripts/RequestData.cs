using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class RequestData : MonoBehaviour
{
    private readonly string apiURL = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

    public static List<BlockData> blocks = new List<BlockData>();
    public Button apiButton;

    public void Awake()
    {
        apiButton.onClick.AddListener(GetData);
    }

    public void GetData()
    {
        blocks.Clear();
        Debug.Log("Retrieving Data");
        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                //JsonData jsonData = JsonReader.(request.downloadHandler.text); .FromJson<PlayerStatus>(request.downloadHandler.text);

                // get json data from request
                JsonData jsonData = JsonMapper.ToObject(request.downloadHandler.text);
                //blocks.Add(blockData);
                if (jsonData != null && jsonData.IsArray)
                {
                    // assign data count
                    int dataCount = jsonData.Count;
                    //List<T> list = new List<T>(dataCount);
                    for (int i = 0; i < dataCount; i++)
                    {
                        // create data block
                        BlockData blockData = new BlockData();
                        blockData.ParseJSON(jsonData[i]);
                        blocks.Add(blockData);
                    }
                }
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }
}

