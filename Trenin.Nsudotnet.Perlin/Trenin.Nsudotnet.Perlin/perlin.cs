using System;
using System.Text;
using System.Drawing;

namespace Trenin.Nsudotnet.Perlin
{
    class MainClass
    {

        public static void Main(string[] args)
        {
            GridBase g, g2, g4;

            int xstep = 256;
            int xintervals = 1;
            int ystep = 256;
            int yintervals = 1;
            string filename = "";

            if (args.Length == 5)
            {
                xstep = int.Parse(args[0]);
                xintervals = int.Parse(args[1]);
                ystep = int.Parse(args[2]);
                yintervals = int.Parse(args[3]);
                filename = args[4];
                g = new GridBase(xstep, xintervals, ystep, yintervals, Color.Beige);
                g2 = new GridBase(xstep / 2, xintervals * 2, ystep / 2, yintervals * 2, Color.DarkGray);
                g4 = new GridBase(xstep / 4, xintervals * 4, ystep / 4, yintervals * 4, Color.LightGray);
            }
            else if (args.Length == 3)
            {
                xintervals = int.Parse(args[0]);
                yintervals = int.Parse(args[1]);
                String source = args[2];

                filename = "_" + source;
                g = new GridBase(xintervals, yintervals, source);
                g2 = new GridBase(xintervals * 2, yintervals * 2, source);
                g4 = new GridBase(xintervals * 4, yintervals * 4, source);
            }
            else return;

            Bitmap b = g.b(); b.Save("g+" + filename);
            Bitmap red = g.b(0); red.Save("red" + filename);
            Bitmap green = g.b(1); green.Save("green" + filename);
            Bitmap blue = g.b(2); blue.Save("blue" + filename);
            Bitmap color2 = g.mix(1, ref red, 1, ref green);
            Bitmap color3 = g.mix(1, ref color2, 1, ref blue);
            color3.Save("red+green+blue" + filename);

            Bitmap mix = g.mixwith(2, ref b, 3, ref g2);
            mix.Save("g+g2" + filename);
            mix = g.mixwith(1, ref mix, 1, ref g4);
            mix.Save(filename);
            mix = g.mixwith(1, ref mix, 1, ref g4);
            mix.Save("g+g4" + filename);

            Console.WriteLine(xstep + " " + xintervals + " " + ystep + " " + yintervals + "\n Hello World!");
        }
    }
}
