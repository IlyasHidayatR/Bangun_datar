using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tugas2
{
    public partial class Form1 : Form
    {
        int X, Y, W, H, K, J, zero=0;
        //float X1, Y1, X2, Y2;
        int Bangun;
        Graphics Gr;
        int mouseState = 0;
        Bitmap myBitmap;
        Bitmap myBitmap1;
        Color warna;
        Color cat;
        private struct PointF
        {
            public float X;
            public float Y;
        };
        const int jp = 10;
        const float dt = 0.04F;
        Point[] PK = new Point[jp];
        Point[] temp1 = new Point[jp];
        Point[] temp2 = new Point[jp];
        int jmlPoint = 0;
        Bitmap tmpimg1, lineimg1, tmpimg2, lineimg2;
        Graphics gr, bmpGraph;
        Pen pBlack = new Pen(Color.Black,2);
        Pen pBlue = new Pen(Color.Blue,2);
        int pAktif;
        Pen[] aPen = new Pen[3];
        int tempX, tempY;
        bool mDown = false;

        public Form1()
        {
            InitializeComponent();
            gr = pictureBox1.CreateGraphics();
            aPen[0] = new Pen(Color.Blue, 2);
            aPen[1] = new Pen(Color.Red, 2);
            aPen[2] = new Pen(Color.Yellow, 2);
            myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            myBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Gr = Graphics.FromImage(myBitmap);
            warna = Color.Black;
            cat = Color.Gray;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Pen myPen = new Pen(warna);
            W = e.X;
            H = e.Y;
            K = W - X;
            J = H - Y;
            switch (Bangun)
            {
                case 0:

                    Gr.DrawEllipse(myPen, X, Y, K, J);
                    break;
                case 1:

                    Gr.DrawRectangle(myPen, X, Y, K, J);
                    break;
                case 2:

                    Gr.DrawLine(myPen, X, Y, W, H);
                    break;
            }
        }

        private long fac(int n)
        {
            long f = 1;
            if (n <= 0){
                return 1;
            }
            else {
                for (int i=1; i<=n; i++){
                    f *= i;
                }
                return f;
            }
        }

        private int kom(int n, int j)
        {
            int k;
            k = (int)(fac(n)/(fac(j)*fac(n-j)));
            return k;

        }

        private float pangkatF(float f, int p)
        {
            float hasil = 1;
            if (p == 0)
            {
                return 1;
            }
            else
            {
                for (int i = 1; i <= p; i++)
                {
                    hasil *= f;
                }
                return hasil;
            }
            
        }

        private float B13(float u)
        {
            return (float)(u * u / 2 - u + 0.5);
        }
        private float B23(float u)
        {
            return (float)(-u * u + u + 0.5);
        }
        private float B33(float u)
        {
            return (float)(u * u / 2);
        }

        private PointF hitungQ(int j, float u)
        {
            PointF PF;
            PF.X = B13(u) * PK[j].X + B23(u) * PK[j + 1].X + +B33(u) * PK[j + 2].X;
            PF.Y = B13(u) * PK[j].Y + B23(u) * PK[j + 1].Y + +B33(u) * PK[j + 2].Y;
            return (PF);
        }

        private PointF hitungPoint(float t)
        {
           
            int N = jmlPoint - 1;
            float koef;
            PointF PF;
            PF.X = PF.Y = 0;
            for (int j = 0; j < jmlPoint; j++)
            {
                koef = kom(N, j)*pangkatF(1-t, N-j)*pangkatF(t,j);
                PF.X += koef * PK[j].X;
                PF.Y += koef * PK[j].Y;
            }
            return (PF);
        }

        private void drawPK()
        {
            for (int j = 0; j < jmlPoint; j++)
            {
                gr.DrawEllipse(pBlack, PK[j].X, PK[j].Y, 4, 4);
            }
        }

        private void drawSpline3()
        {
            float u;
            PointF PF;
            Point dari = new Point(0, 0);
            Point ke = new Point(0, 0);
            int indexPen;
            PF = hitungQ(0, 0);
            dari.X = (int)(PF.X);
            dari.Y = (int)(PF.Y);
            for (int j = 0; j < jmlPoint - 2; j++)
            {
                indexPen = jmlPoint % 3;
                u = 0;
                while (u <= 1)
                {
                    PF = hitungQ(j, u);
                    ke.X = (int)(PF.X);
                    ke.Y = (int)(PF.Y);
                    gr.DrawLine(aPen[indexPen], dari, ke);
                    u += dt;
                    dari = ke;
                }
            }
        }

        private void drawCurve()
        {
            float t=0;
            PointF PF;
            Point dari = PK[0];
            Point k = new Point(0, 0);
            while (t <= 1)
            {
                PF = hitungPoint(t);
                k.X = (int)(PF.X);
                k.Y = (int)(PF.Y);
                gr.DrawLine(pBlue, dari, k);
                t += dt;
                dari=k;
            }
        }

        private int jarak(Point a, Point b)
        {
            double j;
            int dx, dy;
            dx = b.X - a.X;
            dy = b.Y - a.Y;
            j = Math.Sqrt(dx * dx + dy * dy);
            return ((int)j);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            double jar, jt;
            Point p = new Point(e.X, e.Y);
            tempX = e.X;
            tempY = e.Y;
            jt = jarak(p, PK[0]);
            pAktif = 0;
            mDown = true;
            if (checkBox1.Checked && (!checkBox2.Checked))
            {
                for (int i = 1; i < jmlPoint; i++)
                {
                    jar = jarak(p, PK[i]);
                    if (jar < jt)
                    {
                        jt = jar;
                        pAktif = i;
                    }
                }
            }
            else
            {
                if (jmlPoint < jp)
                {
                    PK[jmlPoint].X = e.X;
                    PK[jmlPoint].Y = e.Y;
                    jmlPoint++;
                    gr.DrawEllipse(pBlack, e.X, e.Y, 4, 4);
                }
            }
            if (checkBox2.Checked)
            {
            mouseState = 1;
            X = e.X;
            Y = e.Y;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            jmlPoint = 0;
            gr.Clear(Color.White);
            Gr.Clear(Color.White);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //drawCurve();
            drawSpline3();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int dx, dy;
            if (checkBox1.Checked && mDown && (!checkBox2.Checked))
            {
                dx = e.X - tempX;
                dy = e.Y - tempY;
                PK[pAktif].X += dx;
                PK[pAktif].Y += dy;

                tempX = e.X;
                tempY = e.Y;
                gr.Clear(Color.White);

                drawPK();
                //drawCurve();
                drawSpline3();
            }
            else if (checkBox2.Checked)
            {
                Pen myPen = new Pen(warna);
                if (mouseState == 1)
                {
                    Gr.Clear(Color.White);
                    if (myBitmap1 != null)
                    {
                        Gr.DrawImage(myBitmap1, 0, 0);
                    }
                    K = (e.X - X);
                    J = (e.Y - Y);
                    switch (Bangun)
                    {
                        case 0:
                            Gr.DrawEllipse(myPen, X, Y, K, J);
                            break;
                        case 1:
                            Gr.DrawRectangle(myPen, X, Y, K, J);
                            break;
                        case 2:
                            Gr.DrawLine(myPen, X, Y, e.X, e.Y);
                            break;
                    }
                    pictureBox1.Image = myBitmap;
                }
                textBox1.Text = Convert.ToString(X);
                textBox2.Text = Convert.ToString(Y);
                textBox3.Text = Convert.ToString(K);
                textBox4.Text = Convert.ToString(J);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mDown = false;
            if (checkBox2.Checked)
            {
            Pen myPen = new Pen(warna);
            //int tebal = 3;
            myPen.Width = 3;
            Gr.Clear(Color.White);
            if (myBitmap1 != null)
            {
                Gr.DrawImage(myBitmap1, 0, 0);
            }
            switch (Bangun)
            {
                case 0:
                    Gr.DrawEllipse(myPen, X, Y, K, J);
                    break;
                case 1:
                    Gr.DrawRectangle(myPen, X, Y, K, J);
                    break;
                case 2:
                    Gr.DrawLine(myPen, X, Y, e.X, e.Y);
                    break;
            }
            myBitmap1 = (Bitmap)myBitmap.Clone();
            pictureBox1.Image = myBitmap;
            mouseState = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bangun = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bangun = 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bangun = 2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            myBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = myBitmap1;
            zeroAll();
        }

        private void zeroAll()
        {
            textBox1.Text = zero.ToString();
            textBox2.Text = zero.ToString();
            textBox3.Text = zero.ToString();
            textBox4.Text = zero.ToString();
        }
        
    }
}
