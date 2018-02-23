using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSpiderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            WebSpiderManger tempManger = new WebSpiderManger("http://www.cnblogs.com/Yesi/p/6757884.html", "");
            tempManger.DoSpider();
            Console.WriteLine(tempManger.LstResult.Count.ToString());
            foreach (var oneResult in tempManger.LstResult)
            {
                Console.WriteLine(oneResult.NowTitle);
            }
            Console.ReadKey();
        }
    }
}
