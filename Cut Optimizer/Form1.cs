using System.Drawing;

namespace Cut_Optimizer
{
    public partial class Form1 : Form
    {
        // Test props
        Polygon sheetFormat;
        Polygon partPolygon;
        Polygon partPolygon2;
        Polygon partPolygon3;
        private void createTestProps()
        {
            sheetFormat = new Polygon();
            sheetFormat.AddPoint(new PolygonPoint(0D, 0D));
            sheetFormat.AddPoint(new PolygonPoint(250D, 0D));
            sheetFormat.AddPoint(new PolygonPoint(250D, 170D));
            sheetFormat.AddPoint(new PolygonPoint(0D, 170D));

            partPolygon = new Polygon(4);
            partPolygon.AddPoint(new PolygonPoint(0D, 0D));
            partPolygon.AddPoint(new PolygonPoint(25D, 0D));
            partPolygon.AddPoint(new PolygonPoint(25D, 17D));
            partPolygon.AddPoint(new PolygonPoint(0D, 17D));

            partPolygon2 = new Polygon();
            partPolygon2.AddPoint(new PolygonPoint(0D, 0D));
            partPolygon2.AddPoint(new PolygonPoint(35D, 0D));
            partPolygon2.AddPoint(new PolygonPoint(35D, 17D));
            partPolygon2.AddPoint(new PolygonPoint(0D, 17D));

            partPolygon3 = new Polygon();
            partPolygon3.AddPoint(new PolygonPoint(0D, 0D));
            partPolygon3.AddPoint(new PolygonPoint(15D, 0D));
            partPolygon3.AddPoint(new PolygonPoint(15D, 17D));
            partPolygon3.AddPoint(new PolygonPoint(0D, 17D));
        }
        //
        Graphics graphics;
        PolygonEngine PolygonEngine;
        public Form1()
        {
            InitializeComponent();
            createTestProps();                            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Polygon polygon = new Polygon();
            sheetFormat.RotatePoints(new PolygonPoint(0, 250), Math.PI / 2 * 3);
            PolygonEngine polygonEngine = new PolygonEngine(sheetFormat);
            
            polygonEngine.AddPolygon(partPolygon);
            polygonEngine.AddPolygon(partPolygon2);
            polygonEngine.AddPolygon(partPolygon3);
            polygonEngine.FitToSheet();
            //MessageBox.Show(polygon.determinatePolygonArea().ToString()); 
        }
        private void updateDisplay()
        {   
            PolygonEngine = new PolygonEngine(sheetFormat);
            PolygonEngine.AddPolygon(partPolygon);






            this.graphics = this.panel1.CreateGraphics();
            Pen pen = new Pen(Color.Red, 3);
            List<PointF> pointsF = new List<PointF>();
            foreach (var point in PolygonEngine.SheetFormat.Points)
            {
                pointsF.Add(new PointF((float)point.X, (float)point.Y));
            }
            pointsF.Add(new PointF((float)PolygonEngine.SheetFormat.Points[0].X, (float)PolygonEngine.SheetFormat.Points[0].Y));

            PointF[] points = pointsF.ToArray();

            graphics.DrawLines(pen, points);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            updateDisplay();
            
        }
    }
}