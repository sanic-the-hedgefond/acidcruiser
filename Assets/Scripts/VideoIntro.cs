using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoIntro : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public VideoPlayer videoLoop;
    public GameObject menuUI;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += PlayVideoLoopAndMusic;
        videoLoop.Prepare();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayVideoLoopAndMusic(UnityEngine.Video.VideoPlayer vp)
    {
        videoLoop.Play();
        FindObjectOfType<GameManager>().PlayMusic();
        menuUI.SetActive(true);
    }
}
