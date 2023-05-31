using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockMastery
{
    undefined = -1,
    Glass = 0,
    Wood = 1,
    Stone = 2
}

public class BlockData
{
    public int uniqueID;
    public string blockSubject;
    public string gradeLevel;
    public int blockMastery;
    public string domainID;
    public string blockDomain;
    public string blockCluster;
    public string standardID;
    public string blockStandardDescription;

    public BlockData()
    {
    }

    public void ParseJSON(JsonData data)
    {
        foreach (string key in data.Keys)
        {
            switch (key)
            {
                case "id":
                    uniqueID = data.Get(key).GetInt();
                    break;
                case "subject":
                    blockSubject = data.Get(key).GetString();
                    break;
                case "grade":
                    gradeLevel = data.Get(key).GetString();
                    break;
                case "mastery":
                    blockMastery = data.Get(key).GetInt();
                    break;
                case "domainid":
                    domainID = data.Get(key).GetString();
                    break;
                case "domain":
                    blockDomain = data.Get(key).GetString();
                    break;
                case "cluster":
                    blockCluster = data.Get(key).GetString();
                    break;
                case "standardid":
                    standardID = data.Get(key).GetString();
                    break;
                case "standarddescription":
                    blockStandardDescription = data.Get(key).GetString();
                    break;

                default:
                    Debug.LogError($"Unmapped key {key} in data.");
                    break;
            }
        }
    }
}
