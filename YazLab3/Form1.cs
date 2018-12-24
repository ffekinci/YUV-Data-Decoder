using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YazLab3
{
    public partial class Form1 : Form
    {
        String path;
        int width, height;
        int total_frame;
        int TimerCount = 0;
        Boolean flag = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "yuv files (*.yuv)|*.yuv";
            dialog.InitialDirectory = @"C:\Users\ffekinci\Desktop";
            dialog.Title = "Please select an image file to encrypt.";

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                path = dialog.FileName;
                //Console.WriteLine(path);
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            string a = t_height.Text;
            int result;
            if (!int.TryParse(a, out result))
            {
                MessageBox.Show("Lütfen geçerli değer girin!");
                return;
            }
            height = result;

            result = 0;
            string b = t_width.Text;
            if (!int.TryParse(b, out result))
            {
                MessageBox.Show("Lütfen geçerli değer girin!");
            }
            width = result;

            if (!string.IsNullOrEmpty(path))
            {
                var v = type.SelectedIndex;
                //Console.WriteLine(v);

                ClearFolder();

                if (v == 0)
                {
                    Decode444(path);
                }
                else if (v == 1)
                {                  
                    Decode422(path);
                }
                else if (v == 2)
                {
                    Decode420(path);
                }
            }

            
        }

        private void Decode444(string path)
        {
            var bytes = File.ReadAllBytes(path);
            //Console.WriteLine(bytes.Length);

            total_frame = bytes.Length / (width * height * 3);
            //trackBar1.Minimum = 1;
            //trackBar1.Maximum = total_frame;
            //frame.Text = "Frame : 1";

            Bitmap bitmap = new Bitmap(width, height);
            Color color;

            for (int m = 0; m < total_frame; m++)
            {

                int j = 0, k = 0;
                int begin = m * width * height * 3;
                for (int i = begin; i < begin + width * height; i++)
                {
                    color = Color.FromArgb(bytes[i], bytes[i], bytes[i]);

                    bitmap.SetPixel(k, j, color);

                    k++;

                    if (k == width)
                    {
                        k = 0;
                        j++;
                    }
                }
                string s = "frame-" + m + ".bmp";
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                bitmap.Save("bmp/"+s);

            }


        }

        private void Decode422(string path)
        {
            var bytes = File.ReadAllBytes(path);
            //Console.WriteLine(bytes.Length);

            total_frame = bytes.Length / (width * height * 2);
            //trackBar1.Minimum = 1;
            //trackBar1.Maximum = total_frame;
            //frame.Text = "Frame : 1";

            Bitmap bitmap = new Bitmap(width, height);
            Color color;

            for (int m = 0; m < total_frame; m++)
            {

                int j = 0, k = 0;
                int begin = m * width * height * 2;
                for (int i = begin; i < begin + width * height; i++)
                {
                    color = Color.FromArgb(bytes[i], bytes[i], bytes[i]);

                    bitmap.SetPixel(k, j, color);

                    k++;

                    if (k == width)
                    {
                        k = 0;
                        j++;
                    }
                }
                string s = "frame-" + m + ".bmp";
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                bitmap.Save("bmp/"+s);

            }


        }

        private void Decode420(string path)
        {
            var bytes = File.ReadAllBytes(path);
            //Console.WriteLine(bytes.Length);

            total_frame = (int) (bytes.Length / (width * height * 1.5));
            //trackBar1.Minimum = 1;
            //trackBar1.Maximum = total_frame;
            //frame.Text = "Frame : 1";

            Bitmap bitmap = new Bitmap(width, height);
            Color color;

            for (int m = 0; m < total_frame; m++)
            {

                int j = 0, k = 0;
                int begin =(int)( m * width * height * 1.5);
                for (int i = begin; i < begin + width * height; i++)
                {
                    color = Color.FromArgb(bytes[i], bytes[i], bytes[i]);

                    bitmap.SetPixel(k, j, color);

                    k++;

                    if (k == width)
                    {
                        k = 0;
                        j++;
                    }
                }
                string s = "frame-" + m + ".bmp";
                pictureBox1.Image = bitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                bitmap.Save("bmp/"+s);

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!flag)
            {
                timer1.Interval = 40;
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                flag = false;
            }
            //pictureBox1.Image = new Bitmap("bmp/frame-1.bmp");
            //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            flag = true;
           
            if(TimerCount == total_frame-1)
            {
                timer1.Enabled = false;
                TimerCount = 0;
                flag = false;
                
            }
            pictureBox1.Image = new Bitmap("bmp/frame-" + TimerCount + ".bmp");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            TimerCount++;
        }

        public void ClearFolder()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo("bmp");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }




    }
}
