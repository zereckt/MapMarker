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
    public partial class Form2 : Form
    {
        int[] pointsColor = new int[4];

        Dictionary<string, List<CPoint>> pointsYear;

        public Form2(Dictionary<string, List<CPoint>> pointsYear,int[] pointsColor)
        {
            InitializeComponent();

            this.pointsColor = pointsColor;

            this.pointsYear = pointsYear;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            chart1.ChartAreas[0].AxisY.Title = "Количество маркеров";
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12);

            chart1.ChartAreas[0].AxisX.Title = "Толщина льда в сантиметрах";
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12);

            foreach (string key in pointsYear.Keys)
            {
                List<CPoint> points = pointsYear[key];

                chart1.Series.Add(key);

                chart1.Series[key].Points.Add(pointsColor[0]);
                chart1.Series[key].Points[0].Color = Color.DodgerBlue;
                chart1.Series[key].Points[0].AxisLabel = "50 - 100";
                chart1.Series[key].Points[0].Label = pointsColor[0].ToString(); ;


                chart1.Series[key].Points.Add(pointsColor[1]);
                chart1.Series[key].Points[1].Color = Color.LimeGreen;
                chart1.Series[key].Points[1].AxisLabel = "100 - 150";
                chart1.Series[key].Points[1].Label = pointsColor[1].ToString(); ;

                chart1.Series[key].Points.Add(pointsColor[2]);
                chart1.Series[key].Points[2].Color = Color.Yellow;
                chart1.Series[key].Points[2].AxisLabel = "150 - 200";
                chart1.Series[key].Points[2].Label = pointsColor[2].ToString();

                chart1.Series[key].Points.Add(pointsColor[3]);
                chart1.Series[key].Points[3].Color = Color.Red;
                chart1.Series[key].Points[3].AxisLabel = "200 <";
                chart1.Series[key].Points[3].Label = pointsColor[3].ToString();
            }
        }
    }
}
