using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using my;
using UnityEngine.Video;
using System.IO;


public class Starter : MonoBehaviour {

    private Dropdown dd;
    private VideoMapper vm;
    private GameObject managerObj;
    private SerialPortManager spm;

	void Start ()
    {
        dd = GameObject.FindGameObjectWithTag("video_selection").GetComponent<Dropdown>();
        managerObj = GameObject.FindGameObjectWithTag("resource_manager");
        vm = managerObj.GetComponent<ResourceManager>().vm;
        spm = managerObj.GetComponent<SerialPortManager>();
        //添加处理事件
        GetComponent<Button>().onClick.AddListener(handler);
	}

    void handler()
    {
        

         if(!spm.isConnected())
         {
             MessageBox.simpleTip("请先连接手柄！", "提示");
             return;
         }

        //一切设备就绪之后，执行初始化工作

        //获取数据
        Screen.fullScreen = true;
        string key = dd.options[dd.value].text;
        VideoItem vi = vm.getMp()[key];

        //视频播放器注入数据，准备播放
        GameObject.FindGameObjectWithTag("player1")
            .GetComponent<UGUIPlayer>().ready(vi.v1.path);
        GameObject.FindGameObjectWithTag("player2")
            .GetComponent<UGUIPlayer>().ready(vi.v2.path);

        //注入视频基本信息
        GameObject gObj = GameObject.FindGameObjectWithTag("head_img");//头像
        Image img= gObj.GetComponent<Image>();
        img.sprite = accquireSprite(vi.imgPath, gObj.GetComponent<RectTransform>().rect);
        img.color = Color.white;
        GameObject.Find("name_txt").GetComponent<Text>().text = vi.userName;//用户名
        GameObject.Find("id_txt").GetComponent<Text>().text = vi.userId;//id

        //路径播放器设置播放源，处于准备状态
        GameObject.FindGameObjectWithTag("path_player").GetComponent<PathController>().ready(vi.opPath);
        //设置时间长度
        GameObject.FindGameObjectWithTag("resource_manager").GetComponent<Adjuster>().pe.allTime = vi.allTime;

        //管理对象，调节2视频播放器和路径播放器之间的步伐
         managerObj.GetComponent<Adjuster>().adjust();
        GameObject.Find("process_info").GetComponent<ProcessShower>().run();
    }


    private Sprite accquireSprite(string imgPath,Rect r)
    {
        FileStream stream = new FileStream(imgPath,FileMode.Open,FileAccess.Read);
        stream.Seek(0, SeekOrigin.Begin);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, (int)stream.Length);
        stream.Close();
        stream.Dispose();
        Texture2D t2d = new Texture2D((int)r.width, (int)r.height);
        t2d.LoadImage(data);
        return Sprite.Create(t2d, new Rect(0, 0, r.width, r.height), new Vector2(0.5f, 0.5f));
    }
}
