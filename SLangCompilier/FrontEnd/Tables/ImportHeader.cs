using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Tables
{
    public class ImportHeader
    {
        public string File { get; set; }
        public string Ident { get; set; }

        public ImportHeader(string file, string ident)
        {
            File = file;
            Ident = ident;
        }
    }
}
