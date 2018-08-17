using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using my;
using System;

//串口连接
public class ConnectionController : MonoBehaviour {
    private Button conBtn;
    private Text txtComponent;
    private SerialPortManager spm;

    void Start ()
    {
        spm=GameObject.FindGameObjectWithTag("resource_manager").GetComponent<SerialPortManager>();
        conBtn =GetComponent<Button>();
        txtComponent = GetComponentInChildren<Text>();
        conBtn.onClick.AddListener(handler);
        txtComponent.text = Config.UNCONNECT_TEXT;
    }

    private void handler()
    {
        if (spm.isConnected())
        {
            try
            {
                spm.close();
                txtComponent.text = Config.UNCONNECT_TEXT;
            }
            catch (Exception)
            {
                MessageBox.simpleTip("连接关闭异常！", "提示");
            }
        }
        else if (!spm.isConnected())
        {
            try
            {
                spm.connect();//可以获取异常信息
                txtComponent.text = Config.CONNECT_TEXT;
            }
            catch (Exception)
            {
                MessageBox.simpleTip("无法连接到手柄！", "提示");
            }
        }
    }
}
