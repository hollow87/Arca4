using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arca4
{
    class Font
    {
        public String Name { get; set; }
        public byte Size { get; set; }
        public byte NameColor { get; set; }
        public byte TextColor { get; set; }

        public Font(byte size, String name, byte nc, byte tc)
        {
            this.Name = name;
            this.Size = size;

            if (this.Name == "Cambria Math")
            {
                this.Name = "Verdana";
                this.Size = 10;
            }

            this.NameColor = nc;
            this.TextColor = tc;
        }
    }
}
