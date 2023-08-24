using System;
namespace Cut_Optimizer
{

    public class PolygonEngine
    {
        private List<Polygon> polygons;
        
    }
    public struct PolygonPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class Polygon
    {
        private List<PolygonPoint> points;
        private List<PolygonPoint> correctedPoints;
        private PolygonPoint boxMinX, boxMinY, boxMaxX, boxMaxY;
        private PolygonPoint centerOfRotation;
        public List<PolygonPoint> Points 
        { 
            get 
            {
                if (correctedPoints != null)
                {
                    return correctedPoints;
                }   
                return points; 
            } 
        }
        public void AddPoint(PolygonPoint point)
        {   
            if (this.points == null)
            {
                this.points = new List<PolygonPoint>();
            }            
            this.points.Add(point);
        }
        public void RotatePoints(PolygonPoint centerOfRotation, double angleOfRotationRAD)
        {
            List<PolygonPoint> memoredPoints;
            this.centerOfRotation= centerOfRotation;
            if (correctedPoints == null)
            {
                correctedPoints = new List<PolygonPoint>();
                memoredPoints = this.points;
            }
            else
            {
                memoredPoints = this.correctedPoints;
                correctedPoints.Clear();
            }  
            foreach (PolygonPoint point in memoredPoints) 
            {
                double XP = point.X - this.centerOfRotation.X;
                double YP = point.Y - this.centerOfRotation.Y;
                double R = Math.Sqrt(Math.Pow(XP, 2) + Math.Pow(YP, 2 - centerOfRotation.X));
                double alpha = Math.Acos(XP / R);
                PolygonPoint rotatedPoint = new PolygonPoint() {
                    X = centerOfRotation.X + R / Math.Cos(alpha + angleOfRotationRAD),
                    Y = centerOfRotation.Y + R / Math.Sin(alpha + angleOfRotationRAD)
                };
                correctedPoints.Add(rotatedPoint);
            }
            
            
        }
        private double determinateArea()
        {
            return 0d;
        }
        private void createBox()
        {
            //double rectangleLenght;
            //double rectangleWidht;
            //double rectangleArea;
            List<PolygonPoint> currentPoints;
            if (this.correctedPoints != null)
            {
                currentPoints = this.correctedPoints;
            }
            else 
            {
                currentPoints = this.points;
            }
            this.boxMinX = currentPoints.MinBy(p => p.X);
            this.boxMaxX = currentPoints.MaxBy(p => p.X);
            this.boxMinY = currentPoints.MinBy(p => p.Y);
            this.boxMaxY = currentPoints.MaxBy(p => p.Y);
            


        }
    }
}

