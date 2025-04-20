using UnityEngine;
using UnityEngine.UI;

public class ResetObjectsManager : MonoBehaviour
{
    public Button resetButton;        // Button to trigger the reset
    public GameObject[] objectsToReset; // The objects to reset
    public Transform respawnPoint;    // Respawn point for all objects

    void Start()
    {
        if (resetButton != null)
        {
            // Attach the ResetObjects method to the button's OnClick event
            resetButton.onClick.AddListener(ResetObjects);
        }
        else
        {
            Debug.LogError("Reset Button is not assigned!");
        }

        if (respawnPoint == null)
        {
            Debug.LogError("Respawn point is not assigned!");
        }

        if (objectsToReset.Length == 0)
        {
            Debug.LogError("No objects assigned to reset!");
        }
    }

    public void ResetObjects()
    {
        foreach (GameObject obj in objectsToReset)
        {
            if (obj != null && respawnPoint != null)
            {
                // Reset position and rotation
                obj.transform.position = respawnPoint.position;
                obj.transform.rotation = respawnPoint.rotation;

                // Reset Rigidbody if the object has one
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }
        Debug.Log("Objects have been reset!");
    }
}
