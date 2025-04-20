using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpotlightManager : MonoBehaviour
{
    public GameObject spotlight;   // First spotlight reference
    public GameObject spotlight2;  // Second spotlight reference
    public GameObject gameObject3; // Battery reference
    public GameObject lampu1;
    public GameObject lampu2;
    public GameObject listrikAwal;
    public GameObject listrikAkhir;

    public XRSocketInteractor firstLampSocket; // Reference for the first lamp's socket
    public XRSocketInteractor secondLampSocket; // Reference for the second lamp's socket
    public XRSocketInteractor batterySocket; // Reference to the battery's socket

    private bool isFirstLampSocketed; // Check if the first lamp is in its correct socket
    private bool isSecondLampSocketed; // Check if the second lamp is in its correct socket
    private bool isBatterySocketed; // Check if the battery is socketed

    public Material lampuTerang;
    public Material lampuMati;
    
    public AudioSource audioSource; // AudioSource for the spotlight sound
    public AudioClip lightSound; // Looping sound when lights are on

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

        // Ensure the AudioSource is set up correctly
        if (audioSource != null)
        {
            audioSource.clip = lightSound;
            audioSource.loop = true;
            audioSource.volume = 0.5f;
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
        listrikAwal.SetActive(true);
        UpdateSpotlights();
    }

    private void OnBatterySocketExit(SelectExitEventArgs args)
    {
        isBatterySocketed = false;  // Mark the battery as unsocketed
        listrikAwal.SetActive(false);
        listrikAkhir.SetActive(false);
        UpdateSpotlights();
    }

    private void UpdateSpotlights()
    {
        // Series circuit logic: Both lamps and battery must be socketed for the lights to turn on
        if (isFirstLampSocketed && isSecondLampSocketed && isBatterySocketed)
        {
            // Turn on both lamps
            if (spotlight != null)
            {
                spotlight.SetActive(true);
                ChangeLampMaterial(lampu1);
            }

            if (spotlight2 != null)
            {
                spotlight2.SetActive(true);
                ChangeLampMaterial(lampu2);
            }

            // Play audio if both spotlights are on
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.volume = 1.0f; // Full volume when both are on
                audioSource.Play();
            }
            listrikAkhir.SetActive(true);
        }
        else
        {
            // Turn off both lamps if any component is missing
            if (spotlight != null)
            {
                spotlight.SetActive(false);
                ChangeLampMati(lampu1);
                listrikAkhir.SetActive(false);
            }

            if (spotlight2 != null)
            {
                spotlight2.SetActive(false);
                ChangeLampMati(lampu2);
                listrikAkhir.SetActive(false);
            }

            // Stop audio if one or both spotlights are off
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void ChangeLampMaterial(GameObject lampObject)
    {
        // Find the child object Bulb -> Top
        Transform bulbTransform = lampObject.transform.Find("Bulb/Top");
        if (bulbTransform != null)
        {
            MeshRenderer meshRenderer = bulbTransform.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                // Copy the material array so we can modify it
                Material[] materials = meshRenderer.materials;
                // Change the material at index 2 to lampuTerang
                materials[0] = lampuTerang;
                // Reassign the modified materials array back to the renderer
                meshRenderer.materials = materials;
            }
        }
        else
        {
            Debug.LogWarning("No 'Top' child object found in " + lampObject.name);
        }
    }

    private void ChangeLampMati(GameObject lampObject)
    {
        // Find the child object Bulb -> Top
        Transform bulbTransform = lampObject.transform.Find("Bulb/Top");
        if (bulbTransform != null)
        {
            MeshRenderer meshRenderer = bulbTransform.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                // Copy the material array so we can modify it
                Material[] materials = meshRenderer.materials;
                // Change the material at index 2 to lampuTerang
                materials[0] = lampuMati;
                // Reassign the modified materials array back to the renderer
                meshRenderer.materials = materials;
            }
        }
        else
        {
            Debug.LogWarning("No 'Top' child object found in " + lampObject.name);
        }
    }
}
