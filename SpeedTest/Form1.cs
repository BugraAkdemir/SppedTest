using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;


namespace SpeedTest
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string exePath = Path.Combine(Application.StartupPath, "aa.exe"); // speedtest.exe dosyasının tam yolu

                if (!File.Exists(exePath))
                {
                    MessageBox.Show("Speedtest.exe bulunamadı! Dosyanın doğru yerde olduğundan emin olun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Process process = new Process();
                process.StartInfo.FileName = exePath;
                process.StartInfo.Arguments = "--format=json-pretty"; // JSON formatında çıktı almak için
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // JSON verisini oku ve uygun şekilde parse et
                var result = JsonSerializer.Deserialize<SpeedTestResult>(output);

                // Sonuçları ekrana yazdır
                textBox1.Text = $"Ping: {result.ping.latency} ms\n" +
                                $"Download: {result.download.bandwidth / 125000.0:F2} Mbps\n" +
                                $"Upload: {result.upload.bandwidth / 125000.0:F2} Mbps";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                textBox2.Text = ex.Message;
            }
        }

        public class SpeedTestResult
        {
            public PingResult ping { get; set; }
            public SpeedResult download { get; set; }
            public SpeedResult upload { get; set; }
        }

        public class PingResult
        {
            public double latency { get; set; }
        }

        public class SpeedResult
        {
            public double bandwidth { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
