using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace lab11
{
    public partial class Form1 : Form
    {
        public struct Simple
        {
            public double xx; public double yy; public int ii;
        };
        FileInfo my_file = new FileInfo("SCRATCH");
        double x, y, xmin, xmax, ymin, ymax; double X, Y, Xmin, Xmax, Ymin, Ymax;
        double fx, fy, f, xC, yC, XC, YC, c1, c2;
        Simple s; Graphics dc; Pen p;

        public Form1()
        {
            InitializeComponent();
        }

        private int IX(double x)
        {
            double xx = x * (pictureBox1.Size.Width / 10.0) + 0.5; return (int)xx;
        }
        private int IY(double y)
        {
            double yy = pictureBox1.Size.Height - y * (pictureBox1.Size.Height / 7.0) + 0.5;
            return (int)yy;
        }
        double middle(double x1, double x2)
        { return (x1 + x2) / 2; }
        void star(double xc, double yc, double r)
        {
            double phi, phi_, r_half, r_double, factor = 0.0174533;
            double temp_x, temp_y;
            int i;
            if (r < 0.1) return;
            using (BinaryWriter fw = new BinaryWriter(my_file.Open(FileMode.Open,
            FileAccess.Write, FileShare.Write)))
            {
                fw.Seek(0, SeekOrigin.End);
                s.xx = xc + r; s.yy = yc; s.ii = 0;
                fw.Write(s.xx); fw.Write(s.yy); fw.Write(s.ii);
                for (i = 1; i <= 5; i++)
                {
                    phi = i * 72 * factor;/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    phi_ = (i + 1) * 72 * factor;
                    s.xx = xc; s.yy = yc; s.ii = 1;
                    fw.Write(s.xx); fw.Write(s.yy); fw.Write(s.ii);

                    s.xx = xc + r * Math.Cos(phi); s.yy = yc + r * Math.Sin(phi); s.ii = 1;
                    fw.Write(s.xx); fw.Write(s.yy); fw.Write(s.ii);
                    temp_x = middle(xc + r * Math.Cos(phi), xc + r * Math.Cos(phi_));
                    temp_y = middle(yc + r * Math.Sin(phi), yc + r * Math.Sin(phi_));
                    temp_x = middle(temp_x, xc);
                    temp_y = middle(temp_y, yc);
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////
                    temp_x = middle(temp_x, xc);
                    temp_y = middle(temp_y, yc);

                    temp_x = middle(xc + r * Math.Cos(phi), xc + r * Math.Cos(phi_));
                    temp_y = middle(yc + r * Math.Sin(phi), yc +r * Math.Sin(phi_));
                    temp_x = middle(temp_x, xc);
                    temp_y = middle(temp_y, yc);

                 

                    s.xx = temp_x; s.yy = temp_y; s.ii = 1;
                    fw.Write(s.xx); fw.Write(s.yy); fw.Write(s.ii);

                    s.xx = xc + r * Math.Cos(phi_); s.yy = yc + r * Math.Sin(phi_); s.ii = 1;
                    fw.Write(s.xx); fw.Write(s.yy); fw.Write(s.ii);
                }
                fw.Close();
            }
            r_half = r * 0.5;
            r_double = 2 * r;
            for (i = 0; i < 5; i++)
            {
                phi = (36 + i * 72) * factor;

                star(xc + r_double * Math.Cos(phi), yc + r_double * Math.Sin(phi), r_half);

            }
        }
       
       
        
       
        private void Form1_Activated(object sender, EventArgs e)
        {
            int x2, y2;
            textBox1.Text = pictureBox1.Location.X.ToString();
            textBox2.Text = pictureBox1.Location.Y.ToString();
            x2 = pictureBox1.Location.X + pictureBox1.Size.Width;
            textBox3.Text = x2.ToString();
            y2 = pictureBox1.Location.Y + pictureBox1.Size.Height;
            textBox4.Text = y2.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x1, y1, x2, y2;
            x1 = System.Convert.ToInt32(textBox1.Text);
            y1 = System.Convert.ToInt32(textBox2.Text);
            pictureBox1.Location = new Point(x1, y1);
            x2 = x1 + System.Convert.ToInt32(textBox3.Text);
            y2 = y1 + System.Convert.ToInt32(textBox4.Text);
            pictureBox1.Size = new Size(x2, y2);
        }

        private void записатьВФайлSCRATCHToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (BinaryWriter fw = new BinaryWriter(my_file.Open(FileMode.Create,
           FileAccess.Write, FileShare.Write)))
            {
                fw.Close();
            }
            star(0.0, 0.0, 1);
        }

        private void прочитатьИзФайлпаИОтобразитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dc = pictureBox1.CreateGraphics(); p = new Pen(Brushes.Red, 1);
            Point X1Y1 = new Point(); Point X2Y2 = new Point();
            using (BinaryReader fr = new BinaryReader(my_file.Open(FileMode.OpenOrCreate,

            FileAccess.Read, FileShare.Read)))
            {
                while (fr.BaseStream.Position < fr.BaseStream.Length)
                {
                    s.xx = fr.ReadDouble();
                    s.yy = fr.ReadDouble();
                    s.ii = fr.ReadInt32();
                    x = s.xx; y = s.yy;
                    if (x < xmin) xmin = x; if (x > xmax) xmax = x;
                    if (y < ymin) ymin = y; if (y > ymax) ymax = y;
                }
                fr.Close();
            }
            Xmin = 0; Xmax = 10; Ymin = 0; Ymax = 7;
            fx = (Xmax - Xmin) / (xmax - xmin);

            fy = (Ymax - Ymin) / (ymax - ymin);
            f = (fx < fy ? fx : fy);
            xC = 0.5 * (xmin + xmax);
            yC = 0.5 * (ymin + ymax);
            XC = 0.5 * (Xmin + Xmax);
            YC = 0.5 * (Ymin + Ymax);
            c1 = XC - f * xC;
            c2 = YC - f * yC;
            using (BinaryReader fr = new BinaryReader(my_file.Open(FileMode.OpenOrCreate,
            FileAccess.Read, FileShare.Read)))
            {
                while (fr.BaseStream.Position < fr.BaseStream.Length)
                {
                    s.xx = fr.ReadDouble();
                    s.yy = fr.ReadDouble();
                    s.ii = fr.ReadInt32();
                    x = s.xx; y = s.yy;

                    x = s.xx;
                    y = s.yy;
                    X = f * x + c1;
                    Y = f * y + c2;
                    X2Y2.X = IX(X);
                    X2Y2.Y = IY(Y);
                    if (s.ii == 1) { dc.DrawLine(p, X1Y1, X2Y2); X1Y1 = X2Y2; }

                    else { X1Y1 = X2Y2; }

                }
                fr.Close();
            }
        }

        private void очиститьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            dc.Clear(Color.White);
        }

        private void выходToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Close();
        }



    }
}
