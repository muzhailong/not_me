using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace my
{
    class CoordinatePointChange
    {
        private PointEntry basePoint;
        private PointEntry baseRealPoint;


        public CoordinatePointChange()
        {
            basePoint = new PointEntry(0, 0);
            baseRealPoint = new PointEntry(0, 0);
        }

        public PointEntry nowBasePoint(PointEntry currentRealPoint)
        {
            PointEntry p = PointEntry.sub(currentRealPoint, baseRealPoint);
            p.add(basePoint);
            return p;
        }

        public void changePoints(PointEntry bp, PointEntry brp)
        {
            this.basePoint = bp;
            this.baseRealPoint = brp;
        }

        public void reset(PointEntry bp)
        {
            this.basePoint = bp;
            this.baseRealPoint.setZero();
        }

        public bool isPrePointOfZero()
        {
            return baseRealPoint.isOriginalPoint();
        }


        public void setZero()
        {
            this.basePoint.setZero();
            this.baseRealPoint.setZero();
        }
    }
}