using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using my;
using System.IO;
using System.Threading;
using System;

public class SerialPortManager : MonoBehaviour {
    private SerialPortReceiver spr;

    void Start ()
    {
        spr = new SerialPortReceiver();
	}

    public void connect()
    {
        try
        {
            if (!spr.isConnected())
            {
                spr.connect();
            }
        }
        catch(Exception e)
        {
            throw e;
        }
    }

    public void run()
    {
        spr.run();
    }

    public void close()
    {
        try
        {
            spr.close();
        }catch(Exception e)
        {
            throw e;
        }
    }

    public bool isConnected()
    {
        return spr.isConnected();
    }

    public PointEntry acquireCurrentPoint()
    {
        return spr.now();
    }
}
