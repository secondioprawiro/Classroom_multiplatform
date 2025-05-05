using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Managing.Scened;

public class SceneRequestManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        string deviceModel = SystemInfo.deviceModel;
        bool isVR = deviceModel.Contains("Pico");

        if (!InstanceFinder.IsServerStarted)
            return;


        if (isVR)
        {
            //LoadScene(, "Kelas A");
            //UnloadScene("StarterScene");
        }
        else
        {
            //LoadScene("SceneNonVR");
            //UnloadScene("StarterScene");
        }
    }
    private void LoadScene(NetworkObject nob, string sceneName)
    {
        if (!nob.Owner.IsActive)
            return;

        SceneLookupData lookUp = new SceneLookupData(_stackedSceneHandle, sceneName);
        SceneLoadData sld = new SceneLoadData(lookUp);
        sld.Options.DisallowStacking = false;

        sld.MovedNetworkObjects = new NetworkObject[] { nob };
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadConnectionScenes(nob.Owner, sld);
    }

    public bool SceneStack = false;
    private int _stackedSceneHandle = 0;
    
    private void Start()
    {
        InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
    }

    private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs obj)
    {
        if (!obj.QueueData.AsServer)
            return;
        if (!SceneStack)
            return;
        if (_stackedSceneHandle != 0)
            return;
        if (obj.LoadedScenes.Length > 0)
            _stackedSceneHandle = obj.LoadedScenes[0].handle;
    }
}
