using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SelectionController;

public enum HudButtonType
{
    BreakGlass,
    ResetStacks,
    LeftStackNavigation,
    RightStackNavigation
}

public class HUD : MonoBehaviour
{
    public GameObject rightHUDContainer;
    public Button breakGlassButton;
    public Button resetStacksButton;
    public GameObject bottomHUDContainer;
    public Button leftArrowButton;
    public Button rightArrowButton;

    public GameObject selectedBlockPanelPrefab;
    public Transform selectedBlockPanelTransform;
    private SelectedBlockPanel selectedBlockPanel;

    public delegate void HUDButtonPressed(HudButtonType buttonType);
    public static event HUDButtonPressed OnHUDButtonPress;

    public void Awake()
    {
        breakGlassButton.onClick.AddListener(OnBreakGlassButtonClick);
        resetStacksButton.onClick.AddListener(OnResetStacksButtonClick);
        leftArrowButton.onClick.AddListener(OnLeftArrowClick);
        rightArrowButton.onClick.AddListener(OnRightArrowClick);
    }

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
        // check if UI is already displaying the newly selected block
        if (selectedBlockPanel != null)
        {
            // Selected the same block currently displaying in the UI
            if (selectedBlockPanel.CurrentBlockData == selectedJengaBlock.BlockData)
            {
                return;
            }
        }

        UpdatedSelectedBlockPanel(selectedJengaBlock.BlockData);
    }

    public void UpdatedSelectedBlockPanel(BlockData blockData)
    {
        // assign block panel if it has not been done
        if (selectedBlockPanel == null)
        {
            GameObject gameObj = Instantiate(selectedBlockPanelPrefab, selectedBlockPanelTransform);
            selectedBlockPanel = gameObj.GetComponent<SelectedBlockPanel>();
        }

        selectedBlockPanel.Init(blockData);
    }

    public void OnBreakGlassButtonClick()
    {
        if (OnHUDButtonPress != null)
        {
            OnHUDButtonPress(HudButtonType.BreakGlass);
        }
    }

    public void OnResetStacksButtonClick()
    {
        if (OnHUDButtonPress != null)
        {
            OnHUDButtonPress(HudButtonType.ResetStacks);
        }

        if (selectedBlockPanel != null)
        {
            selectedBlockPanel.HideUI();
        }
    }

    public void OnLeftArrowClick()
    {
        if (OnHUDButtonPress != null)
        {
            OnHUDButtonPress(HudButtonType.LeftStackNavigation);
        }
    }

    public void OnRightArrowClick()
    {
        if (OnHUDButtonPress != null)
        {
            OnHUDButtonPress(HudButtonType.RightStackNavigation);
        }
    }
}
