using ClientSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class JengaStackSpawner : MonoBehaviour
{
    public Transform[] stackTransforms = new Transform[3];
    public GameObject jengaStackPrefab;
    protected bool isSpawning = false;

    private List<GameObject> stacks = new List<GameObject>();
    public delegate void StackSpawningComplete();
    public static event StackSpawningComplete OnStackSpawningComplete;

    public void OnEnable()
    {
        HUD.OnHUDButtonPress += OnHudButtonPress;
    }

    public void OnDisable()
    {
        HUD.OnHUDButtonPress -= OnHudButtonPress;
    }

    private void OnHudButtonPress(HudButtonType buttonType)
    {
        switch (buttonType)
        {
            case HudButtonType.BreakGlass:
                ClearBlocks(BlockMastery.Glass, true); 
                break;
            case HudButtonType.ResetStacks:
                SpawnStacks();
                break;

            //default:
            //    Debug.LogWarning($"Unhandled button type {buttonType}.");
            //    break;
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !isSpawning)
        {
            isSpawning = true;
            SpawnStacks();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            DestoryAllStacks();
            isSpawning = false;
        }
    }

    public void SpawnStacks()
    {
        // clear previous stacks if they exist
        if (stacks.Count > 0)
        {
            DestoryAllStacks();
        }

        if (GameClient.GetManager() == null || GameClient.GetManager().blockDataManager == null)
        {
            return;
        }

        // get dictionary of blocks
        Dictionary<string, List<BlockData>> blocksDictionary = GameClient.GetManager().blockDataManager.BlockDataDictionary;

        // spawn stacks
        int stacksToSpawn = blocksDictionary.Keys.Count;

        // check for out of bounds
        if (stacksToSpawn > stackTransforms.Length)
        {
            Debug.LogWarning("Number of stacks to spawn greater than spawn locations");
            //return;
        }

        // loop through dictionary to spawn stacks
        int loopCounter = 0;
        foreach (KeyValuePair<string, List<BlockData>> dictionaryEntry in blocksDictionary)
        {
            if (loopCounter > stackTransforms.Length -1)
                continue;

            // spawn a stack
            GameObject stackObj = Instantiate(jengaStackPrefab, stackTransforms[loopCounter], false);
            JengaStack jengaStack = stackObj.GetComponent<JengaStack>();
            jengaStack.Init(dictionaryEntry.Value, stackTransforms[loopCounter].position);
            stacks.Add(stackObj);
            loopCounter++;
        }

        if (OnStackSpawningComplete != null)
        {
            OnStackSpawningComplete();
        }
    }

    public void DestoryAllStacks()
    {
        // maybe make them clear in a cool way later
        Debug.Log("clearing stacks");
        for (int i = 0; i < stacks.Count; i++)
        {
            GameObject gameObject = stacks[i];

            if (gameObject != null)
                GameObject.Destroy(gameObject);
        }

        stacks.Clear();
    }

    /// <summary>
    /// Loops through all stacks and removes any block of matching type
    /// </summary>
    /// <param name="blockMastery">Block mastery we want to remove.</param>
    /// <param name="enableIsKinematic">Bool to determine if isKinematic should be enable on the blocks after clearing.</param>
    public void ClearBlocks(BlockMastery blockMastery, bool enableIsKinematic)
    {
        // loop through stacks
        for (int i = 0; i < stacks.Count; i++)
        {
            // assign local reference
            JengaStack stack = stacks[i].GetComponent<JengaStack>();
            if (stack != null)
            {
                // call stack to remove its blocks of matching mastery
                stack.RemoveBlocks(blockMastery);
            }
        }
    }
}

