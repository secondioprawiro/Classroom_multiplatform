using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoveringui : MonoBehaviour
{
    public float speed = 1.0f;    // Speed of the hovering motion
    public float amplitude = 10.0f; // How far up and down the object moves
    private RectTransform rectTransform;
    private Vector3 initialPosition;

    void Start()
    {
        // Get the RectTransform component
        rectTransform = GetComponent<RectTransform>();
        // Store the initial position
        initialPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave for smooth hovering
        float newY = initialPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
        
        // Apply the new position to the RectTransform
        rectTransform.anchoredPosition = new Vector2(initialPosition.x, newY);
    }
}
