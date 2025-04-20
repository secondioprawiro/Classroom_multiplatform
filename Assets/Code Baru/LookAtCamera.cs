using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera targetCamera; // Kamera yang digunakan player

    void Start()
    {
        // Jika kamera tidak diatur, gunakan kamera utama
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (targetCamera != null)
        {
            // Mengatur rotasi objek agar selalu menghadap kamera
            Vector3 direction = targetCamera.transform.position - transform.position;
            direction.y = 0; // Tetap horizontal, tidak memperhatikan sumbu Y
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}
