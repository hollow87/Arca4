using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace arca4
{
    class CommandObject
    {
        public UserObject target { get; set; }
        public String args { get; set; }
        public Boolean bot { get; set; }

        public CommandObject()
        {
            this.target = null;
            this.args = String.Empty;
        }
    }
}
