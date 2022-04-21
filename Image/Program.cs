using System;
using System.Collections.Generic;
using TahsinsLibrary.Analyze;
using TahsinsLibrary.Collections;
using TahsinsLibrary;
using System.Diagnostics;
using TahsinsLibrary.Calculation;
using System.IO;
namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Color[,] colors = new Color[3,3];
            colors[0,1] = Color.auburn;
            colors[2,2] = Color.bronze;
            string [] palette = Image.BMP.CreatePalette(colors);
            FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)+"\\a.bmp",FileMode.CreateNew);
            fs.Write(CustomCalculation.ToByteArray(palette));
            fs.Flush();
            fs.Close();
        }
    }
}
