using System.Collections;
using UnityEngine;
using FishNet.Object;
using UnityEngine.SceneManagement; // HANYA ini untuk SceneManager

public class CameraOwner : NetworkBehaviour
{
    [SerializeField] private string cameraTag;
    private Scene currentScene;

    private bool cameraDisabled = false;

    private void Start()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (IsOwner && currentScene.name == "Kelas A" && !cameraDisabled)
        {
            GameObject cameraObject = GameObject.FindWithTag(cameraTag);
            if (cameraObject != null)
            {
                cameraObject.SetActive(false);
                cameraDisabled = true; // supaya hanya dipanggil sekali
            }
        }
    }
}

