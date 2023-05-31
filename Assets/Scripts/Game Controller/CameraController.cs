using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera sceneCamera;
    public JengaStackSpawner stackSpawner;

    // reference to object we are focused on
    protected Transform focusedObject = null;
    private bool movingToFocus = false;
    private Vector3 focusObjectOffset = new Vector3(1.5f, 7, 2.5f);
    // offset of camera to any of the focused objects
    private Vector3 cameraOffset = new Vector3(0, 0, -19);
    private Vector3 previousPosition;
    private Vector3 cameraTargetPosition;
    private int focusedIndex = 1;

    public void OnEnable()
    {
        HUD.OnHUDButtonPress += OnHudButtonPress;
        JengaStackSpawner.OnStackSpawningComplete += OnStacksSpawned;
    }

    public void OnDisable()
    {
        HUD.OnHUDButtonPress -= OnHudButtonPress;
        JengaStackSpawner.OnStackSpawningComplete -= OnStacksSpawned;
    }

    public void OnHudButtonPress(HudButtonType hudButtonType)
    {
        switch (hudButtonType)
        {
            case HudButtonType.LeftStackNavigation:
                FocusOnStack(false);
                break;
            case HudButtonType.RightStackNavigation:
                FocusOnStack(true);
                break;
        }
    }

    private void OnStacksSpawned()
    {
        if (focusedObject == null) 
        {
            focusedObject = stackSpawner.stackTransforms[focusedIndex];
        }
    }

    public void FocusOnStack(bool increaseStackIndex)
    {
        int targetIndex = LimitMaxIndex(increaseStackIndex);
        if (targetIndex == focusedIndex)
            return;
        else
            focusedIndex = targetIndex;

        focusedObject = stackSpawner.stackTransforms[focusedIndex];

        int xPos = GetXPos();
        cameraTargetPosition = new Vector3(xPos, sceneCamera.transform.position.y, sceneCamera.transform.position.z);
        StartCoroutine(LerpPosition(cameraTargetPosition, 1));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        movingToFocus = true;
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        movingToFocus = false;
        transform.position = targetPosition;
    }

    private int GetXPos()
    {
        if (focusedIndex == 0)
            return -10;
        else if (focusedIndex == 1)
            return 0;
        else if (focusedIndex == 2)
            return 10;

        else
        {
            Debug.LogWarning("Unhandled focus index");
            return 0;
        } 
    }

    private int LimitMaxIndex(bool increaseStackIndex)
    {
        if (focusedIndex == 0 && !increaseStackIndex)
            return 0;
        else if (focusedIndex == 2 && increaseStackIndex)
            return 2;

        else return increaseStackIndex ? focusedIndex + 1 : focusedIndex - 1;
    }

    public void LateUpdate()
    {
        if (movingToFocus || focusedObject == null)
        {
            return;
        }

        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        // store previous position on mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = sceneCamera.ScreenToViewportPoint(Input.mousePosition);
        }

        // handle drag
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = previousPosition - sceneCamera.ScreenToViewportPoint(Input.mousePosition);

            sceneCamera.transform.position = focusedObject.position + focusObjectOffset;

            sceneCamera.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            sceneCamera.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
            sceneCamera.transform.Translate(cameraOffset);

            previousPosition = sceneCamera.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}
