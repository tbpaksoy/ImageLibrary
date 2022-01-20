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
        Random random = new Random();
        List<Color> from = new List<Color>();
        List<Color> to = new List<Color>();
        for (int i = 0; i < 10; i++)
        {
            from.Add(new Color((byte)random.Next(),(byte)random.Next(),(byte)random.Next()));
            to.Add(new Color((byte)random.Next(),(byte)random.Next(),(byte)random.Next()));
        }
        string[] data = Image.BMP.CreateColorTransition(new Color[]{Color.black,Color.gold,Color.lavender,Color.red,Color.indigo},
         new Color[]{Color.white,Color.cyan,Color.blue,Color.pink,Color.lime},50,10);
        byte[] byteData = CustomCalculation.ToByteArray(data);

        FileStream fs = new FileStream("C:\\Users\\Tahsin\\Desktop\\Image\\ColorTransition.bmp",FileMode.Create);
        fs.Write(byteData);
        fs.Flush();
        fs.Close();

    }
}

