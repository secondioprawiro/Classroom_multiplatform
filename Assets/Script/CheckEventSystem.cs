using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckEventSystem : MonoBehaviour
{
    void Start()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        Debug.Log("Jumlah EventSystem: " + eventSystems.Length);
        foreach (var es in eventSystems)
        {
            Debug.Log("EventSystem ditemukan di: " + es.gameObject.name);
        }
    }
}