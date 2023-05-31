using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour
{
    public LayerMask blocksMask;
    protected GameObject buttonDownObject;
    protected Camera selectionCamera;

    protected float lastButtonDownTime;
    protected bool isValidSelection;
    public static SelectionController instance;

    public delegate void BlockSelectionChanged(JengaBlock selectedJengaBlock);
    public static event BlockSelectionChanged OnBlockSelectionChanged;

    public SelectionController()
    {
        // add events?
    }

    public void Awake()
    {
        selectionCamera = GetComponent<Camera>();
        instance = this;
    }

    public void Update()
    {
        HandleMouseSelection();
    }

    public void HandleMouseSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isValidSelection = false;

            if (IsMouseOverUI())
            {
                // ignore mouse handling selection when mouse is over UI
                return;
            }

            isValidSelection = true;
            OnMouseButtonDown();
        }

        else if (isValidSelection && Input.GetMouseButtonUp(0))
        {
            if (IsMouseOverUI())
            {
                // ignore mouse handling selection when mouse is over UI
                return;
            }

            OnMouseButtonUp();
        }
    }

    public bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void OnMouseButtonDown()
    {
        Vector3 inputPosition = Input.mousePosition;
        RaycastHit hit;

        if (Physics.Raycast(selectionCamera.ScreenPointToRay(inputPosition), out hit, 1000f, blocksMask))
        {
            buttonDownObject = hit.collider.gameObject;
        }
        else
        {
            buttonDownObject = null;
        }

        lastButtonDownTime = Time.time;
    }

    public void OnMouseButtonUp()
    {
        // if time held was longer than a second ignore selection
        if (Time.time - lastButtonDownTime > 1f)
        {
            Debug.Log("Ignoring extended selection time");
            return;
        }

        Vector3 inputPosition = Input.mousePosition;
        RaycastHit hit;

        if (Physics.Raycast(selectionCamera.ScreenPointToRay(inputPosition), out hit, 1000f, blocksMask))
        {
            GameObject buttonUpObject = hit.collider.gameObject;

            // button down obj was null on button up
            if (buttonDownObject == null)
            {
                return;
            }
            // button down obj was different on button up
            else if (buttonDownObject != null && buttonDownObject != buttonUpObject)
            {
                return;
            }

            // handle selection
            JengaBlock selectedBlock = buttonUpObject.GetComponent<JengaBlock>();
            if (selectedBlock != null)
            {
                if (OnBlockSelectionChanged != null)
                {
                    OnBlockSelectionChanged(selectedBlock);
                }
            }
        }
        else
        {
            buttonDownObject = null;
        }
    }

    public void OnDestroy()
    {
        instance = null;
    }
}

