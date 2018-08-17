
using System;
using System.Collections.Generic;

namespace my
{
    public class PointEntry
    {
        public float x;
        public float y;
        public float z;
        public float rx;
        public float ry;
        public float rz;
        public float power;//压力
        public float voltage;//电压

        public PointEntry(float x,float y,float power=0)
        {
            this.x = x;
            this.y = y;
            this.power = power;
        }

        public PointEntry(params float[] arr)
        {
            rx = arr[0];
            ry = arr[1];
            rz = arr[2];
            z = arr[3];
            y = arr[4];
            x = arr[5];
            voltage = arr[6];
            power = arr[7];
        }


        public void add(PointEntry disVec)
        {
            this.x += disVec.x;
            this.y += disVec.y;
        }

        public void sub(PointEntry disVec)
        {
            this.x -= disVec.x;
            this.y -= disVec.y;
        }

        public static PointEntry add(PointEntry p1,PointEntry p2)
        {
            PointEntry res = p1.copy();
            res.add(p2);
            return res;
        }

        public static PointEntry sub(PointEntry p1,PointEntry p2)
        {
            PointEntry res = p1.copy();
            res.sub(p2);
            return res;
        }

        public PointEntry copy()
        {
            return new PointEntry(x, y, power);
        }

        public float distance(PointEntry p)
        {
            return (float)Math.Sqrt(Math.Pow(p.x - x, 2)+Math.Pow(p.y - y,2));
        }

        //使用圆半径判断原点
        public bool isOriginalPoint()
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y,2)) < 0.0001;
        }

        public void setZero()
        {
            this.x = 0;
            this.y = 0;
        }

        public bool existIn(List<PointEntry> pointsList)
        {
            foreach (PointEntry item in pointsList)
            {
                if (this.distance(item) < Config.MIN_DISTANCE)
                {
                    return true;
                }
            }
            return false;
        }


        public bool equals(PointEntry p)
        {
            return distance(p) <= 0.001;
        }


        //简单使用中点平滑
        public PointEntry simpleSmoothPoint(PointEntry p)
        {
            return new PointEntry((this.x + p.x) / 2, (this.y + p.y) / 2);
        }


        public string info()
        {
            return "rx:" + rx + " ,ry:" + ry + " ,rz:" + rz + " ,x:" 
                + x + " ,y:" + y + " ,z:" + z + " ,p:" + power + " ,v:" + voltage;
        }
    }

}