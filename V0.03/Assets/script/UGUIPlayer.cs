using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;
using my;

public class UGUIPlayer : MonoBehaviour
{

    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    private Button startBtn;

    void Start()
    {
        videoPlayer = this.GetComponent<VideoPlayer>();
        videoPlayer.playbackSpeed = 1.23f;
        videoPlayer.isLooping = false;
        rawImage = this.GetComponent<RawImage>();
        videoPlayer.Pause();
    }
    
    void Update()
    {
        if (videoPlayer.texture == null)
        {
        }
        else
        {
            rawImage.texture = videoPlayer.texture;
            //speed();
        }
    }


    private void speed()
    {
        videoPlayer.frame += Config.FRAME_PERA_SPEED;
    }

    public void ready(string url)
    {
        //videoPlayer.Stop();
        videoPlayer.frame = 0;
        videoPlayer.url = url;
        rawImage.color = Color.white;
    }

    //控制方法
    public void pause()
    {
        videoPlayer.Pause();
    }

    public void play()
    {
        videoPlayer.Play();
    }
}