using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class cobaparalel : MonoBehaviour
{
    public GameObject bulb1Spotlight;  // Spotlight for Bulb1
    public GameObject bulb2Spotlight;  // Spotlight for Bulb2

    public GameObject bulb1;           // Bulb1 reference
    public GameObject bulb2;           // Bulb2 reference

    public GameObject listrikAwal1;
    public GameObject listrikAwal2;
    public GameObject listrikAkhirFull;
    public GameObject listrikAkhirHalf;
    public GameObject listrikAtas;
    public GameObject listrikBawah;

    public XRSocketInteractor batteryslot;  // Socket for the battery
    public XRSocketInteractor bulbsocket1;  // Socket 1 for the bulbs
    public XRSocketInteractor bulbsocket2;  // Socket 2 for the bulbs

    private bool isBatteryAttached = false;  // Check if the battery is attached
    private GameObject bulbInSocket1 = null; // Tracks the actual bulb in BulbSocket1
    private GameObject bulbInSocket2 = null; // Tracks the actual bulb in BulbSocket2

    public Material lampuTerang;
    public Material lampuMati;

    public AudioSource audioSource;    // The single AudioSource used for the sound
    public AudioClip bulbSound;        // The looped sound for the bulbs

    void Awake()
    {
        // Initialize the spotlights to be off
        bulb1Spotlight.SetActive(false);
        bulb2Spotlight.SetActive(false);

        // Add listeners for the battery and bulbs
        batteryslot.selectEntered.AddListener(OnBatteryAttached);
        batteryslot.selectExited.AddListener(OnBatteryDetached);

        bulbsocket1.selectEntered.AddListener(OnBulbSocket1Attach);
        bulbsocket1.selectExited.AddListener(OnBulbSocket1Detach);

        bulbsocket2.selectEntered.AddListener(OnBulbSocket2Attach);
        bulbsocket2.selectExited.AddListener(OnBulbSocket2Detach);

        // Initialize audio source
        audioSource.clip = bulbSound;
        audioSource.loop = true;
        audioSource.volume = 0;  // Start with no volume
        audioSource.Play();      // Start playing the looped sound
    }

    void OnDestroy()
    {
        // Remove listeners
        batteryslot.selectEntered.RemoveListener(OnBatteryAttached);
        batteryslot.selectExited.RemoveListener(OnBatteryDetached);

        bulbsocket1.selectEntered.RemoveListener(OnBulbSocket1Attach);
        bulbsocket1.selectExited.RemoveListener(OnBulbSocket1Detach);

        bulbsocket2.selectEntered.RemoveListener(OnBulbSocket2Attach);
        bulbsocket2.selectExited.RemoveListener(OnBulbSocket2Detach);
    }

    // Battery attach and detach events
    private void OnBatteryAttached(SelectEnterEventArgs args)
    {
        isBatteryAttached = true;
        UpdateSpotlights();
        listrikAwal1.SetActive(true);
        listrikAwal2.SetActive(true);
    }

    private void OnBatteryDetached(SelectExitEventArgs args)
    {
        isBatteryAttached = false;
        UpdateSpotlights();
        listrikAwal1.SetActive(false);
        listrikAwal2.SetActive(false);
        listrikAkhirFull.SetActive(false);
        listrikAkhirHalf.SetActive(false);
        listrikAtas.SetActive(false);
        listrikBawah.SetActive(false);
    }

    // Bulb attach/detach events for BulbSocket1
    private void OnBulbSocket1Attach(SelectEnterEventArgs args)
    {
        var attachedBulb = args.interactableObject.transform.gameObject;

        if (attachedBulb == bulb1 || attachedBulb == bulb2)
        {
            bulbInSocket1 = attachedBulb;
        }

        UpdateSpotlights();
    }

    private void OnBulbSocket1Detach(SelectExitEventArgs args)
    {
        bulbInSocket1 = null;
        UpdateSpotlights();
    }

    // Bulb attach/detach events for BulbSocket2
    private void OnBulbSocket2Attach(SelectEnterEventArgs args)
    {
        var attachedBulb = args.interactableObject.transform.gameObject;

        if (attachedBulb == bulb1 || attachedBulb == bulb2)
        {
            bulbInSocket2 = attachedBulb;
        }

        UpdateSpotlights();
    }

    private void OnBulbSocket2Detach(SelectExitEventArgs args)
    {
        bulbInSocket2 = null;
        UpdateSpotlights();
    }

    // Update the state of the spotlights and the sound based on the conditions
    private void UpdateSpotlights()
    {
        int activeBulbs = 0;

        // Turn on Bulb1's spotlight if it's in either socket and the battery is attached
        if (isBatteryAttached && (bulbInSocket1 == bulb1 || bulbInSocket2 == bulb1))
        {
            bulb1Spotlight.SetActive(true);
            ChangeLampMaterial(bulb1, true);
            activeBulbs++;
        }
        else
        {
            bulb1Spotlight.SetActive(false);
            ChangeLampMati(bulb1);
        }

        // Turn on Bulb2's spotlight if it's in either socket and the battery is attached
        if (isBatteryAttached && (bulbInSocket1 == bulb2 || bulbInSocket2 == bulb2))
        {
            bulb2Spotlight.SetActive(true);
            ChangeLampMaterial(bulb2, true);
            activeBulbs++;
        }
        else
        {
            bulb2Spotlight.SetActive(false);
            ChangeLampMati(bulb2);
        }
        if (isBatteryAttached && bulbInSocket1 != null) 
        {
            listrikAtas.SetActive(true);
            listrikAkhirFull.SetActive(true);
        }
        if (isBatteryAttached && bulbInSocket1 == null)
        {
            listrikAtas.SetActive(false);
            listrikAkhirFull.SetActive(false);
        }
        if (isBatteryAttached &&  bulbInSocket2 != null) 
        {
            listrikBawah.SetActive(true);
            listrikAkhirHalf.SetActive(true);
        }
        if (isBatteryAttached && bulbInSocket2 == null)
        {
            listrikBawah.SetActive(false);
            listrikAkhirHalf.SetActive(false);
        }
        if (isBatteryAttached && bulbInSocket1 != null && bulbInSocket2 != null)
        {
            listrikAkhirHalf.SetActive(false);
            listrikAkhirFull.SetActive(true);
        }


        // Adjust the audio source volume based on the number of active bulbs
        if (activeBulbs == 1)
        {
            audioSource.volume = 0.5f;  // One bulb active
        }
        else if (activeBulbs == 2)
        {
            audioSource.volume = 1f;    // Both bulbs active
        }
        else
        {
            audioSource.volume = 0f;    // No bulbs active
        }

        // Update the spotlight intensity based on the number of active bulbs
        UpdateSpotlightIntensity(bulb1Spotlight, activeBulbs);
        UpdateSpotlightIntensity(bulb2Spotlight, activeBulbs);
    }

    private void UpdateSpotlightIntensity(GameObject spotlight, int activeBulbs)
    {
        Light light = spotlight.GetComponent<Light>();
        if (light != null)
        {
            // Set intensity based on the number of active bulbs
            light.intensity = activeBulbs == 2 ? 0.12f : 1f;
        }
    }

    private void ChangeLampMaterial(GameObject lampObject, bool isOn)
    {
        // Find the child object Bulb -> Top
        Transform bulbTransform = lampObject.transform.Find("Bulb/Top");
        if (bulbTransform != null)
        {
            MeshRenderer meshRenderer = bulbTransform.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                Material[] materials = meshRenderer.materials;
                materials[0] = isOn ? lampuTerang : lampuMati;
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
        Transform bulbTransform = lampObject.transform.Find("Bulb/Top");
        if (bulbTransform != null)
        {
            MeshRenderer meshRenderer = bulbTransform.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                Material[] materials = meshRenderer.materials;
                materials[0] = lampuMati;
                meshRenderer.materials = materials;
            }
        }
        else
        {
            Debug.LogWarning("No 'Top' child object found in " + lampObject.name);
        }
    }
}