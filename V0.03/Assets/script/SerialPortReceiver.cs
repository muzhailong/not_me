using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Collections;

namespace my
{
    class SerialPortReceiver
    {
        private string portName;
        private int baudRate;
        private Parity parity;
        private int dataBits;
        private StopBits stopBits;

        private SerialPort sp = null;
        private Thread portReceiveThread;
        public PointEntryCollections pec;
        private PointEntry currentPoint=new PointEntry(0,0,0);

        public SerialPortReceiver(
            string portName = Config.DEFAULT_PORT_NAME,
            int baudRate = Config.DEFAULT_BAUD_RATE,
            Parity parity = Config.DEFAULT_PARITY,
            int dataBits = Config.DEFAULT_DATA_BITS,
            StopBits stopBits = Config.DEFAULT_STOP_BITS)
        {
            this.portName = portName;
            this.baudRate = baudRate;
            this.parity = parity;
            this.dataBits = dataBits;
            this.stopBits = stopBits;
        }

        //通过异常判断是否连接上
        public void connect()
        {
            try
            {
                open();
                if (pec != null)
                {
                    pec.clear();
                }
                else
                {
                    pec = new PointEntryCollections();
                }
                portReceiveThread = new Thread(new ThreadStart(dataReceiveFunction));
                portReceiveThread.IsBackground = true;
                //portReceiveThread.Start();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void close()
        {
            try
            {
                if (portReceiveThread.IsAlive)
                {
                    portReceiveThread.Abort();
                    Thread.Sleep(1000);
                }
            }
            catch (Exception )
            { }
            
            try
            {
                if (sp!=null&&sp.IsOpen)
                {
                    sp.Close();
                    Thread.Sleep(1000);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        //打开端口
        private void open()
        {
            if (sp == null || !sp.IsOpen)
            {
                sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
                sp.ReadTimeout = 400;
                try
                {
                    sp.Open();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        //数据接收线程
        private void dataReceiveFunction()
        {
            StreamWriter writer = new StreamWriter("1.txt");
            while (sp!=null&&sp.IsOpen)
            {
                try
                {
                    String s = sp.ReadLine();
                    Debug.Log(s);
                    if (s == null || s.Equals("0")) continue;
                    //269.74  357.42  354.83  0 - 81   2 2734.0  4.03
                    string[] items = s.Split(new String[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    writer.WriteLine(float.Parse(items[5]) + "," + float.Parse(items[4]));
                    writer.Flush();

                    currentPoint = new PointEntry(
                            float.Parse(items[0]),float.Parse(items[1]),float.Parse(items[2]),
                            float.Parse(items[3]),float.Parse(items[4]),float.Parse(items[5]),
                            float.Parse(items[6]),float.Parse(items[7])
                            );
                    //currentPoint = new PointEntry(float.Parse(items[5]), float.Parse(items[4]));
                    pec.addOneEntry(currentPoint);
                }
                catch (Exception)
                {
                }
            }
        }

        public void run()
        {
            portReceiveThread.Start();
        }

        //是否连接
        public bool isConnected()
        {
            if (sp == null || !sp.IsOpen)
            {
                return false;
            }
            return true;
        }

        public PointEntry now()
        {
            return currentPoint;
        }
    }
}
