﻿using System;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using static System.Windows.Forms.DataFormats;

namespace Cut_Optimizer
{

    public class PolygonEngine
    {
        public Polygon SheetFormat { get { return this.sheetFormat; } }
        
        private List<Polygon> polygons;
        private Polygon sheetFormat;
        public PolygonEngine(Polygon sheetFormat) 
        {
            this.sheetFormat = sheetFormat;
            this.SheetFormat.createBox();
        }
        public void FitToSheet()
        {
            double lenghtA;
            IEnumerable<Polygon> h = from P in polygons
                    orderby P.BoxAre descending
                    select P;


            List<Polygon> l = h.ToList();
            
        }
        private void createAnArrangement()
        { 

        }
        private bool lokingForIntersection(PolygonPoint firstSegmentFirstPoint, PolygonPoint firstSegmentSecondPoint, PolygonPoint secondSegmentFirstPoint, PolygonPoint secontSegmentSecontPoint)
        {
            
            double tanAlpha1 = (firstSegmentSecondPoint.Y - firstSegmentFirstPoint.Y) / (firstSegmentSecondPoint.X - firstSegmentFirstPoint.X);
            double tanAlpha2 = (secontSegmentSecontPoint.Y - secondSegmentFirstPoint.Y) / (secontSegmentSecontPoint.X - secondSegmentFirstPoint.X); ;
            double sum = tanAlpha1 - tanAlpha2;
            if (sum == 0)
            {
                return false;
            }
            return true;
        }
        public void AddPolygon(Polygon polygon)
        {
            if (polygons == null)
            {
                polygons = new List<Polygon>();
            }
            polygon.createBox();
            this.polygons.Add(polygon);
        }

    }
    public struct PolygonPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public PolygonPoint(double x, double y)
        {
            X = x;
            Y = y;
        }        
    }
    public class Polygon
    {
        private int quantity;
        private List<PolygonPoint> points;
        private List<PolygonPoint> correctedPoints;
        private Polygon boxPlygon;
        private double boxMinX, boxMinY, boxMaxX, boxMaxY;
        private PolygonPoint centerOfRotation;
        private double area;
        private double boxArea;

        public double Area { get { return this.area; } }
        public double BoxAre { get { return this.boxArea; } }
        public Polygon() 
        {
            quantity = 1;
        }
        public Polygon(int quantity)
        {
            this.quantity = quantity;
        }
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
            PolygonPoint rotatedPoint;
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

                if (alpha is double.NaN)
                {
                    rotatedPoint = new PolygonPoint(
                    centerOfRotation.X, // + R / Math.Cos(alpha + angleOfRotationRAD),
                    centerOfRotation.Y// + R / Math.Sin(alpha + angleOfRotationRAD)
                    );
                }
                else 
                {
                    var h1 = centerOfRotation.X + R * Math.Cos(alpha + angleOfRotationRAD);
                    var h2 = centerOfRotation.Y + R * Math.Sin(alpha + angleOfRotationRAD);
                    rotatedPoint = new PolygonPoint(
                    centerOfRotation.X + R * Math.Cos(alpha + angleOfRotationRAD),
                    centerOfRotation.Y + R * Math.Sin(alpha + angleOfRotationRAD)
                    );
                }
                                
                correctedPoints.Add(rotatedPoint);
            }
            
            
        }
        public void OffsetPolygon()
        { 
        }
        public double determinateThreePointPolygonArea()
        {
            //double[,] doubles = new double[,] {
            //    {0,           57.52922972, 1 },
            //    {27.5199921,  18.53104996, 1 },
            //    {91.41062703, 57.52922972, 1 },                
            //};
            double[,] doubles = new double[,] {
                {3,   3 },
                {-5,  -2 }                
            };
            double determinant = 0;
            double area = 0;
            int lenght = doubles.GetLength(1);
            for (int i = 0; i < lenght; i++) 
            {
                double multiplication = 1;
                double multiplicationMinus = 1;
                for (int j = 0; j < lenght; j++)
                {
                    int _i = j + i;
                    int _iMinus = lenght - (j + i) - 1;
                    if (_i > lenght - 1)
                    {
                        _i = i;
                        _iMinus = lenght - i - 1;
                    }                    
                    multiplication *= doubles[_i, j];
                    multiplicationMinus*= doubles[_iMinus, j];
                }
                determinant += multiplication;
                determinant -= multiplicationMinus;
            }

            area = determinant / 2;

            return area;
        }
        public double determinatePolygonArea(Polygon polygon) 
        {
            //double[,] doubles = new double[,] {
            //    {0,   57.52922972 },
            //    {83.55985646,  73.62245796 },
            //    {91.41062703,  57.52922972 },
            //    {27.5199921,  18.53104996 }
            //};
            double[,] doubles = new double[polygon.Points.Count, 2];
            for (int i = 0; i < polygon.Points.Count; i++)
            {
                doubles[i, 0] = polygon.Points[i].X;
                doubles[i, 1] = polygon.Points[i].Y;
            }

            double determinant = 0;
            double area = 0;
            int lenght = doubles.GetLength(0);
            for (int i = 0; i < lenght; i++)
            {
                double multipler = 1;
                double multiplrtMinus = 1;
                for (int j = 0; j < 2; j++)
                {
                    int iP = i + j;
                    
                    if (iP > lenght - 1)
                    {
                        iP = i - (lenght - 1);
                    }
                    
                    multipler *= doubles[iP, j];
                    multiplrtMinus *= doubles[iP, 1 - j];                    
                }
                determinant += multipler;
                determinant -= multiplrtMinus;                
            }

            area = determinant / 2;

            return Math.Abs(area);            
        }
        public void createBox()
        {
            //double rectangleLenght;
            //double rectangleWidht;
            //double rectangleArea;
            if (boxPlygon == null)
            {
                boxPlygon = new Polygon();
            }
            List<PolygonPoint> currentPoints;
            if (this.correctedPoints != null)
            {
                currentPoints = this.correctedPoints;
            }
            else 
            {
                currentPoints = this.points;
            }
            this.boxMinX = currentPoints.MinBy(p => p.X).X;
            this.boxMaxX = currentPoints.MaxBy(p => p.X).X;
            this.boxMinY = currentPoints.MinBy(p => p.Y).Y;
            this.boxMaxY = currentPoints.MaxBy(p => p.Y).Y;
            this.boxPlygon.AddPoint(new PolygonPoint(boxMinX,boxMinY));
            this.boxPlygon.AddPoint(new PolygonPoint(boxMinX, boxMaxY));
            this.boxPlygon.AddPoint(new PolygonPoint(boxMaxX, boxMaxY));
            this.boxPlygon.AddPoint(new PolygonPoint(boxMaxX, boxMinY));
            this.boxArea = determinatePolygonArea(boxPlygon);
        }
        private double internalAngle(PolygonPoint firstPolygonPoint, PolygonPoint secondPolygonPoint)
        {
            double aV = Math.Sqrt(Math.Pow(firstPolygonPoint.X, 2) + Math.Pow(firstPolygonPoint.Y, 2));
            double bV = Math.Sqrt(Math.Pow(secondPolygonPoint.X, 2) + Math.Pow(secondPolygonPoint.Y, 2));
            double scalar = firstPolygonPoint.X * secondPolygonPoint.X + firstPolygonPoint.Y * secondPolygonPoint.Y;
            double Alpha = Math.Acos(scalar / (aV * bV));
            return Alpha;
        }



    }
}

