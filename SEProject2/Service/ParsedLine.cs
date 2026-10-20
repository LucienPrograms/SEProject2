using System;
using System.Collections.Generic;
using System.Text;

namespace SEProject2.Service
{
    public class ParsedLine
    {
        public int Indent { get; set; }
        public string Raw { get; set; } = string.Empty;
        public string[] Parts { get; set; } = System.Array.Empty<string>();
        
    }
}
