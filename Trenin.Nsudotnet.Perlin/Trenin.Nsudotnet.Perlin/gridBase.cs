using System;
using System.Drawing;

namespace Trenin.Nsudotnet.Perlin
{
    enum colorchoice : uint { cred, cgreen, cblue, any };

    interface GridPaint
    {

        void Paint(string file, int x, int y, Graphics g, uint c);
        Bitmap mix(int f0, ref Bitmap b0, int f1, ref Bitmap b1);
        Bitmap mixwith(int f0, ref Bitmap b0, int f1, ref GridBase g, uint color);
        float GetColor(int x, int y);
    }



    public class GridBase : GridPaint
    {

        Color bc;

        private int xstep;
        private int ystep;
        private int xintervals;
        private int yintervals;


        public void Paint(string file, int x, int y, Graphics g, uint color = 3)
        {
            Bitmap tb = new Bitmap(xstep * xintervals, ystep * yintervals);
            tb = this.b(color);
            Image im = new Bitmap(tb);
            im.Save(file);
            tb.Save("test.png");
            g.DrawImage(tb, new Point(x, y));
            im.Dispose();
        }

        public Bitmap mix(int f0, ref Bitmap b0, int f1, ref Bitmap b1)
        {
            throw new NotImplementedException();
        }

        public Bitmap mixwith(int f0, ref Bitmap bm, int f1, ref GridBase gr, uint color = 3)
        {
            throw new NotImplementedException();
        }

        public float GetColor(int x, int y)
        {
            return 255 * (255 * ((float)red.interpolate(x, y))
                + ((float)green.interpolate(x, y))
                ) + ((float)blue.interpolate(x, y));
        }

        public Color Get_Color(int x, int y)
        { return Color.FromArgb(255, red.interpolate(x, y), green.interpolate(x, y), blue.interpolate(x, y)); }

        private class cmesh
        {
            public byte vmax;
            public byte max(byte a, byte b) { return (a > b ? a : b); }
            private int xstep, xintervals, ystep, yintervals;
            public byte[,] mesh { get; private set; }
            public byte setindex(int i, int j, byte v)
            {
                vmax = max((mesh[i, j] = v), vmax);
                return mesh[i, j];
            }
            public byte setpixel(int i, int j, byte v)
            {
                int di = i / xstep; int ri = i - di * xstep;
                int dj = j / ystep; int rj = j - dj * ystep;
                if (2 * ri > xstep) di++;
                if (2 * rj > ystep) dj++;
                return setindex(di, dj, v);
            }
            public cmesh(int xh, int xis, int yh, int yis)
            {
                xstep = xh; xintervals = xis; ystep = yh; yintervals = yis;
                mesh = new byte[xintervals + 1, yintervals + 1];
                vmax = 0;
            }
            public byte interpolate(int i, int j)
            {
                int di = i / xstep; int ri = i - di * xstep; int dj = j / ystep; int rj = j - dj * ystep;
                if (di < xintervals && dj < yintervals)
                    return (byte)((((int)mesh[di + 1, dj + 1]) * ri * rj + ((int)mesh[di + 1, dj]) * ri * (ystep - rj)
                        + ((int)mesh[di, dj + 1]) * (xstep - ri) * rj + ((int)mesh[di, dj]) * (xstep - ri) * (ystep - rj)) / xstep / ystep);
                else return 0;
            }
        }

        cmesh red, green, blue;

        public GridBase(int xs, int x_intervals, int ys, int y_intervals, Color c)
        {
            Random r = new Random();
            bc = c;
            xstep = xs;
            ystep = ys;
            xintervals = x_intervals;
            yintervals = y_intervals;

            red = new cmesh(xs, x_intervals, ys, y_intervals);
            green = new cmesh(xs, x_intervals, ys, y_intervals);
            blue = new cmesh(xs, x_intervals, ys, y_intervals);

            for (int i = 0; i < xintervals + 1; i++)
                for (int j = 0; j < yintervals + 1; j++)
                {
                    red.setindex(i, j, (byte)r.Next(50, 255));
                    green.setindex(i, j, (byte)r.Next(50, 255));
                    blue.setindex(i, j, (byte)r.Next(50, 255));
                }
        }

        public GridBase(int x_intervals, int y_intervals, string name)
        {
            bc = Color.Azure;
            xintervals = x_intervals;
            yintervals = y_intervals;

            Image im = new Bitmap(name);
            Bitmap b = new Bitmap(im);

            // Console.WriteLine("\n" + b.Width + " " + b.Height + "\n Hello World!");

            xstep = (int)(((float)b.Width) / ((float)xintervals));
            ystep = (int)(((float)b.Height) / ((float)yintervals));

            //Console.WriteLine("\n" + xstep + " " + xintervals + " " + ystep + " " + yintervals + "\n Hello World!");


            red = new cmesh(xstep, x_intervals, ystep, y_intervals);
            green = new cmesh(xstep, x_intervals, ystep, y_intervals);
            blue = new cmesh(xstep, x_intervals, ystep, y_intervals);

            for (int i = 0, ix = 0; i < xintervals + 1 && ix < b.Width; i++, ix += xstep)
                for (int j = 0, jy = 0; j < yintervals + 1 && jy < b.Height; j++, jy += ystep)
                {
                    red.setindex(i, j, (byte)b.GetPixel(ix, jy).R);
                    green.setindex(i, j, (byte)b.GetPixel(ix, jy).G);
                    blue.setindex(i, j, (byte)b.GetPixel(ix, jy).B);
                }

            im.Dispose();
        }

        public GridBase coarse(int timesx, int timesy)
        {
            GridBase r = new GridBase(xstep * timesx, xintervals / timesx, ystep * timesy, yintervals / timesy, bc);
            for (int i = 0; i < xintervals / timesx + 1; i++)
                for (int j = 0; j < yintervals / timesy + 1; j++)
                {
                    r.red.setindex(i, j, red.mesh[timesx * i, timesy * j]);
                    r.green.setindex(i, j, green.mesh[timesx * i, timesy * j]);
                    r.blue.setindex(i, j, blue.mesh[timesx * i, timesy * j]);
                }
            return r;
        }

        public GridBase sharp(int timesx, int timesy)
        {
            GridBase r = new GridBase(xstep / timesx, xintervals * timesx, ystep / timesy, yintervals * timesy, bc);
            for (int i = 0; i < xintervals * timesx + 1; i++)
                for (int j = 0; j < yintervals * timesy + 1; j++)
                {
                    r.red.setindex(i, j, red.interpolate((i * xstep) / timesx, (j * ystep) / timesy));
                    r.green.setindex(i, j, green.interpolate((i * xstep) / timesx, (j * ystep) / timesy));
                    r.blue.setindex(i, j, blue.interpolate((i * xstep) / timesx, (j * ystep) / timesy));
                }
            return r;
        }

        public Bitmap b(uint color = 3)
        {
            int w = xintervals * xstep;
            int h = yintervals * ystep;
            Bitmap b = new Bitmap(w, h);

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {

                    if (color == 0)
                        b.SetPixel(i, j, Color.FromArgb(bc.A, ((255 * (int)red.interpolate(i, j))) / red.vmax, 0, 0));
                    else if (color == 1)
                        b.SetPixel(i, j, Color.FromArgb(bc.A, 0, ((255 * (int)green.interpolate(i, j))) / green.vmax, 0));
                    else if (color == 2)
                        b.SetPixel(i, j, Color.FromArgb(bc.A, 0, 0, ((255 * (int)blue.interpolate(i, j))) / blue.vmax));
                    else b.SetPixel(i, j, Color.FromArgb(255,
                    ((255 * (int)red.interpolate(i, j))) / red.vmax,
                    ((255 * (int)green.interpolate(i, j))) / green.vmax,
                    ((255 * (int)blue.interpolate(i, j))) / blue.vmax));
                }
            return b;
        }

        public Bitmap m(uint color = 3)
        {
            int w = xintervals * xstep;
            int h = yintervals * ystep;
            Bitmap b = new Bitmap(w, h);

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    Color c = Color.Black;
                    if (i % xstep == 0 && j % ystep == 0)
                    {
                        if (color == 0)
                            b.SetPixel(i, j, Color.FromArgb(255, red.mesh[i / xstep, j / ystep], 0, 0));
                        else if (color == 1)
                            b.SetPixel(i, j, Color.FromArgb(255, 0, green.mesh[i / xstep, j / ystep], 0));
                        else if (color == 2)
                            b.SetPixel(i, j, Color.FromArgb(255, 0, 0, blue.mesh[i / xstep, j / ystep]));
                        else b.SetPixel(i, j, Color.FromArgb(255,
                                            red.mesh[i / xstep, j / ystep],
                                            green.mesh[i / xstep, j / ystep],
                                            blue.mesh[i / xstep, j / ystep]));
                    }
                    else b.SetPixel(i, j, Color.FromArgb(255, c.R, c.G, c.B));
                }
            return b;
        }

        public void write(string file) { Bitmap bw = b(); bw.Save(file); }

        ~GridBase() { ;}
    }
}
