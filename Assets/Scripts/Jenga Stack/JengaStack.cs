using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JengaStack : MonoBehaviour
{
    public Transform blockTransform;
    public TextMeshProUGUI stackName;
    public GameObject jengaBlockPrefab;

    protected int blocksInTower;
    private List<GameObject> blocks = new List<GameObject>();
    private Vector3 transformOffset;

    public void OnEnable()
    {
        DestroyCollider.OnBlockCollision += HandleBlockCollision;
    }

    public void OnDisable()
    {
        DestroyCollider.OnBlockCollision -= HandleBlockCollision;
    }

    private void HandleBlockCollision(GameObject collidingBlock)
    {
        JengaBlock jengaBlock = collidingBlock.GetComponent<JengaBlock>();
        if (jengaBlock != null)
        {
            // check block matches current grade stack
            if (jengaBlock.BlockData.gradeLevel == stackName.text)
            {
                blocks.Remove(collidingBlock);
                Destroy(collidingBlock);
            }
        }
    }

    public void Init(List<BlockData> blocksData, Vector3 transformOffset)
    {
        // reference for offset from spawn transform to apply to spawning position
        this.transformOffset = transformOffset;

        // set stack name
        stackName.text = blocksData[0].gradeLevel;

        // spawn blocks with data
        SpawnBlocks(blocksData);
    }

    public void LateUpdate()
    {
        // code used to rotate world space UI to face the camera
        stackName.transform.LookAt(stackName.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void SpawnBlocks(List<BlockData> blocksData)
    {
        int blockQuantity = blocksData.Count > 0 ? blocksData.Count : 30;
        // limit for how many blocks can be placed per row
        int blocksPerRow = 3;
        // count for blocks in the current row
        int blocksInCurrentRow = 0;
        // index for the current row we are in
        int currentRow = 0;

        Quaternion rotation = jengaBlockPrefab.transform.rotation;

        for (int i = 0; i < blockQuantity; i++)
        {
            // check if row should be moved up
            if (blocksInCurrentRow == blocksPerRow)
            {
                // reset counter
                blocksInCurrentRow = 0;

                // reset roation
                rotation = jengaBlockPrefab.transform.rotation;

                // increase row counter
                currentRow++;
            }

            // alternate assigning spawn position for each row
            Vector3 position = new Vector3();
            if (currentRow % 2 == 0)
            {
                position.x = 0 + transformOffset.x;
                position.y = currentRow;
                position.z = blocksInCurrentRow;
            }
            else
            {
                position.x = 1 + blocksInCurrentRow + transformOffset.x;
                position.y = currentRow;
                position.z = 0;

                // also set roation
                rotation = Quaternion.Euler(0, -90, 0);
            }

            // spawn block
            GameObject gameObj = Instantiate(jengaBlockPrefab, position, rotation, blockTransform);
            JengaBlock jengaBlock = gameObj.GetComponent<JengaBlock>();
            jengaBlock.Init(blocksData[i]);
            //jengaBlock.Init(blockData[i]);
            blocks.Add(gameObj);

            // increase counter
            blocksInCurrentRow++;
        }
    }

    public void RemoveBlocks(BlockMastery targetBlockMastery)
    {
        for (int i = blocks.Count - 1; i >= 0; i--)
        {
            JengaBlock jengaBlock = blocks[i].GetComponent<JengaBlock>();

            if (jengaBlock != null)
            {
                // if block mastery is the same remove block
                if ((BlockMastery)jengaBlock.BlockData.blockMastery == targetBlockMastery)
                {
                    // destory the block game object
                    GameObject.Destroy(jengaBlock.gameObject);

                    // remove from list of blocks
                    blocks.RemoveAt(i);

                    continue;
                }

                // enable isKinematic
                jengaBlock.blockRigidbody.isKinematic = false;
            }
            else
            {
                Debug.LogWarning("Could not find JengaBlock component on blocks list");
            }
        }
    }
}
