using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arca4
{
    class Program
    {
        static Arca arca;

        static void Main()
        {
            Console.Title = "Arca4 dev console";
            arca = new Arca();
            arca.Start();
        }
    }
}
