using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using System;
using FishNet.Managing.Scened;

public class BootstrapManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        string deviceModel = SystemInfo.deviceModel;
        bool isVR = deviceModel.Contains("Pico");

        if (InstanceFinder.IsServer)
            return;


        if (isVR)
        {
            LoadScene("Kelas A");
            UnloadScene("StarterScene");
        }
        else
        {
            LoadScene("SceneNonVR");
            UnloadScene("StarterScene");
        }
    }

    void LoadScene(String sceneName)
    {
        if (!InstanceFinder.IsServer)
            return;

        SceneLoadData sld = new SceneLoadData();
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    void UnloadScene(String sceneName)
    {
        if (!InstanceFinder.IsServer)
            return;

        SceneUnloadData sld = new SceneUnloadData();
        InstanceFinder.SceneManager.UnloadGlobalScenes(sld);
    } 
}
