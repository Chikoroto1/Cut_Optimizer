namespace Cut_Optimizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            double g = Math.Pow(2, 2);
            MessageBox.Show(g.ToString());
                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Polygon polygon = new Polygon();
            MessageBox.Show(polygon.determinatePolygonArea().ToString()); 
        }
    }
}