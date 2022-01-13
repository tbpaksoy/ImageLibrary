using System;
using System.Globalization;
using TahsinsLibrary.String;
using TahsinsLibrary.Calculation;
using System.Collections.Generic;
using System.IO;
using TahsinsLibrary;
using TahsinsLibrary.Analyze;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {

        string[] data = Image.BMP.CreatePalette(new Color[]{Color.blue,Color.lime,new Color(255,192,203,255),Color.green},3,3,50);
        byte[] byteData = CustomCalculation.ToByteArray(data);
        /*
        Analyze.AnalyzeByteArray(File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\refrence.bmp"),true);
        Console.WriteLine("--------");
        Analyze.AnalyzeByteArray(File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\test0.bmp"),true);
        */
        //Analyze.AnalyzeByteArray(byteData,true);
        //Analyze.AnalyzeByteArray(File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\refrence3.bmp"),true);
        
        FileStream fs = new FileStream("C:\\Users\\Tahsin\\Desktop\\Image\\pallette.bmp",FileMode.Create);

        fs.Write(byteData);
        fs.Flush();
        fs.Close();
        
        /*
        byte[] paletteRefrence = File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\palletteTest.bmp"); 
        int start = int.Parse("00",NumberStyles.HexNumber);
        Compare.CompareArrays(paletteRefrence,byteData,true,start,start+54);
        int index = int.Parse("22",NumberStyles.HexNumber);
        Console.WriteLine(paletteRefrence[index].ToString("X2")+" "+paletteRefrence[index+1].ToString("X2")+" "+paletteRefrence[index+2].ToString("X2"));
        Console.WriteLine(byteData[index].ToString("X2")+" "+byteData[index+1].ToString("X2")+" "+byteData[index+2].ToString("X2"));
        Console.WriteLine(paletteRefrence.Length +" "+ byteData.Length);*/
        //Analyze.AnalyzeByteArray(byteData,54,true);
        /*
        Console.WriteLine("refrence0");
        byte[] refrenceData = File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\refrence0.bmp"); 
        Analyze.AnalyzeByteArray(refrenceData,true);

        Console.WriteLine("refrence1");
        refrenceData = File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\refrence1.bmp");
        Analyze.AnalyzeByteArray(refrenceData,true);

        Console.WriteLine("refrence2");
        refrenceData = File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\refrence2.bmp");
        Analyze.AnalyzeByteArray(refrenceData,true);

        Console.WriteLine("refrence2");
        refrenceData = File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\refrence3.bmp");
        Analyze.AnalyzeByteArray(refrenceData,true);
        */
        //Compare.CompareArrays(File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\refrence4.bmp"),byteData,true);
        /*
        for (int i = 0; i < result.Length; i++)
        {
            if(!result[i])
            {
                Console.WriteLine(i.ToString("X2") + "->" + (i < refrenceData.Length?refrenceData[i].ToString("X2"):"Null") +"!="+ (i < byteData.Length?byteData[i].ToString("X2"):"Null"));

            }
        }
        */
        /*
        for (int i = int.Parse("12",NumberStyles.HexNumber); i < int.Parse("16",NumberStyles.HexNumber); i++)
        {
            Console.WriteLine(i.ToString("X2")+"->"+refrenceData[i].ToString("X2")+"--"+byteData[i].ToString("X2"));
        }
        */
        //Compare.CompareArrays<byte>(refrenceData,byteData,true);
        //(List<string>,List<int>)analyze0 = Analyze.AnalyzeByteArray(refrenceData, true);
        //(List<string>,List<int>)analyze1 = Analyze.AnalyzeByteArray(byteData, true);
        /*
        Console.WriteLine(refrenceData.Length+" "+byteData.Length);
        for (int i = 0; i < analyze1.Item1.Count; i++)
        {
            string result0 = "    ";
            if(i < analyze0.Item1.Count)
            {
                result0 = analyze0.Item1[i].ToString() + "--" + analyze0.Item2[i].ToString();
            }
            string result1 = analyze1.Item1[i] + "--" + analyze1.Item2[i];

            Console.WriteLine(result0 + " , " + result1);
        }*/
        
        //Analyze.AnalyzeByteArray(File.ReadAllBytes("C:\\Users\\Tahsin\\Desktop\\Image\\test1.bmp"),true);
    }
}

