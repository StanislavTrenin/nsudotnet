using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
//using System.Windows.Controls;

namespace Trenin.Nsudotnet.Perlin
{
    public class Form1 : Form
    {

        static public int size { get; set; }
        static public string filename { get; set; }
        public Button button1;
        public Image im;
        PictureBox pictureBox1;
        GridBase g, g2, g4;

        public Form1()
        {
            this.Size = new Size(750, 450);
            //			this.ClientSize = new Size(730, 430);
            button1 = new Button();
            button1.Size = new Size(40, 40);
            button1.Location = new Point(30, 30);
            button1.Text = "Click me";
            this.Controls.Add(button1);
            button1.Click += new EventHandler(button1_Click);


            pictureBox1 = new PictureBox();
            pictureBox1.Size = new Size(750, 450);
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            pictureBox1.Location = new Point(1, 1);
            pictureBox1.ClientSize = new Size(750, 450);
            this.Controls.Add(pictureBox1);
            pictureBox1.Paint += new PaintEventHandler(PixPaint);


            im = Image.FromFile("index.bmp");

            g = new GridBase(10, 32, Color.BlueViolet);
            g2 = g.coarse(2);
            g4 = g.coarse(3);
            //			g = new GridBase( 8, 50, "index.bmp");

            //			pictureBox1.Image = im;

        }

        private void PixPaint(object sender, PaintEventArgs e)
        {

            // Translate transformation matrix.
            int red = 0;
            int white = 11;
            while (white <= 100)
            {
                e.Graphics.FillRectangle(Brushes.Red, 200 - white, red, 200, 10);
                e.Graphics.FillRectangle(Brushes.White, 200 - red, white, 200, 10);
                red += 20;
                white += 19;
            }
            e.Graphics.ResetTransform();
            e.Graphics.FillRectangle(new SolidBrush(Color.Red), 300, 300, 50, 50);
            e.Graphics.FillRectangle(new SolidBrush(Color.Blue), 320, 320, 10, 10);

            Rectangle destRect = new Rectangle(0, 0, 400, 400);
            e.Graphics.DrawImage(g.b(), destRect, 0, 0, 400, 400, GraphicsUnit.Pixel);

            //			Rectangle destRect1 = new Rectangle(400, 500, 600, 700 );
            //			e.Graphics.DrawImage( g2.b(), destRect1, 400, 500, 600, 700, GraphicsUnit.Pixel  );

            //			Rectangle destRect2 = new Rectangle(600, 600, 700, 700 );
            //			e.Graphics.DrawImage( g4.b(), destRect2, 600, 600, 700, 700, GraphicsUnit.Pixel  );

            //			Rectangle destRect1 = new Rectangle(0, 0, im.Width, im.Height );
            //			e.Graphics.DrawImage( im, destRect1, 0, 0, im.Width, im.Height, GraphicsUnit.Pixel  );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello World");
            g.draw();
            g2.draw();
            g4.draw();

            //im.Save (filename);
        }

        public static void Main(string[] args)
        {
            //			size = 0;
            //			filename = "";

            //			size = int.Parse( args[0], NumberStyles.AllowParentheses | NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
            //			filename = args[1];

            //			Console.WriteLine (size);
            //			Console.WriteLine (filename);

            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }
}
