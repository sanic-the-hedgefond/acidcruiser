using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoLooper : MonoBehaviour
{
    UnityEngine.Video.VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.loopPointReached += Loop;
    }

    void Loop(UnityEngine.Video.VideoPlayer vp)
    {
        //Debug.Log("Loop");
        videoPlayer.Play();
    }
}