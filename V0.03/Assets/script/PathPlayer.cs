using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vectrosity;

namespace my
{
    public class PathPlayer
    {
        public enum LINE_TYPE
        {
            ALL, PART
        }

        public PointEntryCollections pec;
        private bool canPlay;
        private int cntPtIndex;
        private VectorLine vl;
        private LINE_TYPE lineType;
        private Color c;
        private bool valid = false;


        public PathPlayer(  PointEntryCollections pec,VectorLine vl,Color c,LINE_TYPE lineType=LINE_TYPE.ALL)
        {
            this.pec = pec;
            this.vl = vl;
            this.lineType = lineType;
            this.c = c;
            canPlay = false;
            cntPtIndex = -1;
        }

        public PathPlayer(PointEntryCollections pec, VectorLine vl, LINE_TYPE lineType = LINE_TYPE.ALL)
        :this(pec, vl, Color.white, lineType)
        {}

        public void draw()
        {
            if (isValid())
            {
                if (isFirstFrame())
                {
                    vl.points2.Add(next());
                }
                else
                {
                    vl.points2.Add(next());
                    switch (lineType)
                    {
                        case LINE_TYPE.ALL:
                            break;
                        case LINE_TYPE.PART:
                            RemainPointCache();
                            break;
                    }
                    vl.SetColor(c);
                    vl.Draw();
                }
            }
        }

        public void pause()
        {
            valid = false;
        }

        public void play()
        {
            valid = true;
        }

        public void ready()
        {
            canPlay = true;
            cntPtIndex=0;
        }

        public bool isFirstFrame()
        {
            return cntPtIndex == 0;
        }

        public Vector2 next()
        {
            return pec.pei2vec(cntPtIndex++);
        } 

        public bool isValid()
        {
            return pec.isValidIndex(cntPtIndex)&&canPlay&&valid;
        }


        public void forward(int step=Config.FORWARD_STEP)
        {
            for (int i = 0; i < step; ++i)
            {
                vl.points2.Add(next());
            }
        }

        public void RemainPointCache(int remainNum=Config.REMAIN_POINTS_COUNT)
        {
            int p = vl.points2.Count;
            if (p < remainNum) return;
            List<Vector2> tmpList = new List<Vector2>(remainNum);
            tmpList.Clear();
            for (int i = 0; i < remainNum; ++i)
            {
                tmpList.Add(vl.points2[p - remainNum + i]);
            }
            vl.points2.Clear();
            foreach (var item in tmpList)
            {
                vl.points2.Add(item);
            }
        }

        public PointEntry getPointByIndex(int index)
        {
            if (index >= pec.entries.Count)
            {
                return null;
            }
            return pec.entries[index];
        }

        public List<PointEntry> copyPoints()
        {
            return pec.reverseCC(cntPtIndex-vl.points2.Count,vl.points2.Count);
          
        }
    }
}
