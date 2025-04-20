using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideoOnButtonClick : MonoBehaviour
{
    public Button playButton;           // Assign the Button component in the Inspector
    public GameObject canvas;           // Assign the Canvas object in the Inspector
    public VideoPlayer videoPlayer;     // Assign the VideoPlayer component in the Inspector

    void Start()
    {
        // Add a listener to the button to trigger the PlayVideo method
        playButton.onClick.AddListener(PlayVideo);
        
        // Ensure the canvas is initially off
        canvas.SetActive(false);
        
        // Subscribe to the event that triggers when the video finishes playing
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void PlayVideo()
    {
        // Enable the canvas and play the video
        canvas.SetActive(true);
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Check if the video has really stopped
        if (!videoPlayer.isPlaying)
        {
            // Disable the canvas once the video has finished playing
            canvas.SetActive(false);
        }
    }
}