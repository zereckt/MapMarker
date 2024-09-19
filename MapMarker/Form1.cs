using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Dictionary<string, List<CPoint>> pointsYear;
        GMapOverlay ListP = new GMapOverlay("Координаты");
        List<Point> list = new List<Point>();
        List<FileInfo> fileNames;


        int[] pointsColor = new int[4];
        string year;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pointsYear = new Dictionary<string, List<CPoint>>();
        }
        private void gmap_Load(object sender, EventArgs e)
        {
            gmap.Bearing = 0;
            gmap.MapProvider = GMapProviders.OpenStreetMap;
            gmap.Position = new PointLatLng(62.021872, 129.721982);
            gmap.MinZoom = 2;
            gmap.MaxZoom = 16;
            gmap.Zoom = 10;
            gmap.DragButton = MouseButtons.Left;
            gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            gmap.ShowCenter = false;
            gmap.MarkersEnabled = true;
            gmap.IgnoreMarkerOnMouseWheel = true;
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<CPoint> points = new List<CPoint>();

            // Пользователь выбирает файл на компьютере
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Multiselect = true;
            //Фильтр формата файла
            ofd.Filter = "Text files (*.txt)|*.txt";
            // Путь
            ofd.InitialDirectory = @"C:\Users\PC\Desktop";

            ofd.Title = "Выбирите файл";

            fileNames = new List<FileInfo>();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] dirs = ofd.FileNames[0].Split('\\');
                
                year = dirs[dirs.Length - 2];

                foreach (string f in ofd.FileNames)

                    fileNames.Add(new FileInfo(f));
            }
            try
            {
                foreach (FileInfo fl in fileNames)
                {
                    // Читаем в массив все строки файла
                    string[] ArrayOfStrings = File.ReadAllLines(fl.FullName, Encoding.GetEncoding(1251));

                    // От 0 до количества строк в файле
                    for (int i = 0; i < ArrayOfStrings.Length; i++)
                    {
                        // Парсим строку на поля структуры
                        string[] OneString = ArrayOfStrings[i].Split(new char[] { '	' });

                        // Добавляем эту структуру 
                        points.Add(new CPoint(Convert.ToDouble(OneString[0]), Convert.ToDouble(OneString[1]), Convert.ToDouble(OneString[2])));
                    }
                }

                gmap.Overlays.Add(ListP);
               
                pointsYear.Add(year, points);

                draw_map();

                pointsYear = new Dictionary<string, List<CPoint>>();

                MessageBox.Show("Данные успешно прочитаны", "MapMarker");
            }
            catch
            {
                MessageBox.Show("Ошибка при попытке открытия файла", "MapMarker");
            }
        }

        //Рисование маркеров
        private void draw_map()
        {
            // Шаг
            int step;
            step = Convert.ToInt32(toolStripComboBox1.Text);

            очиститьToolStripMenuItem.Visible = true;
            toolStripComboBox1.Visible = true;
            toolStripComboBox2.Visible = true;
            ToolStripMenuItem.Visible = true;
            ToolStripMenuItem2.Visible = true;

            //Размер
            int size;
            size = Convert.ToInt32(toolStripComboBox2.Text);
            int[] pointsColor_ = new int[4] { 0, 0, 0, 0 };


            foreach (string key in pointsYear.Keys)
            {
                List<CPoint> points = pointsYear[key];

                for (int i = 0; i < points.Count; i += step)
                {
                    Bitmap bitmap = new Bitmap(size, size);
                    Graphics g = Graphics.FromImage(bitmap);

                    // Задаем маркер z
                    if (points[i].z >= 200)
                    {
                        g.FillEllipse(new SolidBrush(Color.Red), 0, 0, bitmap.Width, bitmap.Height);
                        pointsColor_[3]++;
                    }
                    else if (points[i].z >= 150)
                    {
                        g.FillEllipse(new SolidBrush(Color.Yellow), 0, 0, bitmap.Width, bitmap.Height);
                        pointsColor_[2]++;
                    }
                    else if (points[i].z >= 100)
                    {
                        g.FillEllipse(new SolidBrush(Color.LimeGreen), 0, 0, bitmap.Width, bitmap.Height);
                        pointsColor_[1]++;
                    }
                    else if (points[i].z >= 50)
                    {
                        g.FillEllipse(new SolidBrush(Color.DodgerBlue), 0, 0, bitmap.Width, bitmap.Height);
                        pointsColor_[0]++;
                    }

                    pointsColor = pointsColor_;

                    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(points[i].x, points[i].y), bitmap);

                    marker.ToolTip = new GMapRoundedToolTip(marker);
                    //Комментарий над маркером
                    marker.ToolTipText = Convert.ToString(points[i].z) + " (" + key + ")";

                    ListP.Markers.Add(marker);

                    list.Add(new Point((int)Math.Round(points[i].x), (int)Math.Round(points[i].y)));
                }
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialogforsavemap = new SaveFileDialog())
            {
                // Формат картинки
                dialogforsavemap.Filter = "PNG (*.png)|*.png";

                // Название картинки
                dialogforsavemap.FileName = "";

                Image image = gmap.ToImage();

                if (dialogforsavemap.ShowDialog() == DialogResult.OK)
                {
                    string fileName = dialogforsavemap.FileName;

                    if (!fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        fileName += ".png";

                    image.Save(fileName);

                    MessageBox.Show("Карта успешно сохранена в директории: " + Environment.NewLine + dialogforsavemap.FileName, "MapMarker");
                }
            }
        }
        //Очистка карты
        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListP.Clear();
            pointsYear = new Dictionary<string, List<CPoint>>();
            pointsColor = new int[4] { 0, 0, 0, 0 };
            очиститьToolStripMenuItem.Visible = false;
            toolStripComboBox1.Visible = false;
            toolStripComboBox2.Visible = false;
            ToolStripMenuItem.Visible = false;
            ToolStripMenuItem2.Visible = false;
        }
        // Отмена ввода
        private void toolStripComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        // Отмена ввода
        private void toolStripComboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        // Изменение шага маркера
        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            ListP.Clear();
            draw_map();
        }
        // Изменение размера маркера
        private void toolStripComboBox2_TextChanged(object sender, EventArgs e)
        {
            ListP.Clear();
            draw_map();
        }

        // Вращение карты
        private void metroTrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            gmap.Bearing = metroTrackBar1.Value;
        }
        // Приближение
        private void button1_Click(object sender, EventArgs e)
        {
            gmap.Zoom++;
        }
        // Отдаление
        private void button2_Click(object sender, EventArgs e)
        {
            gmap.Zoom--;
        }
        // Выход
        private void выходToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        // Окно гистограммы
        private void гистограммаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2(pointsYear, pointsColor);
            fm2.Show();
        }
        // Окно графика
        private void графикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 fm4 = new Form4(pointsYear);
            fm4.Show();
        }

        private void базаДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 fm3 = new Form3();
            fm3.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
    //Класс CPoint
    public class CPoint
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public CPoint(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }
}