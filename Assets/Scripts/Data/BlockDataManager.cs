using ClientSystem;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockDataManager 
{
    public List<BlockData> blockDataList;
    // dictionary of string (grade) with a list of block data
    protected Dictionary<string, List<BlockData>> blockDataDictionary;
    public Dictionary<string, List<BlockData>> BlockDataDictionary { get { return blockDataDictionary; } }
    private static BlockDataManager instance = null;

    public delegate void MoldsParsed();
    public static event MoldsParsed OnMoldsParsed;

    private BlockDataManager()
    {
        blockDataList = new List<BlockData>();
        blockDataDictionary = new Dictionary<string, List<BlockData>>();
    }

    public static BlockDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BlockDataManager();
            }
            return instance;
        }
    }

    public void SetData(JsonData jsonData)
    {
        // assign data count
        int dataCount = jsonData.Count;
        for (int i = 0; i < dataCount; i++)
        {
            // create data block
            BlockData blockData = new BlockData();
            blockData.ParseJSON(jsonData[i]);
            blockDataList.Add(blockData);
        }

        if (OnMoldsParsed != null)
        {
            Debug.Log("parsing complete");

            SeparateBlocksByGrades();
            OnMoldsParsed();
        }
    }

    /// <summary>
    /// Goes through all block data to populate a dictionary of <string, List<BlockData>>
    /// </summary>
    public void SeparateBlocksByGrades()
    {
        IList<List<BlockData>> groups = blockDataList.GroupBy(x => x.gradeLevel).Select(x => x.ToList()).ToList();
        foreach (List<BlockData> group in groups)
        {
            // get current grade being looped over
            string currentGrade = group[0].gradeLevel;

            // sort data in current group
            group.Sort(SortBlocks);

            // do not add duplicate keys
            if (!blockDataDictionary.ContainsKey(currentGrade))
                blockDataDictionary.Add(currentGrade, group);
        }

        //List<BlockData> duplicateList = new List<BlockData>();
        //duplicateList.AddRange(blockDataList.OrderBy(x => x.gradeLevel));

        //// reverse loop to remove elements as they are added to the grouped list
        //for (int i = duplicateList.Count - 1; i >= 0; i--)
        //{
        //    // get current grade being looped over
        //    string currentGrade = duplicateList[i].gradeLevel;

        //    List<BlockData> groupedList = new List<BlockData>();

        //    // go through duplicates list
        //    for (int j = duplicateList.Count - 1; j >= 0; j--)
        //    {
        //        // check if grade is a match
        //        if (duplicateList[j].gradeLevel == currentGrade)
        //        {
        //            // add to grouped list and then remove from duplicate list
        //            groupedList.Add(duplicateList[j]);
        //            duplicateList.RemoveAt(j);  
        //        }
        //    }

        //    // do not add duplicate keys
        //    if (!blockDataDictionary.ContainsKey(currentGrade))
        //        blockDataDictionary.Add(currentGrade, groupedList);
        //}
    }

    public int SortBlocks(BlockData blockA, BlockData blockB)
    {
        // Order the blocks in the stack starting from the bottom up, by domain name ascending,
        // then by cluster name ascending, then by standard ID ascending

        string domainIdA = blockA.domainID;
        string domainIdB = blockB.domainID;

        int result = domainIdA.CompareTo(domainIdB);

        // same domain
        if (result == 0)
        {
            string clusterA = blockA.blockCluster;
            string clusterB = blockB.blockCluster;

            result = clusterA.CompareTo(clusterB);

            // same cluster
            if (result == 0)
            {
                result = blockA.uniqueID.CompareTo(blockB.uniqueID);
                if (result == 0)
                {
                    // error in data, unique ID appears twice
                    Debug.LogError($"ID: {blockB.uniqueID} is not unique.");
                }
            }
        }

        return result;
    }
}
