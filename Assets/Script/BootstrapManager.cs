using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using System;
using FishNet.Managing.Scened;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using FishNetSceneManager = FishNet.Managing.Scened.SceneManager;
public class BootstrapManager : MonoBehaviour
{
    private bool sceneLoaded = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        string deviceModel = SystemInfo.deviceModel;
        bool isVR = deviceModel.Contains("Pico");
        Scene currentScene = UnitySceneManager.GetActiveScene();


        if (!InstanceFinder.IsServerStarted || sceneLoaded)
            return;

        if (isVR && currentScene.name == "StarterScene")
        {
            LoadScene("Kelas A");
            sceneLoaded = true;
        }
        else if (!isVR && currentScene.name == "StarterScene")
        {
            LoadScene("SceneNonVR");
            sceneLoaded = true;
        }
    }

    void LoadScene(string sceneName)
    {
        if (!InstanceFinder.IsServerStarted)
            return;

        SceneLoadData sld = new SceneLoadData(sceneName);
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    //void UnloadScene(string sceneName)
    //{
    //    if (!InstanceFinder.IsServerStarted)
    //        return;

    //    SceneUnloadData sld = new SceneUnloadData(sceneName);
    //    InstanceFinder.SceneManager.UnloadGlobalScenes(sld);
    //} 
}
