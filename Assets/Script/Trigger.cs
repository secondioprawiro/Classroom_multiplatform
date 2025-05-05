using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing.Scened;
using UnityEngine.SceneManagement;
using FishNet.Object;
using FishNet.Managing.Logging;

public class Trigger : MonoBehaviour
{
    [Server(Logging = LoggingType.Off)]
    private void OnTriggerEnter(Collider other)
    {
        NetworkObject nob = other.GetComponent<NetworkObject>();
        if (nob != null)
        {
            LoadWObject(nob, "Kelas A");
            Debug.Log("Load Kelas A");
            Debug.Log("Ini network object " + nob);
        }
    }

    private void LoadWObject(NetworkObject nob, string sceneName)
    {
        if (!nob.Owner.IsActive)
            return;

        SceneLoadData sld = new SceneLoadData(sceneName);
        sld.MovedNetworkObjects = new NetworkObject[] { nob };
        sld.ReplaceScenes = ReplaceOption.All;

        // Register event untuk mendeteksi ketika scene selesai dimuat
        InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;

        InstanceFinder.SceneManager.LoadConnectionScenes(nob.Owner, sld);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);

        // Simpan referensi objek yang dipindahkan untuk digunakan nanti
        _lastMovedObject = nob;
    }

    private NetworkObject _lastMovedObject;

    private void OnSceneLoadEnd(SceneLoadEndEventArgs args)
    {
        // Hapus event listener
        InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoadEnd;

        if (_lastMovedObject != null)
        {
            // Cari SpawnPointNonVR di scene baru
            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            if (spawnPoint != null)
            {
                // Pindahkan objek ke posisi spawn point
                _lastMovedObject.transform.position = spawnPoint.transform.position;
                _lastMovedObject.transform.rotation = spawnPoint.transform.rotation;
                Debug.Log("Objek dipindahkan ke spawn point");
            }
            else
            {
                Debug.LogWarning("SpawnPointNonVR tidak ditemukan di scene baru");
            }

            _lastMovedObject = null;
        }
    }
}