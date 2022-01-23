using System;
using System.Globalization;
using TahsinsLibrary.String;
using TahsinsLibrary.Calculation;
using System.Collections.Generic;
using System.IO;
using TahsinsLibrary;
using TahsinsLibrary.Analyze;
using System.Linq;
using TahsinsLibrary.Array;

class Program
{
    public static void Main(string[] args)
    {   
        string fileName = Console.ReadLine();
        string[] data = Image.BMP.CreateMidColorTable(new Color[]{Color.red,Color.green,Color.blue},25);
        byte[] byteData = CustomCalculation.ToByteArray(data);

        int index = 0;
        while(File.Exists($"C:\\Users\\Tahsin\\Desktop\\Image\\{fileName}{index}.bmp")) index++;

        FileStream fs = new FileStream($"C:\\Users\\Tahsin\\Desktop\\Image\\{fileName}{index}.bmp",FileMode.Create);
        fs.Write(byteData);
        fs.Flush();
        fs.Close();

    }
}

