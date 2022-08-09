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
using TahsinsLibrary.Geometry;
namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (byte b in Image.BMP.GetBMPHeader(new Color[4, 4])) Console.Write(b + ",");
        }
    }
}
