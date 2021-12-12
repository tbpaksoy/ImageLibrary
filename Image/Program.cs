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

            string[] data = Image.BMP.CreateData(50,50,c=>new Color((byte)(c),(byte)(c),(byte)(c),(byte)(c)));
            byte[] byteData = CustomCalculation.ToByteArray(data);
            /*for (int i = 14; i < 18; i++)
            {
                Console.Write(byteData[i].ToString("X2")+" ");
            }*/
            Console.WriteLine();
            byte[] original = File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\zxc.bmp");
            /*for (int i = 14; i < 18; i++)
            {
                Console.Write(original[i].ToString("X2")+" ");
            }*/
            for (int i = 2; i < 6; i++)
            {
                Console.WriteLine(original[i].ToString("X2")+" "+byteData[i].ToString("X2"));
            }
            CustomCalculation.Length=6;

            Console.WriteLine((byteData.Length ==  original.Length) + " " + byteData.Length + " " + original.Length);

            /*foreach(byte b in byteData)
            {
                Console.Write(b.ToString("X2")+" ");
            }
            Console.WriteLine();
            Console.WriteLine(byteData.Length);*/
            FileStream fs = new FileStream("C:\\Users\\Tahsin\\Desktop\\Image\\test0.bmp",FileMode.Create);
            fs.Write(byteData);
            fs.Flush();
            fs.Close();
        }
    }

