using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using my;
using LitJson;
using System.IO;



public class ResourceManager : MonoBehaviour {

    public string dirPath;
    public VideoMapper vm;

    private GameObject mainCanvas;
    private GameObject inCanvas;


    void Awake()
    {
        dirPath = Application.dataPath + "/StreamingAssets/data/";
        vm = new VideoMapper(dirPath);
    }

    void Start()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("main_canvas");
        inCanvas = GameObject.FindGameObjectWithTag("in_canvas");
        mainCanvas.SetActive(true);
        inCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            mainCanvas.SetActive(false);
            inCanvas.SetActive(true);
        }
    }

    //清理资源
    public void cleanResource()
    {
        //关闭串口
        var spm=GetComponent<SerialPortManager>();
        spm.close();
    }

    public void quit()
    {
        cleanResource();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
