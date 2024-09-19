using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {

        Dictionary<string, List<CPoint>> pointsYear;

        public Form4(Dictionary<string, List<CPoint>> pointsYear)
        {
            InitializeComponent();

            this.pointsYear = pointsYear;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Maximum = 300;
            chart1.ChartAreas[0].AxisY.Title = "Толщина льда в сантиметрах";
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12);

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Interval = 400;
            chart1.ChartAreas[0].AxisX.Title = "Протяженность в метрах";
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12 );


            foreach (string key in pointsYear.Keys)
            {
                List<CPoint> points = pointsYear[key];
                chart1.Series.Add(key + " год");
                chart1.Series[key + " год"].ChartType = SeriesChartType.SplineArea;
                chart1.Series[key + " год"].BackHatchStyle = ChartHatchStyle.BackwardDiagonal;

                for (int i = 0; i < points.Count; i += 100)
                {
                    chart1.Series[key + " год"].Points.AddXY(i, points[i].z);
                }
            }
            
        }
    }
}
