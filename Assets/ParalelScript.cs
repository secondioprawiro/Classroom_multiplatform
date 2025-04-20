using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ParalelScript : MonoBehaviour
{
    public GameObject spotlight;   // First spotlight reference
    public GameObject spotlight2;  // Second spotlight reference
    public GameObject gameObject3; // Battery reference

    public XRSocketInteractor firstLampSocket; // Reference for the first lamp's socket
    public XRSocketInteractor secondLampSocket; // Reference for the second lamp's socket
    public XRSocketInteractor batterySocket; // Reference to the battery's socket

    private bool isFirstLampSocketed; // Check if the first lamp is in its correct socket
    private bool isSecondLampSocketed; // Check if the second lamp is in its correct socket
    private bool isBatterySocketed; // Check if the battery is socketed

    void Awake()
    {
        // Initialize both spotlights to be off
        if (spotlight != null)
        {
            spotlight.SetActive(false);
        }

        if (spotlight2 != null)
        {
            spotlight2.SetActive(false);
        }

        // Add listeners for the first lamp socket
        if (firstLampSocket != null)
        {
            firstLampSocket.selectEntered.AddListener(OnFirstLampSocketEnter);
            firstLampSocket.selectExited.AddListener(OnFirstLampSocketExit);
        }

        // Add listeners for the second lamp socket
        if (secondLampSocket != null)
        {
            secondLampSocket.selectEntered.AddListener(OnSecondLampSocketEnter);
            secondLampSocket.selectExited.AddListener(OnSecondLampSocketExit);
        }

        // Add listeners for the battery socket
        if (batterySocket != null)
        {
            batterySocket.selectEntered.AddListener(OnBatterySocketEnter);
            batterySocket.selectExited.AddListener(OnBatterySocketExit);
        }
    }

    void OnDestroy()
    {
        // Remove listeners for the first lamp socket
        if (firstLampSocket != null)
        {
            firstLampSocket.selectEntered.RemoveListener(OnFirstLampSocketEnter);
            firstLampSocket.selectExited.RemoveListener(OnFirstLampSocketExit);
        }

        // Remove listeners for the second lamp socket
        if (secondLampSocket != null)
        {
            secondLampSocket.selectEntered.RemoveListener(OnSecondLampSocketEnter);
            secondLampSocket.selectExited.RemoveListener(OnSecondLampSocketExit);
        }

        // Remove listeners for the battery socket
        if (batterySocket != null)
        {
            batterySocket.selectEntered.RemoveListener(OnBatterySocketEnter);
            batterySocket.selectExited.RemoveListener(OnBatterySocketExit);
        }
    }

    private void OnFirstLampSocketEnter(SelectEnterEventArgs args)
    {
        isFirstLampSocketed = true;  // Mark the first lamp as socketed
        UpdateSpotlights();
    }

    private void OnFirstLampSocketExit(SelectExitEventArgs args)
    {
        isFirstLampSocketed = false;  // Mark the first lamp as unsocketed
        UpdateSpotlights();
    }

    private void OnSecondLampSocketEnter(SelectEnterEventArgs args)
    {
        isSecondLampSocketed = true;  // Mark the second lamp as socketed
        UpdateSpotlights();
    }

    private void OnSecondLampSocketExit(SelectExitEventArgs args)
    {
        isSecondLampSocketed = false;  // Mark the second lamp as unsocketed
        UpdateSpotlights();
    }

    private void OnBatterySocketEnter(SelectEnterEventArgs args)
    {
        isBatterySocketed = true;  // Mark the battery as socketed
        UpdateSpotlights();
    }

    private void OnBatterySocketExit(SelectExitEventArgs args)
    {
        isBatterySocketed = false;  // Mark the battery as unsocketed
        UpdateSpotlights();
    }

    private void UpdateSpotlights()
    {
        // Parallel circuit logic: Each lamp works independently if the battery is connected
        if (isBatterySocketed)
        {
            // Check first lamp's status
            if (isFirstLampSocketed && spotlight != null)
            {
                spotlight.SetActive(true);
            }
            else if (spotlight != null)
            {
                spotlight.SetActive(false);
            }

            // Check second lamp's status
            if (isSecondLampSocketed && spotlight2 != null)
            {
                spotlight2.SetActive(true);
            }
            else if (spotlight2 != null)
            {
                spotlight2.SetActive(false);
            }
        }
        else
        {
            // If the battery is not connected, turn off both lamps
            if (spotlight != null)
            {
                spotlight.SetActive(false);
            }

            if (spotlight2 != null)
            {
                spotlight2.SetActive(false);
            }
        }
    }
}
