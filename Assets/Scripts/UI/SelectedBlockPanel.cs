using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedBlockPanel : MonoBehaviour
{
    public Button closeButton;
    public TextMeshProUGUI header;
    public TextMeshProUGUI gradeLevelLabel;
    public TextMeshProUGUI domainLabel;
    public TextMeshProUGUI clusterLabel;
    public TextMeshProUGUI standardIDLabel;
    public TextMeshProUGUI standardDescriptionLabel;

    //[Grade level]: [Domain]
    //[Cluster]
    //[Standard ID]: [Standard Description]
    protected BlockData currentBlockData;
    public BlockData CurrentBlockData { get { return currentBlockData; } }

    public void Awake()
    {
        closeButton.onClick.AddListener(OnCloseClick);
    }

    public void Init(BlockData blockData)
    {
        if (blockData == null)
        {
            Debug.LogWarning("Block Data being passed to panel is null.");
            return;
        }

        // assign local reference
        currentBlockData = blockData;

        // set block data text fields
        gradeLevelLabel.text = blockData.gradeLevel;
        domainLabel.text = blockData.blockDomain;
        clusterLabel.text = blockData.blockCluster;
        standardIDLabel.text = blockData.standardID;
        standardDescriptionLabel.text = blockData.blockStandardDescription;

        this.gameObject.SetActive(true);
    }

    public void ResetText()
    {
        gradeLevelLabel.text = "";
        domainLabel.text = "";
        clusterLabel.text = "";
        standardIDLabel.text = "";
        standardDescriptionLabel.text = "";
        currentBlockData = null;
    }

    public void OnCloseClick()
    {
        HideUI();
    }

    public void HideUI()
    {
        this.gameObject.SetActive(false);
        ResetText();
    }
}
