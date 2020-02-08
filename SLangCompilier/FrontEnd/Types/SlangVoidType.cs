using System;
using System.Collections.Generic;
using System.Text;

namespace SLangCompiler.FrontEnd.Types
{
    public class SlangVoidType : SlangType
    {
        public override string ToString() => "void";

        public override bool Equals(SlangType other)
        {
            return false;
        }
    }
}
