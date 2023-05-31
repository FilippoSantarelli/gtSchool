using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaBlock : MonoBehaviour
{
    public Renderer blockRenderer;
    public Rigidbody blockRigidbody;
    public Material[] materials;
    public Outline outlineComponent;

    protected BlockData blockData;
    public BlockData BlockData { get { return blockData; } }
    protected bool isSelected = false;

    public void OnEnable()
    {
        SelectionController.OnBlockSelectionChanged += OnBlockSelection;
    }

    public void OnDisable()
    {
        SelectionController.OnBlockSelectionChanged -= OnBlockSelection;
    }

    private void OnBlockSelection(JengaBlock selectedJengaBlock)
    {
        // enable outline if selected block matches and outline is disabled
        if (selectedJengaBlock.blockData.uniqueID == blockData.uniqueID)
        {
            if (!outlineComponent.enabled)
                outlineComponent.enabled = true;
        }
        // disable outline if it is enabled
        else if (outlineComponent.enabled)
        {
            outlineComponent.enabled = false;
        }
    }

    public void Init(BlockData blockData)
    {
        this.blockData = blockData;

        SetBlockMaterial((BlockMastery)blockData.blockMastery);
    }

    public void SetBlockMaterial(BlockMastery blockMastery)
    {
        switch (blockMastery)
        {
            case BlockMastery.Glass:
                blockRenderer.material = materials[0];
                break;
            case BlockMastery.Wood:
                blockRenderer.material = materials[1];
                break;
            case BlockMastery.Stone:
                blockRenderer.material = materials[2];
                break;

            // set default as wood
            default:
                blockRenderer.material = materials[1];
                Debug.LogWarning($"Unmapped block mastery {blockMastery}, for {blockData.uniqueID}.");
                break;
        }

        // enable renderer
        blockRenderer.enabled = true;
    }

    //public void OnDestroy()
    //{
        //Destroy the instance
        //Destroy(blockRenderer.material);
        //Output the amount of materials to show if the instance was deleted
        //print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
    //}
}
