using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using my;

//调整2视频播放器+1路径播放器+端口接收数据之间的协调性

public class Adjuster : MonoBehaviour {
    private PathController pc;
    private SerialPortManager spm;
    private UGUIPlayer up1;
    private UGUIPlayer up2;

    private CoordinatePointChange cpc;
    private PointEntry currentPoint;
    private List<PointEntry> currentPointsList;
    private bool isStart=false;
    private PointerContoller pointerContoller;
   
    //进度
    public ProcessEntry pe=new ProcessEntry();

    private Text txtWidget;
    private GameObject txtGameObj;

    void Start ()
    {
        // 路径描绘控制
        pc = GameObject.FindGameObjectWithTag("path_player").GetComponent<PathController>();
        //串口数据控制
        spm = GameObject.FindGameObjectWithTag("resource_manager").GetComponent<SerialPortManager>();
        //视频播放控制
        up1 = GameObject.FindGameObjectWithTag("player1").GetComponent<UGUIPlayer>();
        up2 = GameObject.FindGameObjectWithTag("player2").GetComponent<UGUIPlayer>();
        pointerContoller = GameObject.Find("top_tool").GetComponent<PointerContoller>();
        cpc = new CoordinatePointChange();



        //显示从蓝牙接收的信息
        txtGameObj = GameObject.Find("show_info");
        txtWidget = txtGameObj.GetComponent<Text>();
        txtGameObj.SetActive(false);
        
    }

    void FixedUpdate()
    {

        if (isStart)
        {
            //acquire point
            currentPoint = spm.acquireCurrentPoint();
            //acquire point list
            currentPointsList = pc.acquirePointsList();
            if (!isDo(currentPoint, currentPointsList))//
            {
                allPause();
            }
            else
            {
                allPlay();
            }

            //放在这里不合适，有时间了在重构
            txtWidget.text = currentPoint.info();
            if (Input.GetKeyDown(KeyCode.P))
            {
                txtGameObj.SetActive(true);
            }
        }
    }

    //协调进度
    public void adjust()
    {
        allPlay();
        spm.run();
        cpc.setZero();
        isStart = true;
    }

    private void allPlay()
    {
        pc.play();
        up1.play();
        up2.play();
    }

    private void allPause()
    {
        pc.pause();
        up1.pause();
        up2.pause();
    }

    private bool isDo(PointEntry point, List<PointEntry> pointsList)
    {
        //执行一次操作
        pe.addOneDoneTimes();
        //in path
        PointEntry tmp = cpc.nowBasePoint(point);
        if (tmp.existIn(pointsList))
        {
            cpc.changePoints(tmp, point);
            //指示正确信号
            pointerContoller.correntShow();
            return true;
        }

        //reset
        if (point.isOriginalPoint()&&!cpc.isPrePointOfZero())
        {
            //重置信号
            pointerContoller.defaultShow();
            cpc.reset(pointsList[0]);
            //错误次数增加
           pe.addErrTimes();
            return false;
        }
        //错误信号
        pointerContoller.errorShow();
        return false;
    }
}