using System;
using System.Collections.Generic;
using TahsinsLibrary.Analyze;
using TahsinsLibrary.Collections;
using TahsinsLibrary;
using System.Diagnostics;
using TahsinsLibrary.Calculation;
using TahsinsLibrary.Array;
using TahsinsLibrary.String;
using System.IO;
namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Color[,] colors = new Color[1,1];
            colors[0,0] = Color.white;
            string [] palette = Image.BMP.CreatePalette(colors,100,100);
            FileStream fs = new FileStream(Directory.GetCurrentDirectory()+"\\a.bmp",FileMode.CreateNew);
            fs.Write(CustomCalculation.ToByteArray(palette));
            fs.Flush();
            fs.Close();*/
            Color[,] cols = new Color[10,10];
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    cols[i,j] = Color.white;
                }
            }
            foreach(string s in Image.BMP.GenerateColorMatrix(10,Image.ToSingleDimension(cols)))
            {
                Console.Write(s+" ");
            }
        }
    }
}
