using FishNet.Connection;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;
using TMPro;
using FishNet;
using FishNet.Managing.Scened;

public class CharacterSelectionNonVR : NetworkBehaviour
{
    [SerializeField] private List<GameObject> character = new List<GameObject>(); // List for boy and girl characters
    [SerializeField] private GameObject characterSelectorPanel; // Panel to select character
    [SerializeField] private GameObject canvasObject; // Canvas containing character selection
    [SerializeField] private string cameraTag;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            canvasObject.SetActive(true);
        }
        else
        {
            canvasObject.SetActive(false); // Pastikan canvas non-owner langsung nonaktif
        }
    }

    public void SpawnBoy()
    {
        characterSelectorPanel.SetActive(false);
        SpawnCharacter(0);

    }
    public void SpawnGirl()
    {
        characterSelectorPanel.SetActive(false);
        SpawnCharacter(1);
    }
    private void SpawnCharacter(int spawnIndex)
    {
        if (IsOwner)
        {
            SpawnRequest(spawnIndex, Owner);

            GameObject cameraObject = GameObject.FindWithTag(cameraTag);
            if (cameraObject != null)
            {
                cameraObject.SetActive(false);
            }

            DisableCanvasOnAllClients(); // Panggil fungsi baru ini
        }
    }

    [ObserversRpc]
    private void DisableCanvasOnAllClients()
    {
        if (canvasObject != null)
        {
            canvasObject.SetActive(false);
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnRequest(int spawnIndex, NetworkConnection conn)
    {
        // Validasi prefab dari list karakter
        if (spawnIndex < 0 || spawnIndex >= character.Count || character[spawnIndex] == null)
        {
            Debug.LogError("Invalid spawn index or character prefab is null.");
            return;
        }

        GameObject prefabToSpawn = character[spawnIndex];

        // Spawn karakter
        GameObject playerInstance = Instantiate(
            prefabToSpawn,
            SpawnPoint.instance.transform.position,
            quaternion.identity
        );
        InstanceFinder.ServerManager.Spawn(playerInstance, conn);
    }
}
