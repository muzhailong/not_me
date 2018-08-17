using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace my
{
    public class ProcessEntry
    {
        //时间以秒为单位
        public int startTime;
        public int errTimes;
        public int allTime;
        public int doneTimes;


        public ProcessEntry()
        {
            errTimes = 0;
            doneTimes = 0;
        }


        public void addErrTimes(int times=1)
        {
            this.errTimes += times;
        }

        public float getSuccessRate()
        {
            if (doneTimes == 0)
            {
                return 0;
            }
            return errTimes / doneTimes;
        }

        public void addOneDoneTimes()
        {
            this.doneTimes++;
        }
        public void reset()
        {
            startTime = (int)Time.time;
            errTimes = 0;
            doneTimes = 0;
        }
    }
}
