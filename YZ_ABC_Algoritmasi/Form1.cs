using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace YZ_ABC_Algoritmasi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            textBox1.Text = "20"; // Varsayılan Koloni Boyutu
            textBox2.Text = "100"; // Varsayılan İterasyon Sayısı
            textBox3.Text = "50"; // Varsayılan Limit
            button1.Text = "Optimize";
            button1.Click += new EventHandler(btnOptimize_Click);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Bu metod Form yüklenirken yapılacak işlemler için kullanılabilir
        }

        private void btnOptimize_Click(object sender, EventArgs e)
        {
            int koloniBoyutu = int.Parse(textBox1.Text);
            int maksIterasyon = int.Parse(textBox2.Text);
            int limit = int.Parse(textBox3.Text);
            int boyutlar = 5; // Problemin boyutunu belirtin
            double[] altSinirlar = Enumerable.Repeat(-5.0, boyutlar).ToArray(); // Uygun sınırları ayarlayın
            double[] ustSinirlar = Enumerable.Repeat(5.0, boyutlar).ToArray(); // Uygun sınırları ayarlayın

            ABCAlgoritmasi abc = new ABCAlgoritmasi(koloniBoyutu, maksIterasyon, limit, boyutlar, altSinirlar, ustSinirlar);
            var (enIyiCozum, enIyiUygunluk, uygunlukGecmisi) = abc.OptimizasyonYap();

            richTextBox1.Text = $"En İyi Çözüm: {string.Join(", ", enIyiCozum)}\nEn İyi Uygunluk: {enIyiUygunluk}";
            YakinsamaGrafikCiz(uygunlukGecmisi);
        }

        private void YakinsamaGrafikCiz(double[] uygunlukGecmisi)
        {
            chart1.Series.Clear();
            Series series = new Series
            {
                Name = "Yakınsama",
                ChartType = SeriesChartType.Line,
                Color = System.Drawing.Color.Blue
            };
            chart1.Series.Add(series);

            for (int i = 0; i < uygunlukGecmisi.Length; i++)
            {
                series.Points.AddXY(i, uygunlukGecmisi[i]);
            }

            chart1.Invalidate();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Bu olayı kullanmak istemiyorsanız, bu metodu boş bırakabilirsiniz
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
