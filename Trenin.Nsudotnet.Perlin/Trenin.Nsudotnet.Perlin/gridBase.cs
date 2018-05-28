using System;
using System.Text;
using System.Drawing;
using System.Windows.Controls;

namespace Trenin.Nsudotnet.Perlin
{
 class GridBase
 {
     private int p1;
     private int p2;
     private Color color;
     private int xstep;
     private int xintervals;
     private int ystep;
     private int yintervals;

     public GridBase(int p1, int p2, Color color)
     {
         // TODO: Complete member initialization
         this.p1 = p1;
         this.p2 = p2;
         this.color = color;
     }

     public GridBase(int xstep, int xintervals, int ystep, int yintervals, Color color)
     {
         // TODO: Complete member initialization
         this.xstep = xstep;
         this.xintervals = xintervals;
         this.ystep = ystep;
         this.yintervals = yintervals;
         this.color = color;
     }


     internal GridBase coarse(int p)
     {
         throw new NotImplementedException();
     }

     internal void draw()
     {
         throw new NotImplementedException();
     }

     internal Bitmap b()
     {
         throw new NotImplementedException();
     }

     internal GridBase coarse(int p1, int p2)
     {
         throw new NotImplementedException();
     }

     internal Bitmap mixwith(int p1, ref Bitmap mix, int p2, ref GridBase g4, int p3)
     {
         throw new NotImplementedException();
     }

     internal Bitmap mixwith(int p1, ref Bitmap mix, int p2, ref GridBase g4)
     {
         throw new NotImplementedException();
     }

     internal Bitmap b(int p)
     {
         throw new NotImplementedException();
     }
 

  public static void Main1 (string[] args)
  {
   GridBase g, g2, g4;

   int xstep = 256;
   int xintervals = 1;
   int ystep = 256;
   int yintervals = 1;
   string filename = "";

   xstep = int.Parse (args [0]);
   xintervals = int.Parse( args[1]);
   ystep = int.Parse( args[2]);
   yintervals = int.Parse( args[3]);
   filename = args[4];

   g = new GridBase( xstep, xintervals, ystep, yintervals, Color.LightPink );
   g2 = new GridBase( xstep/2, xintervals*2, ystep/2, yintervals*2, Color.Azure );
   g4 = g.coarse( 2, 2 );

   Bitmap b = g.b();
   Bitmap red = g.b(0); red.Save ("red" + filename);
   Bitmap green = g.b(1); green.Save ("green" + filename);
   Bitmap blue = g.b(2); blue.Save ("blue" + filename);

   Bitmap mix = g.mixwith (1, ref b, 1, ref g2, 1);
   mix.Save ("g+g2" + filename);
   mix = g.mixwith (1, ref mix, 1, ref g4, 1);
   mix.Save ("g+g2+g4" + filename);
   mix = g.mixwith (0, ref mix, 1, ref g4);
   mix.Save ("g4_" + filename);

   Console.WriteLine (xstep + " " + xintervals + " " + ystep + " " + yintervals + "\n Hello World!");
  }
 }
}
