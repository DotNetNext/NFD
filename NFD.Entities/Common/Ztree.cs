using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFD.Entities.Common
{
    public class Ztree
    {
        public int id { get; set; }
        public string url { get; set; }
        public string target { get; set; }
        public int pId { get; set; }
        public string name { get; set; }
        public string open { get; set; }
        public string iconOpen { get; set; }
        public string iconClose { get; set; }
        public bool @checked { get; set; }
    }
}
