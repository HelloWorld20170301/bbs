using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace exam
{
    class Program
    {
        public static string Apath = @"..\..\A.txt";
        public static string Bpath    = @"..\..\B.txt";
        static void Main(string[] args)
        {
            Step(Apath, Bpath);
            Console.WriteLine("已将文本加密到B.txt");
            File.WriteAllText(Apath, "");
            Console.WriteLine("文件A.txt内容清除成功");
            Step(Bpath, Apath);
            Console.WriteLine("已将文本解密到A.txt");



            //string ss = "Students view plasticine giant panda models at Liaodu Chinese School, in Vientiane, Laos, Nov 28, 2016. Panda Chengdu Culture Trip was held in Liaodu Chinese School on Monday. Teachers from Chengdu in many fields, such as traditional Chinese handicrafts, Traditional Chinese Medicine treatment, Chinese calligraphy, Chinese folk music, gave lessons and communicated with students.";
            //int count = 0;
            //foreach (string str in ss.Split(' ', '.', ','))
            //{
            //    if (str == "Chinese")
            //    {
            //        count++;
            //    }
            //}
            //Console.WriteLine("'Chinese'单词出现的次数为:" + count);

            Console.Read();
        }

        public static void Step(string readpath, string writepath)
        {
            string input = String.Empty;
            StreamReader reader = new StreamReader(readpath);
            StreamWriter writer = new StreamWriter(writepath);
            while ((input = reader.ReadLine()) != null)
            {
                writer.WriteLine(mi(input));
            }
            reader.Close();
            writer.Close();
        }

        public static string mi(string text)
        {
            char[] char_array = text.ToCharArray();
            char a = '#';
            int i = 0;
            foreach (char ch in char_array)
            {
                char_array[i++] = (char)(ch ^ a);
            }
            return string.Concat(char_array);
        }
    }


    public class point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public point() { }
    }
    public class line : point
    {
        public point Point { get; set; }
        public double X2
        {
            get { return Point.X; }
            set { Point.X = value; }
        }
        public double Y2
        {
            get { return Point.Y; }
            set { Point.Y = value; }
        }

        public line()
        {
            Point = new point();
        }

        public line(point p1, point p2)
        {
            base.X = p1.X;
            base.Y = p1.Y;
            Point = p2;
        }

        public double Length()
        {
            return Math.Sqrt((X2 - X) * (X2 - X) + (Y2 - Y) * (Y2 - Y));
        }
    }
    public class rect : line
    {
        public double ZC()
        {
            return 2 * (Math.Abs(X2 - X) + Math.Abs(Y2 - Y));
        }
        public double MJ()
        {
            return Math.Abs((X2 - X) * (Y2 - Y));
        }
    }
}
