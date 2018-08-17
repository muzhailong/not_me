using my;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

public class PathEnterAction : MonoBehaviour {

    private Button enterBtn;
    private Button saveBtn;
    private Button backBtn;
    private Rect rect;
    private SerialPort sp = null;
    private Thread portReceiveThread;
    private PointEntryCollections pec;
    private GameObject mainCanvas;



    private VectorLine vl;
    private bool isStart;

    void Start ()
    {
        enterBtn = transform.Find("enter_btn").GetComponent<Button>();
        saveBtn = transform.Find("save_btn").GetComponent<Button>();
        backBtn = transform.Find("back_btn").GetComponent<Button>();

        mainCanvas = GameObject.FindGameObjectWithTag("main_canvas");
        rect = GetComponent<RectTransform>().rect;
        sp = new SerialPort(Config.DEFAULT_PORT_NAME, Config.DEFAULT_BAUD_RATE, 
                    Config.DEFAULT_PARITY, Config.DEFAULT_DATA_BITS, Config.DEFAULT_STOP_BITS);
        enterBtn.onClick.AddListener(dealWith1);//start
        saveBtn.onClick.AddListener(dealWith2);//save
        backBtn.onClick.AddListener(dealWith3);//返回
        pec = new PointEntryCollections();
        isStart = false;
        vl = new VectorLine("Dots", new List<Vector2>(), 4, LineType.Points);
    }

    void Update()
    {
        if (isStart)
        {
            vl.SetColor(Color.red);
            vl.Draw();
        }
    }

    private void dealWith1()
    {
        if (!sp.IsOpen)
        {
            sp.Open();
        }
        portReceiveThread = new Thread(new ThreadStart(bgThreadFunc));
        portReceiveThread.IsBackground = true;
        portReceiveThread.Start();
        isStart = true;
    }


    private void close()
    {
        try
        {
            if (sp.IsOpen)
            {
                sp.Close();
            }
        }
        catch (Exception) { }

        try
        {
            if (portReceiveThread.IsAlive)
            {
                portReceiveThread.Abort();
            }
        }
        catch (Exception) { }
    }

    //save
    private void dealWith2()
    {
        close();
        List<PointEntry>lt=smoothDealWith();

        StreamWriter writer = new StreamWriter("1.txt");
        string s = lt[0].x + "," + lt[0].y;
        writer.Write(s);

        for(int i =1; i < lt.Count; ++i)
        {
            s = "\n"+lt[i].x +","+ lt[i].y;
            writer.Write(s);
        }
        writer.Flush();
        writer.Close();
        isStart = false;
        //切换
        Destroy(GameObject.Find("Dots"));
        gameObject.SetActive(false);
        mainCanvas.SetActive(true);
    }


    //back
    private void dealWith3()
    {
        close();
        Destroy(GameObject.Find("Dots"));
        gameObject.SetActive(false);
        mainCanvas.SetActive(true);
    }

    private void bgThreadFunc()
    {
        sp.DiscardInBuffer();
        while (sp != null && sp.IsOpen)
        {
            try
            {
                string s = sp.ReadLine();
                
                if (s == null || s.Equals("0")) continue;
                string[] items = s.Split(new String[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                pec.addOneEntry(new PointEntry(float.Parse(items[5]), float.Parse(items[4])));
                vl.points2.Add(
                    new Vector2(float.Parse(items[5])+rect.width/2, float.Parse(items[4])+rect.height/2));
            }
            catch (Exception)
            {
            }
        }
    }

    private List<PointEntry> smoothDealWith()
    {
        int st = 0, ed = 0;
        int sz = pec.entries.Count;

        List<PointEntry> res = new List<PointEntry>(ed-st+1);

        for (int i = 20; i < sz; ++i)
        {
            if (!pec.entries[i].equals(pec.entries[i - 1]))
            {
                st = i - 1;
                break;
            }
        }

        for (int i = sz - 1; i > 0; --i)
        {
            if (!pec.entries[i - 1].equals(pec.entries[i]))
            {
                ed = i;
                break;
            }
        }

        res.Add(pec.entries[st]);
        //进行微弱的平滑处理，插入一个点
        for(int i = st+1; i <= ed; ++i)
        {
            if (isNeedSmooth(pec.entries[i], pec.entries[i - 1]))
            {
                res.Add(pec.entries[i].simpleSmoothPoint(pec.entries[i - 1]));
            }
            res.Add(pec.entries[i]);
        }
        return res;
    }

    private bool isNeedSmooth(PointEntry p1, PointEntry p2)
    {
        return p1.distance(p2) >= Config.MIN_SMOOTH_DISTANCE;
    }
}
