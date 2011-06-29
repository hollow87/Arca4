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
            arca = new Arca();
            arca.Start();
        }
    }
}
