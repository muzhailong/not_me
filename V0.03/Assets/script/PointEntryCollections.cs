using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace my
{
    public class PointEntryCollections
    {
        public List<PointEntry> entries;

        float X_min = float.MaxValue, X_max = float.MinValue;
        float Y_min = float.MaxValue, Y_max = float.MinValue;
        float X_sm = 0, Y_sm = 0;
        float X_mean = 0, Y_mean = 0;
        float x_factor=1, y_factor=1;
        PointEntry disVec;


        public PointEntryCollections()
        {
            entries = new List<PointEntry>();
        }

        public void addOneEntry(PointEntry e)
        {
            entries.Add(e);
        }
       
        public void clear()
        {
            entries.Clear();
        }

        public Vector2 pei2vec(int index)
        {
            return new Vector2(entries[index].x, entries[index].y);
        }

        public bool isValidIndex(int index)
        {
            return index < entries.Count && index >= 0;
        }

        public static PointEntryCollections build(String path)
        {
            StreamReader reader = new StreamReader(path);
            string line = "";
            PointEntry pe = null;
            PointEntryCollections pec = new PointEntryCollections();
            while ((line = reader.ReadLine()) != null)
            {
                var item = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                pe = new PointEntry(float.Parse(item[0]), float.Parse(item[1]), 0);
                pec.addOneEntry(pe);
            }
            reader.Close();
            return pec;
        }

        public PointEntryCollections compressChange(PointEntry centerPoint,KeyValuePair<float,float>rect)
        {
            foreach (PointEntry e in entries)
            {
                X_min = Math.Min(X_min, e.x);
                X_max = Math.Max(X_max, e.x);
                Y_min = Math.Min(Y_min, e.y);
                Y_max = Math.Max(Y_max, e.y);
                X_sm += e.x;
                Y_sm += e.y;
            }
            X_mean = X_sm / entries.Count();
            Y_mean = Y_sm / entries.Count();

            //按条件压缩
            if(Math.Abs(X_max - X_min) > rect.Key)
            {
                x_factor = rect.Key / Math.Abs(X_max - X_min);
            }
            if (Math.Abs(Y_max - Y_min) > rect.Key)
            {
                y_factor = rect.Value / Math.Abs(Y_max - Y_min);
            }
     
            disVec = new PointEntry(centerPoint.x - X_mean, centerPoint.y - Y_mean);

            foreach (PointEntry item in entries)
            {
                item.x = X_mean +(item.x-X_mean) * x_factor;
                item.y = Y_mean +(item.y-Y_mean) * y_factor;
                item.add(disVec);
            }
            return this;
        }

        public List<PointEntry> reverseCC(int down,int size=Config.REMAIN_POINTS_COUNT)
        {
            List<PointEntry> res = new List<PointEntry>(size);
            for(int i = 0; i < size; ++i)
            {
                PointEntry tmp = entries[down + i].copy();
                res.Add(tmp);
                tmp.sub(disVec);
                tmp.x = X_mean + (tmp.x - X_mean) / x_factor;
                tmp.y = Y_mean + (tmp.y - Y_mean) / y_factor;
            }
            return res;
        }
    }
}
