using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    public float scaleMultiplier = 1.2f; // Scale size when hovered
    public float scaleSpeed = 0.2f; // How fast the scaling happens
    
    private Vector3 originalScale;
    private Coroutine scaleCoroutine;

    public GameObject TargetUI;
    public GameObject Self;
    
    private void Awake()
    {
        // Save the original scale of the UI element
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.activeSelf)
        {
            // Calculate the target scale based on the multiplier
            Vector3 targetScale = originalScale * scaleMultiplier;

            // Start scaling up to the hover size
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine);
            }
            scaleCoroutine = StartCoroutine(ScaleTo(targetScale));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.activeSelf)
        {
            // Return to the original size
            if (scaleCoroutine != null)
            {
                StopCoroutine(scaleCoroutine);
            }
            scaleCoroutine = StartCoroutine(ScaleTo(originalScale));
        }
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startingScale = transform.localScale;

        while (elapsedTime < scaleSpeed)
        {
            transform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / scaleSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void ToggleMenu()
    {
        if (TargetUI != null)
        {
            // Toggle the target UI visibility
            TargetUI.SetActive(!TargetUI.activeSelf);
        }

        if (Self != null)
        {
            // Hide the current UI element
            Self.SetActive(false);
        }
    }
}
