using System;
using System.Globalization;
using TahsinsLibrary.String;
using TahsinsLibrary.Calculation;
using System.Collections.Generic;
using System.IO;
using TahsinsLibrary;
using System.Linq;
    class Program
    {
        static void Main(string[] args)
        {

            string[] data = Image.BMP.CreateData(50,50,c=>new Color((byte)(255),(byte)(212),(byte)(71),(byte)(255)));
            byte[] byteData = CustomCalculation.ToByteArray(data);

            FileStream fs = new FileStream("C:\\Users\\Tahsin\\Desktop\\Image\\test1.bmp",FileMode.Create);

            fs.Write(byteData);
            fs.Flush();
            fs.Close();
        }
    }

